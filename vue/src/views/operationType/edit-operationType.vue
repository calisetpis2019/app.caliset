<template>
    <div>
        <Modal :title="L('EditOperationType')"
               :value="value"
               @on-ok="save"
               @on-visible-change="visibleChange">
            <Form ref="operationTypeForm" label-position="top" :rules="operationTypeRule" :model="operationType">
                <Tabs value="detail">
                    <TabPane :label="L('Details')" name="detail">
                        <FormItem :label="L('Name')" prop="name">
                            <Input v-model="operationType.name" :maxlength="32"></Input>
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
    import OperationType from '@/store/entities/operationType'
    @Component
    export default class EditOperationType extends AbpBase{
        @Prop({type:Boolean,default:false}) value:boolean;
        operationType:OperationType=new OperationType();
        created(){

        }
        save() {
            (this.$refs.operationTypeForm as any).validate(async (valid:boolean)=>{
                if (valid) {
                    await this.$store.dispatch({
                        type:'operationType/update',
                        data:this.operationType
                    });
                    (this.$refs.operationTypeForm as any).resetFields();
                    this.$emit('save-success');
                    this.$emit('input',false);
                }
            })
        }
        cancel(){
            (this.$refs.operationTypeForm as any).resetFields();
            this.$emit('input',false);
        }
        visibleChange(value:boolean){
            if(!value){
                this.$emit('input',value);
            } else {
                this.operationType = Util.extend(true, {}, this.$store.state.operationType.editOperationType);
            }
        }

        operationTypeRule={
            name:[{required: true,message:this.L('FieldIsRequired',undefined,this.L('Name')),trigger: 'blur'}]
        }
    }
</script>
