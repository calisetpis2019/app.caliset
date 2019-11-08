<template>
    <div>
        <Modal :title="L('Datos de la Operación')"
               :value="value"
               @on-visible-change="visibleChange"
               width="90%">
               <Table :loading="loadingOperation" :columns="columnsInfoOperacion" no-data-text="No existe informacion." border :data="operationDetails" size="small" style="margin-bottom:20px;">
              </Table>
              <Table :loading="loadingOperation" :columns="columnsInfoOperacion_2" no-data-text="No existe informacion." border :data="operationDetails" size="small">
              </Table>
              <Divider />
               <Tabs value="assignations" type="card">
                    <TabPane label="Asignaciones" name="assignations">
                        <Table :loading="loadingAssignation" :columns="columnsAssignation" no-data-text="No existen asignaciones" border :data="assignations" v-if="operatorRenderOnly"></Table>
                    </TabPane>
                    <TabPane label="Comentarios" name="comments">
                       <Table :loading="loadingAssignation" :columns="columnsComments" no-data-text="No existen comentarios" border :data="comments" v-if="operatorRenderOnly"></Table>
                    </TabPane>
                    <TabPane label="Muestras" name="samples">
                        <Table :loading="loadingAssignation" :columns="columnsSamples" no-data-text="No existen muestras" border :data=operationReponse.samples v-if="operatorRenderOnly"></Table>
                    </TabPane>
                    <TabPane label="Fotos" name="photos">
                        <Button @click="openGooglePhotos" long>Ver fotos de la operacion</Button>
                        <Table :loading="loadingAssignation" :columns="columnsCredentials" no-data-text="No existen asignaciones" border :data="assignations" v-if="operatorRenderOnly"></Table>
                    </TabPane>
                </Tabs>
            <div slot="footer">
            </div>
        </Modal>
        <edit-comment-operation v-model="editCommentOperationModalShow" @save-success="visibleChange(true)"></edit-comment-operation>
        <view-user v-model="viewUserAssignedToOperationModalShow" ></view-user>
    </div>
</template>

<script lang="ts">
    import { Component, Vue,Inject, Prop,Watch } from 'vue-property-decorator';
    import Util from '@/lib/util'
    import AbpBase from '@/lib/abpbase'
    import UserRequest from '@/store/entities/user-request'
    import Operation from '@/store/entities/operation'
    import Client from '@/store/entities/client'
    import User from '@/store/entities/user'
    import Location from '@/store/entities/location'
    import OperationState from '@/store/entities/operationState'
    import Assignation from '@/store/entities/assignation'
    import Comment from '@/store/entities/comment'
    import EditCommentOperation from './edit-comment-operation.vue'
    import ViewUser from '../setting/user/view-user.vue'
    import moment from 'moment'

    class PageViewOperationRequest extends UserRequest {
    }

    @Component({
        components: { EditCommentOperation, ViewUser }
    })
    export default class ViewOperation extends AbpBase{

        @Prop({type:Boolean,default:false}) value:boolean;
        operation:Operation=new Operation();
        operationReponse:Operation=new Operation();
        user:User=new User();

        pagerequest: PageViewOperationRequest = new PageViewOperationRequest();
        operatorRenderOnly: boolean = Util.abp.auth.hasPermission('Pages.Operador');

        location:Location = new Location();

        editCommentOperationModalShow: boolean = false;
        viewUserAssignedToOperationModalShow: boolean = false;
        // usersAssigned:Array<User> = new Array<User>();

        get loadingAssignation() {
            return this.$store.state.assignation.loading;
        }

        get assignations() {
            return this.$store.state.assignation.assignmentsOfOperation;
        }

        get comments() {
            this.$store.commit('comment/setOperationStateId',this.operation.operationStateId);
            return this.$store.state.comment.commentsOfOperation; 
        }

        get loadingOperation() {
            return this.$store.state.operation.loading && this.$store.state.location.loading;
        }

        get operationDetails() {
            var obj: {[k: string]: any} = this.operation;
            obj.location = this.location.name;
            return new Array<Object>(obj);
        }

        visibleChange(value:boolean){
            if(!value){
                this.$emit('input',value);
            }
            else {
                this.operation = Util.extend(true, {}, this.$store.state.operation.viewOperation);
                this.pagerequest["id"] = this.operation.locationId;
                this.getLocation().then(result => {
                    this.getAssignations();

                    this.pagerequest["id"] = this.operation.id;
                    this.getComments();

                    this.pagerequest["id"] = this.operation.id;
                    this.getOperation();
                });
            }
        }

        async getOperation() {
            this.operationReponse = await this.$store.dispatch({
                type: 'operation/get',
                data: this.pagerequest
            })
        }

        async getLocation() {
            this.location = await this.$store.dispatch({
                type: 'location/get',
                data: this.pagerequest
            })
        }

        async getAssignations() {
            this.pagerequest["id"] = this.operation.id;
            await this.$store.dispatch({
                type: 'assignation/getAssignmentsByOperation',
                data: this.pagerequest
            })

        }

        async getComments() {
            await this.$store.dispatch({
                type: 'comment/getCommentsByOperation',
                data: this.pagerequest
            })
        }

        async getUser(){
            let user = await this.$store.dispatch({
                type: 'user/get',
                data: this.pagerequest
            })
            return user
        }

        async deleteAssignation(){
            await this.$store.dispatch({
                type: 'assignation/delete',
                data: this.pagerequest
            })
            this.getAssignations();
        }

        commentEdit(){
            this.editCommentOperationModalShow = true;
        }

        viewUserDetail(){
            this.viewUserAssignedToOperationModalShow = true;
        }

        openGooglePhotos() {
            window.open("https://accounts.google.com/signin/v2/identifier?continue=https%3A%2F%2Fphotos.google.com%2Fsearch%2FOperacion" + this.operation.id + "&flowName=GlifWebSignIn&flowEntry=AddSession", "_blank");
        }

        columnsInfoOperacion = [
            {
                title: 'Id',
                key: 'id'
            },
            {
                title: 'Tipo',
                key: 'operationType'
            },
            {
                title: 'Nominador',
                key: 'nominatorName'
            },
            {
                title: 'Cargador',
                key: 'chargerName'
            },
            {
                title: 'Lugar',
                key: 'location'
            },
            {
                title: 'Fecha inicio',
                render:(h:any,params:any)=>{
                    return h('Span', moment(params.row.date).locale('es').format("DD/MM/YYYY, HH:mm"));
                }
            },
            {
                title: 'Responsable',
                key: 'managerName'
            },
            {
                title: 'Mercaderia',
                key: 'commodity'
            },
            {
                title: 'Empaque',
                key: 'package' 
            }

        ]
        columnsInfoOperacion_2 = [
            {
                title: 'Nombre del barco',
                key: 'shipName'
            },
            {
                title: 'Destino',
                key: 'destiny'
            },
            {
                title: 'Referencia del cliente',
                key: 'clientReference'
            },
            {
                title: 'Línea',
                key: 'line'
            },
            {
                title: 'Número de Booking',
                key: 'bookingNumber'
            },
            {
                title: 'Notas',
                key: 'notes'
            }
        ]

        columnsAssignation = [
            {
                title: 'Nombre',
                key: 'name',
                render:(h:any,params:any)=>{
                  return h('Span', params.row.inspector.name);
                }
            },
            {
                title: 'Apellido',
                key: 'surname',
                render:(h:any,params:any)=>{
                  return h('Span', params.row.inspector.surname);
                }
            },
            {
                title: 'Fecha inicio',
                render:(h:any,params:any)=>{
                    return h('Span', moment(params.row.date).locale('es').format("DD/MM/YYYY, HH:mm"));
                }
            },
            {
                title: 'Telefono',
                key: 'phone',
                render:(h:any,params:any)=>{
                  return h('Span', params.row.inspector.phone);
                }
            },
            {
                title: 'Estado',
                key: 'surname',
                render:(h:any,params:any)=>{
                  if (params.row.aware == null){
                      return h('Span', 'Pendiente');
                  } else if (params.row.aware) {
                      return h('Span', 'Confirmada');
                  } else {
                      return h('Span', 'Rechazada');
                  }
                }
            },
            {
                title:this.L('Actions'),
                key:'Actions',
                width:140,
                render:(h:any,params:any)=>{
                    var toRender = [
                            h('Button',{
                                props:{
                                    type: 'info',
                                    size:'small'
                                },
                                style:{
                                    marginRight:'5px'
                                },
                                on:{
                                    click:()=>{
                                        this.pagerequest.id = params.row.inspector.id;
                                        this.getUser().then(result =>{
                                            this.$store.commit('user/view',result);
                                            this.viewUserDetail();
                                        });
                                    }
                                }
                            },'Ver'),
                            h('Button',{
                                props:{
                                    type: 'info',
                                    size:'small'
                                },
                                style:{
                                    marginRight:'5px'
                                },
                                on:{
                                    click:()=>{
                                        this.pagerequest.id = params.row.id;
                                        this.deleteAssignation();
                                    }
                                }
                            },'Quitar')
                        ]
                    
                    return toRender;
                }
            }]        

        columnsSamples =[
            {
                title: 'Código Muestra',
                key: 'id'
            },
            {
                title: 'Comentario',
                key: 'comment'
            },
            {
                title: 'Fecha',
                render:(h:any,params:any)=>{
                    return h('Span', moment(params.row.creationTime).locale('es').format("DD/MM/YYYY, HH:mm"));
                }
            },
            {
                title: 'Creador',
                render:(h:any,params:any)=>{
                    return h('Span', params.row.inspector.name+" "+params.row.inspector.surname);    
                }
            },

        ]

        columnsComments = [
            {
                title: 'Comentario',
                key: 'commentary'
            },
            {
                title: 'Fecha inicio',
                render:(h:any,params:any)=>{
                    return h('Span', moment(params.row.creationTime).locale('es').format("DD/MM/YYYY, HH:mm"));
                }
            },
            {
                title: 'Creador',
                render:(h:any,params:any)=>{
                    return h('Span', params.row.creatorUser.name+" "+params.row.creatorUser.surname);
                }
            },
            {
                title:this.L('Actions'),
                key:'Actions',
                width:100,
                render:(h:any,params:any)=>{
                    var toRender;
                    if(params.row.creatorUser.id === this.$store.state.session.user.id && this.$store.state.comment.operationStateId!==3){
                        toRender = [
                            h('Button',{
                                props:{
                                    type: 'success',
                                    size:'small'
                                },
                                style:{
                                    marginRight:'5px'
                                },
                                on:{
                                    click:()=>{
                                        this.$store.commit('comment/edit',params.row);
                                        this.commentEdit();
                                    }
                                }
                            },'Editar')
                        ]
                    }
                    return toRender;
                }
            }
        ]

        columnsCredentials = [
            {
                title: 'Usuario',
                key: 'user',
                render:(h:any,params:any)=>{
                    return h('Span', params.row.inspector.cuentaGP);
                }
            },
            {
                title: 'Contraseña',
                key: 'password',
                render:(h:any,params:any)=>{
                    return h('Span', params.row.inspector.passwordGP);
                }
            }
        ]
    }
</script>
