<template>
    <div>
        <Modal :title="L('Asignar Usuario')"
               :value="value"
               @on-ok="save"
               @on-visible-change="visibleChange">
            <Form ref="assignationForm" label-position="top" :rules="assignationRule" :model="assignation">
                <FormItem label="Inspector" prop="inspectorId">
                    <Select v-model="assignation.inspectorId" style="padding: 10px 0px 20px 0px;" filterable :value="this.operation.managerId">
                        <Option v-for="item in listOfUsers" :value="item.id" :key="item.id">{{ item.name }}</Option>
                    </Select>
                </FormItem>

                <FormItem label="Fecha y hora de inicio" prop="date">
                    <VueCtkDateTimePicker label="Seleccione" hint=" " v-model="assignation.date" locale="es" v-bind:right="true" />
                </FormItem>

                <FormItem label="Fecha y hora de fin" prop="finalDate">
                    <VueCtkDateTimePicker label="Seleccione" hint=" " v-model="assignation.finalDate" locale="es" v-bind:right="true" />
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
            var result = [];
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
            return result;
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
                }
            })
        }
        cancel(){
            (this.$refs.assignationForm as any).resetFields();
            this.$emit('input',false);
        }
        visibleChange(value:boolean){
            if(!value){
                this.$emit('input',value);
            }
            else{
                this.operation = Util.extend(true, {}, this.$store.state.operation.editOperation);
            }
            this.getUsers()
        }

        async getUsers() {
            await this.$store.dispatch({
                type: 'user/getAll',
                data: this.pagerequest
            })
        }

        assignationRule={
            date:[
                {required: true,message:this.L('FieldIsRequired',undefined,this.L('Fecha y hora')),trigger: 'blur'}
            ],
            inspectorId:[
                {type: "number", required: true,message:this.L('FieldIsRequired',undefined,this.L('Responsable')),trigger: 'blur'}
            ]
        }
    }
</script>
