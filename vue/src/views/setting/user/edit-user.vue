<template>
    <div>
        <Modal
         :title="L('EditUser')"
         :value="value"
         @on-ok="save"
         @on-visible-change="visibleChange"        >
            <Form ref="userForm"  label-position="top" :rules="userRule" :model="user">
                <Tabs value="detail">
                    <TabPane :label="L('Details')" name="detail">
                        <FormItem label="Correo electrónico" prop="emailAddress">
                            <Input v-model="user.emailAddress" type="email" :maxlength="32"></Input>
                        </FormItem>
                        <FormItem label="Nombre" prop="name">
                            <Input v-model="user.name" :maxlength="32"></Input>
                        </FormItem>
                        <FormItem label="Apellido" prop="surname">
                            <Input v-model="user.surname" :maxlength="1024"></Input>
                        </FormItem>
                        <FormItem label="Documento" prop="document">
                            <input type="number" class="ivu-input" v-model="user.document"/>
                        </FormItem>
                        <FormItem label="Número de contacto" prop="phone">
                            <Input v-model="user.phone" :maxlength="32"></Input>
                        </FormItem>
                        <FormItem label="Fecha de nacimiento" prop="birthDate">
                            <DatePicker format="dd/MM/yyyy" type="date" v-model="user.birthDate" style="width:100%"></DatePicker>
                        </FormItem>

                        <FormItem label="Ciudad" prop="city">
                            <Input v-model="user.city"></Input>
                        </FormItem>

                        <FormItem label="Dirección" prop="adress">
                            <Input v-model="user.adress"></Input>
                        </FormItem>
                        <FormItem label="Rol" prop="roleNames" >
                            <RadioGroup v-model="user.roleNames">
                                <Radio v-for="role in roles" :label="normalize_role(role.normalizedName)" :key="role.id"></Radio>
                            </RadioGroup>
                        </FormItem>
                        <FormItem>
                            <Checkbox v-if="differentUser()" v-model="user.isActive">{{L('IsActive')}}</Checkbox>
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
    import Util from '../../../lib/util'
    import AbpBase from '../../../lib/abpbase'
    import User from '../../../store/entities/user'
    import update from 'immutability-helper'
    @Component
    export default class EditUser extends AbpBase{
        @Prop({type:Boolean,default:false}) value:boolean;
        user:User=new User();
        created(){ }

        normalize_role(role){
            return role[0]+role.substring(1,role.length).toLowerCase();
        }
        get roles(){
            return this.$store.state.user.roles;
        }
        save() {
            this.user.userName = this.user.emailAddress;
            (this.$refs.userForm as any).validate(async (valid:boolean)=>{
                if(valid){
                    await this.$store.dispatch({
                        type:'user/update',
                        data:this.user
                    });
                    (this.$refs.userForm as any).resetFields();
                    this.$emit('save-success');
                    this.$emit('input',false);
                }
            })
        }
        cancel(){
            (this.$refs.userForm as any).resetFields();
            this.$emit('input',false);
        }
        visibleChange(value:boolean){
            if(!value){
                this.$emit('input',value);
            }
            else{
                this.$store.state.user.editUser.birthDate=this.parse_date(this.$store.state.user.editUser.birthDate);
                var rol=this.normalize_role(this.$store.state.user.editUser.roleNames[0]);
                this.user=Util.extend(true,{},update(this.$store.state.user.editUser,{roleNames:{$set:rol}}));
            }
        }
        parse_date(date:string){
            if(date!=null){
                var array_date = date.split("T");
                if(array_date.length==1){
                    return date;
                }
                array_date=array_date[0].split("-");
                return array_date[2]+"/"+array_date[1]+"/"+array_date[0];
            }
            return date;
        }
        differentUser(){
            if(this.user.id!=null){
                if(typeof this.$store.state.session.user.id !== "undefined" && typeof this.user.id !== "undefined"){
                    return this.$store.state.session.user.id !== this.user.id;
                }
                else{
                    return true;
                }
            }
        }
        userRule={
            userName:[{required: true,message:this.L('FieldIsRequired',undefined,this.L('UserName')),trigger: 'blur'}],
            name:[{required:true,message:this.L('FieldIsRequired',undefined,this.L('Name')),trigger: 'blur'}],
            surname:[{required:true,message:this.L('FieldIsRequired',undefined,this.L('Surname')),trigger: 'blur'}],
            emailAddress:[{required:true,message:this.L('FieldIsRequired',undefined,this.L('Email')),trigger: 'blur'},{type: 'email'}],
        }
    }
</script>

