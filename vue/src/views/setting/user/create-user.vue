<template>
    <div>
        <Modal
         :title="L('CreateNewUser')"
         :value="value"
         @on-ok="save"
         @on-visible-change="visibleChange"
        >
            <Form ref="userForm"  label-position="top" :rules="userRule" :model="user">
                <Tabs value="detail">
                    <TabPane :label="L('Details')" name="detail">
                      
                        <FormItem :label="L('EmailAddress')" prop="emailAddress" >
                            <Input v-model="user.emailAddress" :maxlength="32" :minlength="2"></Input>
                        </FormItem>
                        <FormItem :label="L('Name')" prop="name">
                            <Input v-model="user.name" :maxlength="32"></Input>
                        </FormItem>
                        <FormItem :label="L('Surname')" prop="surname">
                            <Input v-model="user.surname" :maxlength="1024"></Input>
                        </FormItem>

                        <FormItem :label="L('Password')" prop="password">
                            <Input v-model="user.password" type="password" :maxlength="32"></Input>
                        </FormItem>
                        <FormItem :label="L('ConfirmPassword')" prop="confirmPassword">
                            <Input v-model="user.confirmPassword" type="password" :maxlength="32"></Input>
                        </FormItem>

                        <FormItem :label="L('Documento')" prop="document">
                            <!--<Input v-model="user.document" type="number"></Input>-->
                            <input type="number" class="ivu-input" v-model="user.document"/>
                        </FormItem>
                        <FormItem :label="L('Número de contacto')" prop="phone">
                            <Input v-model="user.phone" :maxlength="32"></Input>
                        </FormItem>
                        <FormItem :label="L('Fecha de nacimiento')" prop="birthDate">
                            <!--<Input v-model="user.birthDate" type="date"></Input>-->
                            <DatePicker format="dd/MM/yyyy" type="date" :start-date="set_start_date()" v-model="user.birthDate" style="width:100%"></DatePicker>
                        </FormItem>

                        <FormItem :label="L('Ciudad')" prop="city">
                            <Input v-model="user.city"></Input>
                        </FormItem>

                        <FormItem :label="L('Dirección')" prop="adress">
                            <Input v-model="user.adress"></Input>
                        </FormItem>

                        <FormItem label="Especialidad">
                            <Input v-model="user.specialty"></Input>
                        </FormItem>

                        <FormItem label="Rol" prop="roleNames" >
                            <CheckboxGroup v-model="user.roleNames">
                                <Checkbox :label="role.normalizedName" v-for="role in roles" :key="role.id"><span>{{role.name}}</span></Checkbox>
                            </CheckboxGroup>
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
    @Component
    export default class CreateUser extends AbpBase{
        @Prop({type:Boolean,default:false}) value:boolean;
        user:User=new User();
        get roles(){
            return this.$store.state.user.roles;
        }
        save() {
            this.user.userName = this.user.emailAddress;
            (this.$refs.userForm as any).validate(async (valid:boolean)=>{
                if(valid){
                    //this.user.isActive=true;
                    await this.$store.dispatch({
                        type:'user/create',
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
        }
        validatePassCheck = (rule:any, value:any, callback:any) => {
            if (!value) {
                callback(new Error(this.L('Campo obligatorio')));
            } else if (value !== this.user.password) {
                callback(new Error(this.L('La confirmación no coincide con la contraseña')));
            } else {
                callback();
            }
        }
        set_start_date(){
            var year=new Date().getFullYear()-20;
            return new Date(year, 1, 1);
        };
        userRule={
            //userName:[{required: true,message:this.L('FieldIsRequired',undefined,this.L('UserName')),trigger: 'blur'}],
            name:[
                {required:true,message:this.L('FieldIsRequired',undefined,this.L('Name')),trigger: 'blur'}
            ],
            surname:[
                {required:true,message:this.L('FieldIsRequired',undefined,this.L('Surname')),trigger: 'blur'}
            ],
            document:[
                {required:true,message:this.L('FieldIsRequired',undefined,this.L('Surname')),trigger: 'blur'}
            ],
            emailAddress:[
                {required:true,message:this.L('FieldIsRequired',undefined,this.L('Email')),trigger: 'blur'},{type: 'email',message: 'El formato del email es incorrecto'}
            ],
            phone:[
                {required:true,message:this.L('FieldIsRequired',undefined,this.L('Surname')),trigger: 'blur'}
            ],
            password:[
                {required:true,message:this.L('FieldIsRequired',undefined,this.L('Password')),trigger: 'blur'}
            ],
            confirmPassword:[
                {required:true,validator:this.validatePassCheck,trigger: 'blur'}
            ],
            roleNames:[
                {type: "array", required: true, min: 1, message: 'Seleccionar mínimo uno', trigger: 'change' }
            ]
        }
    }
</script>

