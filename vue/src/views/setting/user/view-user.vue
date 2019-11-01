<template>
    <div>
        <Modal
         title="Ver Usuario"
         :value="value"
         @on-visible-change="visibleChange" 
         width="90%"       >
            <Form ref="userForm"  label-position="top" :model="user">
                <h2> {{user.name}} {{user.surname}} </h2>
                <Tabs value="detail">
                    <TabPane :label="L('Details')" name="detail">
                        <FormItem :label="L('EmailAddress')" prop="emailAddress">
                            <input :readonly="true" v-model="user.emailAddress" type="email" :maxlength="32" style="width:100%"></input>
                        </FormItem>
                        <FormItem :label="L('Name')" prop="name">
                            <input :readonly="true" v-model="user.name" style="width:100%"></input>
                        </FormItem>
                        <FormItem :label="L('Surname')" prop="surname">
                            <input :readonly="true" v-model="user.surname" style="width:100%"></input>
                        </FormItem>
                        <FormItem :label="L('Document')" prop="document">
                            <input :readonly="true" v-model="user.document" style="width:100%"></input>
                        </FormItem>
                        <FormItem :label="L('Phone')" prop="phone">
                            <input :readonly="true" v-model="user.phone" style="width:100%"></input>
                        </FormItem>
                        <FormItem :label="L('BirthDate')" prop="birthDate">
                            <input :readonly="true" v-model="user.birthDate" type="datetime" style="width:100%"></input>
                        </FormItem>
                        <FormItem :label="L('City')" prop="city">
                            <input :readonly="true" v-model="user.city" style="width:100%"></input>
                        </FormItem>
                        <FormItem :label="L('Adress')" prop="adress">
                            <input :readonly="true" v-model="user.adress" style="width:100%"></input>
                        </FormItem>
                        <FormItem :label="L('Speciality')" prop="speciality">
                            <input :readonly="true" v-model="user.specialty" style="width:100%"></input>
                        </FormItem>
                        <FormItem>
                            <h2 v-if="user.isActive" style="color:green;">{{L('IsActive')}}</h2>
                            <h2 v-else style="color:red;" >Inactivo </h2>
                        </FormItem>
                    </TabPane>
                    <TabPane :label="L('Roles')" name="roles">
                        <ul>
                            <li v-for="role in roles" style="font-size: 20px;margin-left: 30px">
                                {{role.name}}
                            </li>
                        </ul>
                    </TabPane>
                    <TabPane :label="L('Asignaciones')" name="assignations">
                        <!-- Aca van las asignaciones del usuario sobre operaciones -->
                        <Table  :loading="loading" 
                                :columns="assColumns"
                                no-data-text="No existen registros" 
                                border 
                                :data="assignations"></Table>

                    </TabPane>
                    <TabPane :label="L('Adjuntos')" name="attachments">
                        <!-- Aca van documentos adjuntos del usuario -->
                    </TabPane>
                </Tabs>
            </Form>
            <div slot="footer">
                <Button @click="cancel">{{L('Cancel')}}</Button>
            </div>
        </Modal>
        <view-operation-dummy v-model="viewOperationModalShow" ></view-operation-dummy>
    </div>
</template>
<script lang="ts">

    import { Component, Vue,Inject, Prop,Watch } from 'vue-property-decorator';
    import Util from '../../../lib/util'
    import AbpBase from '../../../lib/abpbase'
    import User from '../../../store/entities/user'
    import ViewOperationDummy from '../../operation/view-operation-dummy.vue'

    @Component({
        components:{ ViewOperationDummy }
    })
    export default class ViewUser extends AbpBase{


        @Prop({type:Boolean,default:false}) value:boolean;
        user:User=new User();

        viewOperationModalShow:boolean = false;

        //datos hardcodeados en el backend:
        active=2;
        future=1;


        get roles(){
            return this.$store.state.user.roles;
        }

        get loading(){
            return this.$store.state.assignation.loading;
        }

        get assignations(){

            let assignments = this.$store.state.assignation.assignmentsByUsers;

            var assignationsReturn = [];
            for(var i = 0; i < assignments.length; i++) {

                if ( (assignments[i].operation.operationState.id == this.future ) || 
                    (assignments[i].operation.operationState.id == this.active )  ) {
                    if (assignments[i].aware ==null){
                        assignments[i].aware = "Sin Respuesta";
                    }else if (assignments[i].aware) {
                        assignments[i].aware = "Confirmada";
                    }else {
                        assignments[i].aware = "Rechazada";
                    }

                    assignationsReturn.push(assignments[i]);
                }
            }

            return assignationsReturn;
        }


        cancel(){
            (this.$refs.userForm as any).resetFields();
            this.$emit('input',false);
        }
        visibleChange(value:boolean){
            if(!value){
                this.$emit('input',value);
            }else{
                this.user=Util.extend(true,{},this.$store.state.user.viewUser);
                this.$store.dispatch({
                    type: 'assignation/getAssignationsByUser',
                    data: this.user,
                });
                
            }
        }

        async getOperation(pagerequest) {
            return await this.$store.dispatch({
                type: 'operation/get',
                data: pagerequest
            })
        }

        viewOperationDetail(){
            this.viewOperationModalShow = true;
        }

        assColumns=[
            {
                title:'Operación',
                render:(h:any,params:any)=>{
                    return h('span',params.row.operation.id) 
                    }
            },
            {
                title:'Commodity',
                render:(h:any,params:any)=>{
                    return h('span',params.row.operation.commodity)
                }
            },
            {
                title:'Destino',
                render:(h:any,params:any)=>{
                   return h('span',params.row.operation.destiny)
                }
            },
            {
                title:'Fecha',
                render:(h:any,params:any)=>{
                   return h('span',new Date(params.row.operation.date).toLocaleString())
                }
            },
            {
                title:'Comienzo',
                render:(h:any,params:any)=>{
                   return h('span',new Date(params.row.date).toLocaleString())
                }
            },
            {
                title:'Fin',
                render:(h:any,params:any)=>{
                   return h('span',new Date(params.row.dateFin).toLocaleString())
                }
            },
            {
                title:'Estado',
                render:(h:any,params:any)=>{
                   return h('span',params.row.operation.operationState.name)
                }
            },
            {
                title:'Asignación',
                render:(h:any,params:any)=>{
                   return h('span',params.row.aware)
                }
            },
            {
                title:this.L('Actions'),
                key:'Actions',
                width:100,
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
                                        let pagerequest = { 
                                                id: params.row.operationId
                                            }
                                        this.getOperation(pagerequest).then(result =>{
                                            let operation = {
                                                id: result["id"],
                                                bookingNumber: result["bookingNumber"],
                                                chargerId: result["charger"]["id"],
                                                chargerName: result["charger"]["name"],
                                                clientReference: result["clientReference"],
                                                commodity: result["commodity"],
                                                date: result["date"],
                                                destiny: result["destiny"],
                                                line: result["line"],
                                                location: result["location"]["name"],
                                                locationId: result["location"]["id"],
                                                managerId: result["manager"]["id"],
                                                managerName: result["manager"]["name"],
                                                nominatorId: result["nominator"]["id"],
                                                nominatorName: result["nominator"]["name"],
                                                notes: result["notes"],
                                                operationState: result["operationState"]["name"],
                                                operationStateId: result["operationState"]["id"],
                                                operationTypeId: result["operationType"]["id"],
                                                operationType: result["operationType"]["name"],
                                                package: result["package"],
                                                shipName: result["shipName"]
                                            }
                                            this.$store.commit('operation/view',operation);
                                            this.viewOperationDetail();
                                        });
                                    }
                                }
                            },'Ver')
                        ]
                    
                    return toRender;
                }
            }
        ]

    }
</script>