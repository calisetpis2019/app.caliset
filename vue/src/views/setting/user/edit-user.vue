<template>
    <div>
        <Modal
         :title="L('EditUser')"
         :value="value"
         @on-ok="save"
         @on-visible-change="visibleChange">
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

                        <FormItem label="Especialidad">
                            <Input v-model="user.specialty"></Input>
                        </FormItem>

                        <FormItem :label="L('Usuario de Google')" prop="cuentaGP" >
                            <Input v-model="user.cuentaGP" :maxlength="32" :minlength="2"></Input>
                        </FormItem>
                        
                        <FormItem :label="L('Contraseña de Google')" prop="passwordGP">
                            <Input v-model="user.passwordGP" type="password" ></Input>
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
                    <TabPane label="Adjuntos">
                        <Row type="flex" justify="center" class="code-row-bg">
                            <Button @click="uploadClick" icon="ios-cloud-upload-outline" type="primary" size="large">Subir Imagen</Button>
                        </Row>
                        <Divider />
                        <Row type="flex" justify="center" class="code-row-bg"><h3>Imagenes subidas</h3></Row>
                        <Table  :loading="loadingAttachment" 
                                :columns="fileColumns"
                                no-data-text="No se han subido archivos" 
                                border 
                                :data="fileList">
                        </Table>
                    </TabPane>
                    <TabPane label="Resetear Contraseña">
                        <FormItem label="Nueva Contraseña">
                            <Input v-model="newPassword" type="password" :maxlength="32"></Input>
                        </FormItem>

                        <Divider/>

                        <Row type="flex" justify="center" class="code-row-bg">
                            <Button :loading="loadingV" @click="resetPassword" type="primary" size="large">
                                <span v-if="!loadingV">Enviar</span>
                                <span v-else>Cambiando...</span>
                            </Button>
                        </Row>
                    </TabPane>
                </Tabs>
            </Form>
            <div slot="footer">
                <Button @click="cancel">{{L('Cancel')}}</Button>
                <Button @click="save" type="primary">{{L('OK')}}</Button>
            </div>
            <input style="visibility:hidden" id="uploadFileElement" type="file" @change="onFileChanged">
        </Modal>
    </div>
</template>
<script lang="ts">
    import { Component, Vue,Inject, Prop,Watch } from 'vue-property-decorator';
    import Util from '../../../lib/util'
    import AbpBase from '../../../lib/abpbase'
    import User from '../../../store/entities/user'
    import update from 'immutability-helper'
    import FileRecord from '../../../store/entities/user-file'

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
        
        //Password Reset
        loading:boolean=false;
        get loadingV(){
            return this.loading;
        }
        newPassword:string="";
        async resetPassword(){
            if(this.newPassword!=""){
                this.loading=true;
                let pagerequest = { 
                    userId: this.user.id,
                    newPassword: this.newPassword,
                }
                await this.$store.dispatch({
                    type:'user/resetPassword',
                    data: pagerequest
                });
                this.$Message.success({content:'Contraseña cambiada exitosamente.',duration:4});
                this.newPassword="";
            }
            else{
                this.$Message.warning({content:'Debe escribir una contraseña.',duration:4});
            }
            this.loading=false;
        }
        //END Password Reset

        //User File Code
        selectedFileName:string;
        uploadButton:any;
        get loadingAttachment(){
            return this.$store.state.userfile.loading;
        }
        get fileList(){
            return this.$store.state.userfile.list;
        }
        onFileChanged(e){
            var thisthis = this;
            if (e.target.files && e.target.files[0]) {
                this.selectedFileName=e.target.files[0].name;
                this.readFile(e.target.files[0], async function(e) {
                    let encoded = e.target.result.toString().replace(/^data:(.*,)?/, '');
                    if ((encoded.length % 4) > 0) {
                        encoded += '='.repeat(4 - (encoded.length % 4));
                    }
                    let pagerequest = { 
                        userId: thisthis.user.id,
                        name: thisthis.selectedFileName,
                        photo: encoded
                    }
                    await thisthis.$store.dispatch({
                        type:'user/uploadPicture',
                        data: pagerequest
                    });
                    thisthis.reloadImages();
                    thisthis.$Message.success({content:'Archivo subido exitosamente.',duration:4});
                });
            }
        }
        readFile(file, onLoadCallback){
            var reader = new FileReader();
            reader.onload = onLoadCallback;
            reader.readAsDataURL(file);
        }
        uploadClick(){
            this.uploadButton.click();
        }
        reloadImages(){
            this.$store.dispatch({
                type: 'userfile/getFileList',
                data: this.user.id
            });
        }
        async deleteFile(idFile){
            await this.$store.dispatch({
                type: 'userfile/delete',
                data: idFile
            });
            this.reloadImages();
        }
        //END User File Code


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
                (this.$refs.userForm as any).resetFields();
                this.newPassword="";
                this.$emit('input',value);
            }
            else{
                var birthDate=this.parse_date(this.$store.state.user.editUser.birthDate);
                var rol=this.normalize_role(this.$store.state.user.editUser.roleNames[0]);
                var modUser=update(this.$store.state.user.editUser,{roleNames:{$set:rol}});
                var modUser=update(modUser,{birthDate:{$set:birthDate}});
                this.user=Util.extend(true,{},modUser);

                this.$store.dispatch({
                    type: 'userfile/getFileList',
                    data: this.user.id
                });
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

        fileColumns=[
            {
                title:'Id',
                key: 'id',
                width:70,
            },
            {
                title:'Nombre',
                key: 'name'
            },
            {
                title:this.L('Actions'),
                width:100,
                render:(h:any,params:any)=>{
                    var toRender = [
                        h('Button',{
                            props:{
                                type:'error',
                                size:'small'
                            },
                            on:{
                                click:()=>{
                                    this.deleteFile(params.row.id);
                                }
                            }
                        },this.L('Eliminar'))
                    ];
                    return toRender;
                }
            }
        ]

        validate_document = (rule:any, value:any, callback:any) => { 
            var numString=value.toString();
            if(numString.length<8){
                callback(new Error(this.L('Documento debe tener al menos 8 digitos')));
            }
            else{
                callback();
            }
        }

        userRule={
            name:[
                {required:true,message:this.L('FieldIsRequired',undefined,this.L('Name')),trigger: 'blur'}
            ],
            surname:[
                {required:true,message:this.L('FieldIsRequired',undefined,this.L('Surname')),trigger: 'blur'}
            ],
            document:[
                {required:true,validator:this.validate_document,trigger: 'blur'}
            ],
            emailAddress:[
                {required:true,message:this.L('FieldIsRequired',undefined,this.L('Email')),trigger: 'blur'},
                {type: 'email',message: 'El formato del email es incorrecto'}
            ],
            phone:[
                {required:true,message:this.L('FieldIsRequired',undefined,this.L('Surname')),trigger: 'blur'}
            ],
            roleNames:[
                {required: true, message: 'Seleccionar uno', trigger: 'change'}
            ]
        }
        async mounted(){
            this.uploadButton=document.getElementById('uploadFileElement');
        }
    }
</script>

