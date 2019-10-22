<template>
    <div>
        <Modal title="Cambio de Contraseña"
               :value="value"
               @on-ok="save"
               @on-visible-change="visibleChange">
            <Form ref="changePasswordForm" label-position="top" :rules="changePasswordRule" :model="instanceChangePassword">
                <FormItem label="Contraseña Actual" prop="currentPassword">
                    <Input type="password" v-model="instanceChangePassword.currentPassword" :maxlength="32"></Input>
                </FormItem>
                <FormItem label="Nueva Contraseña" prop="newPassword">
                    <Input type="password" v-model="instanceChangePassword.newPassword" :minlength="8"></Input>
                </FormItem>
                <FormItem label="Confirmación Nueva Contraseña" prop="confirmPassword">
                    <Input v-model="instanceChangePassword.confirmPassword" type="password" :minlength="8"></Input>
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
    import ChangePasswordEntity from '@/store/entities/change-password'
    @Component
    export default class ChangePassword extends AbpBase{
        @Prop({type:Boolean,default:false}) value:boolean;
        instanceChangePassword:ChangePasswordEntity=new ChangePasswordEntity();

        save() {
            (this.$refs.changePasswordForm as any).validate(async (valid:boolean)=>{
                if (valid) {
                    await this.$store.dispatch({
                        type:'changepasswordentity/update',
                        data:this.instanceChangePassword
                    });
                    (this.$refs.changePasswordForm as any).resetFields();
                    this.$emit('input',false);
                    this.$Message.success({content:'Contraseña cambiada exitosamente.',duration:3.5});
                }
            })
        }
        cancel(){
            (this.$refs.changePasswordForm as any).resetFields();
            this.$emit('input',false);
        }
        visibleChange(value:boolean){
            if(!value){
                (this.$refs.changePasswordForm as any).resetFields();
                this.$emit('input',value);
            }
        }
        validatePassCheck = (rule:any, value:any, callback:any) => {
            if (!value) {
                callback(new Error(this.L('Campo obligatorio')));
            }
            else if (value !== this.instanceChangePassword.newPassword) {
                callback(new Error(this.L('La confirmación no coincide con la contraseña')));
            }
            else if (value === this.instanceChangePassword.currentPassword){
                callback(new Error(this.L('La nueva contraseña coincide con la su anterior contraseña')));
            }
            else{
                callback();
            }
        }
        changePasswordRule={
            currentPassword:[{required: true,message:this.L('FieldIsRequired',undefined,this.L('Name')),trigger: 'blur'}],
            newPassword:[{required: true,message:this.L('FieldIsRequired',undefined,this.L('Name')),trigger: 'blur'}],
            confirmPassword:[
                {required:true,validator:this.validatePassCheck,trigger: 'blur'}
            ]
        }
    }
</script>