<template>
    <div>
        <Modal :title="L('CreateNewOperation')"
               :value="value"
               @on-ok="save"
               @on-visible-change="visibleChange">
            <Form ref="operationForm" label-position="top" :rules="operationRule" :model="operation">
                <Tabs value="detail">
                    <TabPane :label="L('Details')" name="detail">
                        <FormItem :label="L('Date')" prop="date">
                            <Input v-model="operation.date" type="date"></Input>
                        </FormItem>
                       <FormItem :label="L('Commodity')" prop="commodity">
                            <Input v-model="operation.commodity" :maxlength="32"></Input>
                        </FormItem>
                        <FormItem :label="L('Package')" prop="package">
                            <Input v-model="operation.package" :maxlength="32"></Input>
                        </FormItem>
                        <FormItem :label="L('Ship')" prop="ship">
                            <Input v-model="operation.shipName" :maxlength="32"></Input>
                        </FormItem>
                        <FormItem :label="L('Destination')" prop="destination">
                            <Input v-model="operation.destiny" :maxlength="32"></Input>
                        </FormItem>
                        <FormItem :label="L('Line')" prop="line">
                            <Input v-model="operation.line" :maxlength="32"></Input>
                        </FormItem>
                        <FormItem :label="L('Booking')" prop="bookingNumber">
                            <Input v-model="operation.bookingNumber" :maxlength="32"></Input>
                        </FormItem>
                        <span>{{L('Location')}}</span>
                        <Select :label="L('Location')" v-model="operation.idLocation" prop="location" style="padding: 10px 0px 20px 0px;">
                            <Option v-for="item in listOfLocations" :value="item.id" :key="item.id">{{ item.id }}</Option>
                        </Select>
                        <span>{{L('Type')}}</span>
                        <Select :label="L('Type')" v-model="operation.operationType" prop="operationType" style="padding: 10px 0px 20px 0px;">
                            <Option v-for="item in listOfOperationTypes" :value="item.id" :key="item.id">{{ item.name }}</Option>
                        </Select>
                        <span>{{L('State')}}</span>
                        <Select :label="L('State')" v-model="operation.operationState" prop="operationState" style="padding: 10px 0px 20px 0px;">
                            <Option v-for="item in listOfOperationStates" :value="item.id" :key="item.id">{{ item.name }}</Option>
                        </Select>
                        <FormItem :label="L('Nominator')" prop="nominator">
                            <Input v-model="operation.nominador" type="number"></Input>
                        </FormItem>
                        <FormItem :label="L('Loader')" prop="loader">
                            <Input v-model="operation.cargador" type="number"></Input>
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
    import Operation from '@/store/entities/operation'
    import OperationState from '@/store/entities/operationState'
    import Location from '@/store/entities/location'

    @Component
    export default class CreateOperation extends AbpBase{
        @Prop({type:Boolean,default:false}) value:boolean;
        operation:Operation=new Operation();

        get listOfLocations() {
            return this.$store.state.location.list;
        };

        get listOfOperationStates() {
            console.log(this.$store.state.operationState.list);
            return this.$store.state.operationState.list;
        };

        get listOfOperationTypes() {
            console.log(this.$store.state.operationType.list);
            return this.$store.state.operationType.list;
        };

        save() {
            (this.$refs.operationForm as any).validate(async (valid:boolean)=>{
                if (valid) {

                    await this.$store.dispatch({
                        type:'operation/create',
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
            }
        }


        operationRule={
            date :[{required: true,message:this.L('FieldIsRequired',undefined,this.L('Date')),trigger: 'blur'}],
            // nominator :[{required: true,message:this.L('FieldIsRequired',undefined,this.L('Nominator')),trigger: 'blur'}],
            // loader :[{required: true,message:this.L('FieldIsRequired',undefined,this.L('Loader')),trigger: 'blur'}],
            location :[{required: true,message:this.L('FieldIsRequired',undefined,this.L('Location')),trigger: 'blur'}],
            commodity :[{required: true,message:this.L('FieldIsRequired',undefined,this.L('Commodity')),trigger: 'blur'}],
            package :[{required: true,message:this.L('FieldIsRequired',undefined,this.L('Package')),trigger: 'blur'}] ,
            operationType :[{required: true,message:this.L('FieldIsRequired',undefined,this.L('Type')),trigger: 'blur'}]
        }
    }
</script>
