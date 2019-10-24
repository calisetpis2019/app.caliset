<template>
    <div>
        <Modal :title="L('Asignar Usuario')"
               :value="value"
               :mask-closable="false"
               @on-ok="save"
               @on-visible-change="visibleChange">
            <Form ref="assignationForm" label-position="top" :rules="assignationRule" :model="assignation">
                <FormItem label="Inspector" prop="inspectorId">
                    <Select v-if="listOfUsers.length > 0" v-model="assignation.inspectorId" style="padding: 10px 0px 20px 0px;" filterable :value="this.operation.managerId">
                        <Option v-for="item in listOfUsers" :value="item.id" :key="item.id">{{ item.name+" "+item.surname }}</Option>
                    </Select>

                    <Select v-else placeholder="No se encontraron inspectores elegibles" v-model="assignation.inspectorId" style="padding: 10px 0px 20px 0px;" filterable :value="this.operation.managerId">
                        <Option v-for="item in listOfUsers" :value="item.id" :key="item.id">{{ item.name+" "+item.surname }}</Option>
                    </Select>
                </FormItem>

                <FormItem label="Fecha y hora de inicio" prop="date">
                    <VueCtkDateTimePicker label="Seleccionar" hint=" " v-model="assignation.date" locale="es" v-bind:right="true" :min-date="formattedDate" :format="'YYYY-MM-DD HH:mm'"/>
                    <!--<DatePicker type="datetime" format="dd-MM-yyyy HH:mm" v-model="assignation.date" style="width: 100%"></DatePicker>-->
                </FormItem>

                <FormItem label="Fecha y hora de fin" prop="dateFin">
                    <VueCtkDateTimePicker label="Seleccionar" hint=" " v-model="assignation.dateFin" locale="es" v-bind:right="true" :min-date="formattedDate" :format="'YYYY-MM-DD HH:mm'"/>
                </FormItem>
            </Form>
            <div slot="footer">
                <Button @click="cancel">{{L('Cancel')}}</Button>
                <Button @click="save" type="primary">{{L('OK')}}</Button>
            </div>
        </Modal>
    </div>
</template>

<script lang="ts">
    import { Component, Vue,Inject, Prop,Watch } from 'vue-property-decorator';
    import Util from '@/lib/util'
    import AbpBase from '@/lib/abpbase'
    import PageRequest from '@/store/entities/page-request'
    import Operation from '@/store/entities/operation'
    import Assignation from '@/store/entities/assignation'
    import moment from 'moment'

    class PageAssignOperationRequest extends PageRequest {
    }

    @Component
    export default class AssignOperation extends AbpBase{
        @Prop({type:Boolean,default:false}) value:boolean;
        //esta asociado con this.operation = Util.extend(true, {}, this.$store.state.operation.editOperation);
        operation:Operation=new Operation();
        //esta asociado a :model="assignation"
        assignation:Assignation=new Assignation();
        created(){ }

        pagerequest: PageAssignOperationRequest = new PageAssignOperationRequest();


        get listOfUsers() {
            return this.$store.state.user.list;
            /*var result = [];
            var auxUsuarios=this.$store.state.user.list;
            for (let i = 0; i < auxUsuarios.length; i++) {
                var roles=auxUsuarios[i]["roleNames"];
                for (let j = 0; j < roles.length; j++) {
                    if(roles[j]=="INSPECTOR"){
                        result.push(auxUsuarios[i]);
                        break;
                    }
                }
            }
            return result;*/
        }

        save() {
            this.assignation.operationId=this.operation.id;
            (this.$refs.assignationForm as any).validate(async (valid:boolean)=>{
                if (valid) {
                    await this.$store.dispatch({
                        type:'assignation/create',
                        data:this.assignation
                    });
                    (this.$refs.assignationForm as any).resetFields();
                    this.$emit('save-success');
                    this.$emit('input',false);
                    this.$Message.success({content:'Asignación creada exitosamente.',duration:3.5});
                }
            })
        }
        cancel(){
            (this.$refs.assignationForm as any).resetFields();
            this.$emit('input',false);
            
        }
        visibleChange(value:boolean){
            if(!value){
                (this.$refs.assignationForm as any).resetFields();
                this.$emit('input',value);
            }
            else{
                this.operation = Util.extend(true, {}, this.$store.state.operation.editOperation);
                this.getEligibleUsers();

            }
        }

        async getEligibleUsers() {
            await this.$store.dispatch({
                type: 'user/getEligibleUsers',
                data: ""
            })
        }

        get formattedDate(){
            const fDate = moment(this.operation.date);
            console.log(fDate.format('YYYY-MM-DD hh:mm a'))
            return fDate.format('YYYY-MM-DD hh:mm a');
        }

        validate_dates = (rule:any, value:any, callback:any) => {
            if(typeof value !== "undefined" && typeof this.assignation.date !== "undefined" && value <= this.assignation.date){
                callback(new Error(this.L('Fecha de Inicio debe ser anteior a Fecha de Fin')));
            }
            else{
                callback();
            }
        }

        assignationRule={
            date:[
                {required: true,message:this.L('FieldIsRequired',undefined,this.L('Fecha y hora')),trigger: 'blur'}
            ],
            inspectorId:[
                {type: "number", required: true,message:this.L('FieldIsRequired',undefined,this.L('Responsable')),trigger: 'blur'}
            ],
            dateFin:[
                {required:false,validator:this.validate_dates,trigger: 'blur'}
            ]
        }
    }
</script>
