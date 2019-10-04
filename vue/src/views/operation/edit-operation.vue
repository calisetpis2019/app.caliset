<template>
    <div>
        <Modal :title="L('EditOperation')"
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
                        <FormItem :label="L('Location')" prop="location">
                            <Input v-model="operation.idLocation"></Input>
                        </FormItem>
                        <FormItem :label="L('Type')" prop="type">
                            <Input v-model="operation.operationType"></Input>
                        </FormItem>
                        <FormItem :label="L('Nominator')" prop="nominator">
                            <Input v-model="operation.nominador"></Input>
                        </FormItem>
                        <FormItem :label="L('Loader')" prop="loader">
                            <Input v-model="operation.cargador"></Input>
                        </FormItem>
                        <FormItem :label="L('State')" prop="state">
                            <Input v-model="operation.operationState"></Input>
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
    @Component
    export default class EditOperation extends AbpBase{
        @Prop({type:Boolean,default:false}) value:boolean;
        operation:Operation=new Operation();
        created(){

        }
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
                this.operation = Util.extend(true, {}, this.$store.state.operation.editoperation);
            }
        }

        operationRule={
            date:[{required: true,message:this.L('FieldIsRequired',undefined,this.L('Name')),trigger: 'blur'}]
        }
    }
</script>
