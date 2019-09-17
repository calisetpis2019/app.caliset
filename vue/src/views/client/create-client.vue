<template>
    <div>
        <Modal :title="L('CreateNewClient')"
               :value="value"
               @on-ok="save"
               @on-visible-change="visibleChange">
            <Form ref="clientForm" label-position="top" :rules="clientRule" :model="client">
                <Tabs value="detail">
                    <TabPane :label="L('Details')" name="detail">
                        <FormItem :label="L('Name')" prop="name">
                            <Input v-model="client.name" :maxlength="32"></Input>
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
    import Client from '@/store/entities/client'
    @Component
    export default class CreateClient extends AbpBase{
        @Prop({type:Boolean,default:false}) value:boolean;
        client:Client=new Client();

        save() {
            (this.$refs.clientForm as any).validate(async (valid:boolean)=>{
                if (valid) {

                    await this.$store.dispatch({
                        type:'client/create',
                        data:this.client
                    });
                    (this.$refs.clientForm as any).resetFields();
                    this.$emit('save-success');
                    this.$emit('input',false);
                }
            })
        }
        cancel(){
            (this.$refs.clientForm as any).resetFields();
            this.$emit('input',false);
        }
        visibleChange(value:boolean){
            if(!value){
                this.$emit('input',value);
            }
        }

        clientRule={
            name:[{required: true,message:this.L('FieldIsRequired',undefined,this.L('Name')),trigger: 'blur'}]
        }
    }
</script>
