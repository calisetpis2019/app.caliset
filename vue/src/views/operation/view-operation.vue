<template>
    <div>
        <Modal :title="L('Datos de la Operación')"
               :value="value"
               :width="1000"
               :fullscreen="true"
               @on-visible-change="visibleChange">
               <Row>
                   <Col span="12">
                       Tipo de operación: {{ this.operation.operationType }} 
                   </Col>
                   <Col span="12">
                       Nominador: {{ this.operation.nominatorName }} 
                   </Col>
               </Row>
               <Row>
                   <Col span="12">
                       Cargador: {{ this.operation.chargerName }} 
                   </Col>
                   <Col span="12">
                       Lugar: {{ this.location.name }} 
                   </Col>
               </Row>
               <Row>
                   <Col span="12">
                       Fecha y hora de inicio: {{ this.operation.date }} 
                   </Col>
                   <Col span="12">
                       Responsable de la operación: {{ this.operation.managerName }} 
                   </Col>
               </Row>
               <Row>
                   <Col span="12">
                       Mercaderia: {{ this.operation.commodity }} 
                   </Col>
                   <Col span="12">
                       Empaque: {{ this.operation.package }} 
                   </Col>
               </Row>
               <Row>
                   <Col span="12">
                       Nombre del barco: {{ this.operation.shipName }} 
                   </Col>
                   <Col span="12">
                       Destino: {{ this.operation.destiny }} 
                   </Col>
               </Row>
               <Row>
                   <Col span="12">
                       Referencia del cliente: {{ this.operation.clientReference }} 
                   </Col>
                   <Col span="12">
                       Línea: {{ this.operation.line }} 
                   </Col>
               </Row>
               <Row>
                   <Col span="12">
                       Número de Booking: {{ this.operation.bookingNumber }} 
                   </Col>
                   <Col span="12">
                       Notas: {{ this.operation.notes }} 
                   </Col>
               </Row>
               <Divider>Usuarios asignados</Divider>
               <Table :loading="loadingAssignation" :columns="columns" no-data-text="No existen asignaciones" border :data="usersAssigned" v-if="operatorRenderOnly"></Table>
               <Divider>Muestras</Divider>
               <Table :loading="loadingAssignation" :columns="columnsSamples" no-data-text="No existen muestras" border :data=operationReponse.samples v-if="operatorRenderOnly"></Table>
               <Divider>Comentarios</Divider>
               <Table :loading="loadingAssignation" :columns="columnsComments" no-data-text="No existen comentarios" border :data="comments" v-if="operatorRenderOnly"></Table>
            <div slot="footer">
            </div>
        </Modal>
        <edit-comment-operation v-model="editCommentOperationModalShow" ></edit-comment-operation>
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

    class PageViewOperationRequest extends UserRequest {
    }

    @Component({
        components: { EditCommentOperation }
    })
    export default class ViewOperation extends AbpBase{
        @Prop({type:Boolean,default:false}) value:boolean;
        operation:Operation=new Operation();
        operationReponse:Operation=new Operation();

        pagerequest: PageViewOperationRequest = new PageViewOperationRequest();
        operatorRenderOnly: boolean = Util.abp.auth.hasPermission('Pages.Operador');

        location:Location = new Location();

        editCommentOperationModalShow: boolean = false;
        // usersAssigned:Array<User> = new Array<User>();

        get loadingAssignation() {
            return this.$store.state.assignation.loading;
        }

        get usersAssigned() {
          return this.$store.state.assignation.usersAssignedToOperation;
        }

        get comments() {
           return this.$store.state.comment.commentsOfOperation; 
        }


        visibleChange(value:boolean){
            if(!value){
                this.$emit('input',value);
            } else {
                this.operation = Util.extend(true, {}, this.$store.state.operation.viewOperation);
            }

            this.pagerequest["id"] = this.operation.id;
            this.getOperation();

            this.pagerequest["id"] = this.operation.locationId;
            this.getLocation();

            this.pagerequest["id"] = this.operation.id;
            this.getAssignations();

            this.pagerequest["id"] = this.operation.id;
            this.getComments();


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
            await this.$store.dispatch({
                type: 'assignation/getUsersByOperation',
                data: this.pagerequest
            })

        }

        async getComments() {
            await this.$store.dispatch({
                type: 'comment/getCommentsByOperation',
                data: this.pagerequest
            })
        }

        commentEdit(){
            this.editCommentOperationModalShow = true;
        }


        columns = [
            {
                title: 'Nombre',
                key: 'name'
            },
            {
                title: 'Apellido',
                key: 'surname'
            },
            {
                title: 'Email',
                key: 'emailAddress'
            },
            {
                title: 'Telefono',
                key: 'phone'
            }]        

        columnsSamples =[
            {
                title: 'Muestras',
                key: 'comment'
            }
        ]

        columnsComments = [
            {
                title: 'Comentario',
                key: 'commentary'
            },
            {
              title:this.L('Actions'),
              key:'Actions',
              width:100,
              render:(h:any,params:any)=>{
                  var toRender;
                  if(params.row.creatorUser.id === this.$store.state.session.user.id){
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
    }
</script>
