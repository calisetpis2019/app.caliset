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
                        <FormItem label="Documento" prop="document">
                            <input :readonly="true" v-model="user.document" style="width:100%"></input>
                        </FormItem>
                        <FormItem label="Teléfono" prop="phone">
                               <input :readonly="true" v-model="user.phone" style="width:100%"></input>
                        </FormItem>
                        <FormItem label="Fecha Nacimiento" prop="birthDate">
                            <input :readonly="true" v-model="user.birthDate" type="datetime" style="width:100%"></input>
                        </FormItem>
                        <FormItem label="Ciudad" prop="city">
                            <input :readonly="true" v-model="user.city" style="width:100%"></input>
                        </FormItem>
                        <FormItem label="Dirección" prop="adress">
                            <input :readonly="true" v-model="user.adress" style="width:100%"></input>
                        </FormItem>
                        <FormItem label="Especialidad" prop="speciality">
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
    </div>
</template>
<script lang="ts">

    import { Component, Vue,Inject, Prop,Watch } from 'vue-property-decorator';
    import Util from '../../../lib/util'
    import AbpBase from '../../../lib/abpbase'
    import User from '../../../store/entities/user'
    import moment from 'moment'

    @Component
    export default class ViewUser extends AbpBase{


        @Prop({type:Boolean,default:false}) value:boolean;
        user:User=new User();

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
                title:'Fecha creación',
                render:(h:any,params:any)=>{
                   return h('span',moment(params.row.operation.date).locale('es').format("DD/MM/YYYY, HH:mm"))
                }
            },
            {
                title:'Comienzo',
                render:(h:any,params:any)=>{
                   return h('span',moment(params.row.date).locale('es').format("DD/MM/YYYY, HH:mm"))
                }
            },
            {
                title:'Fin',
                render:(h:any,params:any)=>{
                   return h('span',moment(params.row.dateFin).locale('es').format("DD/MM/YYYY, HH:mm"))
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
            }
        ]

    }
</script>