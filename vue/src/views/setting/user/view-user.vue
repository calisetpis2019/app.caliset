<template>
    <div>
        <Modal
         title="Ver Usuario"
         :value="value"
         @on-visible-change="visibleChange"        >
            <Form ref="userForm"  label-position="top" :model="user">
                <Tabs value="detail">
                    <TabPane :label="L('Details')" name="detail">
                        <FormItem :label="L('EmailAddress')" prop="emailAddress">
                            <input :readonly="true" v-model="user.emailAddress" type="email" :maxlength="32" style="width:100%"></input>
                        </FormItem>
                        <FormItem :label="L('Name')" prop="name">
                            <input :readonly="true" v-model="user.name" style="width:100%"></input>
                        </FormItem>
                        <FormItem :label="L('Surname')" prop="surname">
                            <input :readonly="true" v-model="user.surname" style="width:100%"></input>
                        </FormItem>
                        <FormItem :label="L('Document')" prop="document">
                            <input :readonly="true" v-model="user.document" style="width:100%"></input>
                        </FormItem>
                        <FormItem :label="L('Phone')" prop="phone">
                            <input :readonly="true" v-model="user.phone" style="width:100%"></input>
                        </FormItem>
                        <FormItem :label="L('BirthDate')" prop="birthDate">
                            <input :readonly="true" v-model="user.birthDate" type="datetime" style="width:100%"></input>
                        </FormItem>
                        <FormItem :label="L('City')" prop="city">
                            <input :readonly="true" v-model="user.city" style="width:100%"></input>
                        </FormItem>
                        <FormItem :label="L('Adress')" prop="adress">
                            <input :readonly="true" v-model="user.adress" style="width:100%"></input>
                        </FormItem>
                        <FormItem :label="L('Speciality')" prop="speciality">
                            <input :readonly="true" v-model="user.specialty" style="width:100%"></input>
                        </FormItem>
                        <FormItem>
                            <h2 v-if="user.isActive" style="color:green;">{{L('IsActive')}}</h2>
                            <h2 v-else style="color:red;" >Inactivo </h2>
                        </FormItem>
                    </TabPane>
                    <TabPane :label="L('Roles')" name="roles" v-model="user.roleNames">
                        <ul>
                            <li v-for="role in roles" style="font-size: 20px;margin-left: 30px">
                                {{role.name}}
                            </li>
                        </ul>
                    </TabPane>
                    <TabPane :label="L('Asignaciones')" name="asignations">
                        <!-- Aca van las asignaciones del usuario sobre operaciones -->
                        <!--<ul>
                            <li v-for="role in roles" style="font-size: 20px;margin-left: 30px">
                                {{role.name}}
                            </li>
                        </ul>
                        -->
                    </TabPane>
                    <TabPane :label="L('Adjuntos')" name="attachments">
                        <!-- Aca van documentos adjuntos del usuario -->
                    </TabPane>
<!--
                    <TabPane :label="L('Roles')" name="roles">
                        <CheckboxGroup v-model="user.roleNames">
                            <Checkbox :label="role.normalizedName" v-for="role in roles" :key="role.id"><span>{{role.name}}</span></Checkbox>
                        </CheckboxGroup>
                    </TabPane>
-->
                </Tabs>
            </Form>
            <div slot="footer">
                <Button @click="cancel">{{L('Cancel')}}</Button>
            </div>
        </Modal>
    </div>
</template>
<script lang="ts">
    import { Component, Vue,Inject, Prop,Watch } from 'vue-property-decorator';
    import Util from '../../../lib/util'
    import AbpBase from '../../../lib/abpbase'
    import User from '../../../store/entities/user'
    import UserRequest from '@/store/entities/user-request'

    class PageViewUserRequest extends UserRequest {
    }

    @Component
    export default class ViewUser extends AbpBase{


        @Prop({type:Boolean,default:false}) value:boolean;
        user:User=new User();

        pagerequest: PageViewUserRequest = new PageViewUserRequest();


        created(){
            
        }
        get roles(){
            return this.$store.state.user.roles;
        }

        get assingnations(){
            console.log(this.$store.state.assignation.assignmentsByUsers);
            return this.$store.state.assignation.assignmentsByUsers;

        }

        cancel(){
            (this.$refs.userForm as any).resetFields();
            this.$emit('input',false);
        }
        visibleChange(value:boolean){
            if(!value){
                this.$emit('input',value);
            }else{
                this.user=Util.extend(true,{},this.$store.state.user.viewUser);
            }
        }

        async getAssignations() {
            await this.$store.dispatch({
                type: 'assignation/getAssignationsByUser',
                data: this.pagerequest
            })

        }
    }
</script>