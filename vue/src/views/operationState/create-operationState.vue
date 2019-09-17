<template>
    <div>
        <Modal :title="L('CreateNewOperationState')"
               :value="value"
               @on-ok="save"
               @on-visible-change="visibleChange">
            <Form ref="operationStateForm" label-position="top" :rules="operationStateRule" :model="operationState">
                <Tabs value="detail">
                    <TabPane :label="L('Details')" name="detail">
                        <FormItem :label="L('Name')" prop="name">
                            <Input v-model="operationState.name" :maxlength="32"></Input>
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
    import OperationState from '@/store/entities/operationState'
    @Component
    export default class CreateOperationState extends AbpBase{
        @Prop({type:Boolean,default:false}) value:boolean;
        operationState:OperationState=new OperationState();

        save() {
            (this.$refs.operationStateForm as any).validate(async (valid:boolean)=>{
                if (valid) {

                    await this.$store.dispatch({
                        type:'operationState/create',
                        data:this.operationState
                    });
                    (this.$refs.operationStateForm as any).resetFields();
                    this.$emit('save-success');
                    this.$emit('input',false);
                }
            })
        }
        cancel(){
            (this.$refs.operationStateForm as any).resetFields();
            this.$emit('input',false);
        }
        visibleChange(value:boolean){
            if(!value){
                this.$emit('input',value);
            }
        }

        operationStateRule={
            name:[{required: true,message:this.L('FieldIsRequired',undefined,this.L('Name')),trigger: 'blur'}]
        }
    }
</script>
