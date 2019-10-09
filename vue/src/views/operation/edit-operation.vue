<template>
    <div>
        <Modal :title="L('EditOperation')"
               :value="value"
               @on-ok="save"
               @on-visible-change="visibleChange">
            <Form ref="operationForm" label-position="top" :rules="operationRule" :model="operation">
                <Tabs value="detail">
                    <TabPane :label="L('Details')" name="detail">
                        <FormItem label="Tipo" prop="operationTypeId">
                            <Select v-model="operation.operationTypeId" style="padding: 10px 0px 20px 0px;" filterable :value="this.operation.operationTypeId">
                                <Option v-for="item in listOfOperationTypes" :value="item.id" :key="item.id">{{ item.name }}</Option>
                            </Select>
                        </FormItem>

                        <FormItem label="Lugar" prop="locationId">
                            <Select v-model="operation.locationId" style="padding: 10px 0px 20px 0px;" filterable :value="this.operation.locationId">
                                <Option v-for="item in listOfLocations" :value="item.id" :key="item.id">{{ item.name }}</Option>
                            </Select>
                        </FormItem>

                        <FormItem label="Fecha y hora de inicio" prop="date">
                            <VueCtkDateTimePicker v-model="operation.date" locale="es" v-bind:right="true" />
                        </FormItem>

                        <FormItem label="Responsable" prop="managerId">
                            <Select v-model="operation.managerId" style="padding: 10px 0px 20px 0px;" filterable :value="this.operation.managerId">
                                <Option v-for="item in listOfUsers" :value="item.id" :key="item.id">{{ item.name }}</Option>
                            </Select>
                        </FormItem>

                        <FormItem label="Mercaderia" prop="commodity">
                            <Input v-model="operation.commodity" :maxlength="32"></Input>
                        </FormItem>
                        
                        <FormItem label="Paquete" prop="package">
                            <Input v-model="operation.package" :maxlength="32"></Input>
                        </FormItem>
                    </TabPane>
                </Tabs>
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

    class PageEditOperationRequest extends PageRequest {
    }

    @Component
    export default class EditOperation extends AbpBase{
        @Prop({type:Boolean,default:false}) value:boolean;
        operation:Operation=new Operation();
        created(){

        }

        pagerequest: PageEditOperationRequest = new PageEditOperationRequest();

        get listOfUsers() {
            return this.$store.state.user.list;
        };

        get listOfLocations() {
            return this.$store.state.location.list;
        };

        get listOfOperationTypes() {
            return this.$store.state.operationType.list;
        };

        save() {
            (this.$refs.operationForm as any).validate(async (valid:boolean)=>{
                if (valid) {
                    await this.$store.dispatch({
                        type:'operation/update',
                        data:this.operation
                    });
                    (this.$refs.operationForm as any).resetFields();
                    this.$emit('save-success');
                    this.$emit('input',false);
                }
            })
        }
        cancel(){
            (this.$refs.operationForm as any).resetFields();
            this.$emit('input',false);
        }
        visibleChange(value:boolean){
            if(!value){
                this.$emit('input',value);
            } else {
                this.operation = Util.extend(true, {}, this.$store.state.operation.editOperation);
            }
            this.getOperationTypes()
            this.getLocations()
            this.getUsers()
        }

        async getOperationTypes() {

            await this.$store.dispatch({
                type: 'operationType/getAll',
                data: this.pagerequest
            })

        }

        async getLocations() {

            await this.$store.dispatch({
                type: 'location/getAll',
                data: this.pagerequest
            })

        }

        async getUsers() {
            await this.$store.dispatch({
                type: 'user/getAll',
                data: this.pagerequest
            })

        }

        operationRule={
            operationTypeId :[
                { type: "number", required: true,message:this.L('FieldIsRequired',undefined,this.L('Tipo')), trigger: 'blur' }
            ],
            locationId      :[
                {type: "number", required: true,message:this.L('FieldIsRequired',undefined,this.L('Lugar')),trigger: 'blur'}
            ],
            date          :[
                {required: true,message:this.L('FieldIsRequired',undefined,this.L('Fecha y hora')),trigger: 'blur'}
            ],
            managerId       :[
                {type: "number", required: true,message:this.L('FieldIsRequired',undefined,this.L('Responsable')),trigger: 'blur'}
            ],
            commodity     :[
                {required: true,message:this.L('FieldIsRequired',undefined,this.L('Commodity')),trigger: 'blur'}
            ],
            package       :[
                {required: true,message:this.L('FieldIsRequired',undefined,this.L('Package')),trigger: 'blur'}
            ] 
        }
    }
</script>
