<template>
    <div>
        <Modal title="Editar Operación"
               :value="value"
               @on-ok="save"
               @on-visible-change="visibleChange">
            <Form ref="operationForm" label-position="top" :rules="operationRule" :model="operation">
                <Tabs>
                    <TabPane :label="L('Details')" name="detail">
                        <FormItem label="Tipo" prop="operationTypeId">
                            <v-select v-model="operation.operationTypeId" label="name" :reduce="name => name.id" :options="listOfOperationTypes">
                                <span slot="no-options">No existen opciones para la busqueda ingresada.</span>
                            </v-select>
                        </FormItem> 

                        <FormItem label="Lugar" prop="locationId">
                            <v-select v-model="operation.locationId" label="name" :reduce="name => name.id" :options="listOfLocations">
                                <span slot="no-options">No existen opciones para la busqueda ingresada.</span>
                            </v-select>
                        </FormItem>

                        <FormItem label="Fecha y hora de inicio" prop="date">
                            <VueCtkDateTimePicker id="operationStartingDateEdit" label="Seleccionar" hint=" " v-model="operation.date" locale="es" v-bind:right="true" />
                        </FormItem>

                        <FormItem label="Responsable" prop="managerId">
                            <v-select v-model="operation.managerId" label="fullName" :reduce="name => name.id" :options="listOfUsers">
                                <span slot="no-options">No existen opciones para la busqueda ingresada.</span>
                            </v-select>
                        </FormItem>

                        <FormItem label="Mercadería" prop="commodity">
                            <Input v-model="operation.commodity" :maxlength="32"></Input>
                        </FormItem>
                        
                        <FormItem label="Empaque" prop="package">
                            <Input v-model="operation.package" :maxlength="32"></Input>
                        </FormItem>

                        <FormItem label="Nominador" prop="nominatorId" v-if="operation.operationStateId != 3">
                            <v-select v-model="operation.nominatorId" label="name" :reduce="name => name.id" :options="listOfClients">
                                <span slot="no-options">No existen opciones para la busqueda ingresada.</span>
                            </v-select>
                        </FormItem>

                        <FormItem label="Cargador" prop="chargerId" v-if="operation.operationStateId != 3">
                            <v-select v-model="operation.chargerId" label="name" :reduce="name => name.id" :options="listOfClients">
                                <span slot="no-options">No existen opciones para la busqueda ingresada.</span>
                            </v-select>
                        </FormItem>

                        <FormItem label="Nombre del Barco" prop="ship" v-if="operation.operationStateId != 3">
                            <Input v-model="operation.shipName" :maxlength="32"></Input>
                        </FormItem>

                        <FormItem label="Destino" prop="destination" v-if="operation.operationStateId != 3">
                            <Input v-model="operation.destiny" :maxlength="32"></Input>
                        </FormItem>

                        <FormItem label="Referencia cliente" prop="clientReference" v-if="operation.operationStateId != 3">
                            <Input v-model="operation.clientReference" :maxlength="32"></Input>
                        </FormItem>

                        <FormItem label="Linea" prop="line" v-if="operation.operationStateId != 3">
                            <Input v-model="operation.line" :maxlength="32"></Input>
                        </FormItem>

                        <FormItem label="Número de booking" prop="bookingNumber" v-if="operation.operationStateId != 3">
                            <Input v-model="operation.bookingNumber" :maxlength="32"></Input>
                        </FormItem>

                        <FormItem label="Notas" prop="notes" v-if="operation.operationStateId != 3">
                            <Input v-model="operation.notes" :maxlength="32"></Input>
                        </FormItem>
                    </TabPane>
                    <TabPane label="Formularios">
                        <Row type="flex" justify="center" class="code-row-bg"><h3>Formularios Asociados</h3></Row>
                        <Table  :loading="loadingAssignedForms" 
                                :columns="columnsAssignedForms"
                                no-data-text="No se han subido archivos" 
                                border 
                                :data="listAssignedForms">
                        </Table>
                        <Divider/>
                        <Row type="flex" justify="center" class="code-row-bg"><h3>Formularios Disponibles</h3></Row>
                        <Table  :loading="loadingAvailableForms" 
                                :columns="columnsAvailableForms"
                                no-data-text="No se han subido archivos" 
                                border 
                                :data="listAvailableForms">
                        </Table>
                    </TabPane>
                </Tabs>
            </Form>
            <div slot="footer">
                <Button @click="cancel">{{L('Cancel')}}</Button>
                <Button v-if="operation.operationStateId != 3" @click="save" type="primary">{{L('OK')}}</Button>
                <Button v-if="operation.operationStateId == 3" @click="save_finished" type="primary">{{L('OK')}}</Button>
            </div>
        </Modal>
    </div>
</template>

<script lang="ts">
    import { Component, Vue,Inject, Prop,Watch } from 'vue-property-decorator';
    import Util from '@/lib/util'
    import AbpBase from '@/lib/abpbase'
    import PageRequest from '@/store/entities/page-request'
    import Operation from '@/store/entities/operation'
    import FormularyEntity from '@/store/entities/formulary'

    class PageEditOperationRequest extends PageRequest { }

    @Component
    export default class EditOperation extends AbpBase{
        @Prop({type:Boolean,default:false}) value:boolean;
        operation:Operation=new Operation();
        created(){ }

        pagerequest: PageEditOperationRequest = new PageEditOperationRequest();

        async downloadFile(){
            await this.$store.dispatch({
                type: 'formulary/get',
                data: this.$store.state.formulary.formularyId
            })
            var uri = "data:application/octet-stream;charset=utf-16le;base64,"+this.$store.state.formulary.retrievedForm.photo;
            var link = document.createElement('a');
            link.setAttribute("download", this.$store.state.formulary.retrievedForm.name);
            link.setAttribute("href", uri);
            link.click();
        }

        get listOfUsers() {
            var result = [];
            var auxUser = this.$store.state.user.list;
            auxUser.forEach( (element) => {
                element["fullName"]=element["name"]+" "+element["surname"];
                result.push(element);
            });
            return result;
        };

        get listOfLocations() {
            return this.$store.state.location.list;
        };

        get listOfOperationTypes() {
            return this.$store.state.operationType.list;
        };

        get listOfClients() {
            return this.$store.state.client.list;
        };


        //Operation Formularies
        get listAssignedForms() {
            return this.$store.state.formulary.listAssignedForms;
        };
        get loadingAssignedForms() {
            return this.$store.state.formulary.loadingAssignedForms;
        }

        get listAvailableForms() {
            return this.$store.state.formulary.listAvailableForms;
        };
        get loadingAvailableForms() {
            return this.$store.state.formulary.loadingAvailableForms;
        }
        async assignFormulary(){
            let pagerequest = { 
                operationId: this.operation.id,
                formId: this.$store.state.formulary.formularyId
            }
            await this.$store.dispatch({
                type: 'formulary/associateForm',
                data: pagerequest
            })
            this.getAssignedForms();
            this.getAvailableForms();
        }
        async unassignFormulary(){
            let pagerequest = { 
                operationId: this.operation.id,
                formId: this.$store.state.formulary.formularyId
            }
            await this.$store.dispatch({
                type: 'formulary/unassociateForm',
                data: pagerequest
            })
            this.getAssignedForms();
            this.getAvailableForms();
        }
        //END Operation Formularies


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
        save_finished() {
            (this.$refs.operationForm as any).validate(async (valid:boolean)=>{
                if (valid) {
                    await this.$store.dispatch({
                        type:'operation/update_finished',
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
                this.operation = Util.extend(true, {}, this.$store.state.operation.editOperation);
            }
            this.getOperationTypes();
            this.getLocations();
            this.getUsers();
            this.getClients();
            this.getAssignedForms();
            this.getAvailableForms();
        }

        async getOperationTypes() {
            await this.$store.dispatch({
                type: 'operationType/getAll',
                data: this.pagerequest
            })
        }

        async getLocations() {
            await this.$store.dispatch({
                type: 'location/getAll',
                data: this.pagerequest
            })
        }

        async getUsers() {
            await this.$store.dispatch({
                type: 'user/getAllUsers',
                data: this.pagerequest
            })
        }

        async getClients() {
            await this.$store.dispatch({
                type: 'client/getAllUsersFilter',
                data: this.pagerequest
            })
        }

        async getAvailableForms() {
            await this.$store.dispatch({
                type: 'formulary/getAvailable',
                data: this.operation.id
            })
        }
        async getAssignedForms() {
            await this.$store.dispatch({
                type: 'formulary/getAssigned',
                data: this.operation.id
            })
        }

        columnsAssignedForms = [
            {
                title: this.L('Id'),
                key: 'id',
                width:70
            },
            {
                title: this.L('Name'),
                key: 'name'
            },
            {
                title:this.L('Actions'),
                key:'Actions',
                width:200,
                render:(h:any,params:any)=>{
                    return h('div',[
                        h('Button',{
                            props:{
                                type:'info',
                                size:'small'
                            },
                            style:{
                                marginRight:'5px'
                            },
                            on:{
                            click:()=>{
                                this.$store.commit('formulary/setFormIdAction',params.row.id);
                                this.downloadFile();
                            }
                        }
                        },this.L('Descargar')),
                        h('Button',{
                            props:{
                                type:'warning',
                                size:'small'
                            },
                            style:{
                                marginRight:'5px'
                            },
                            on:{
                            click:()=>{
                                this.$store.commit('formulary/setFormIdAction',params.row.id);
                                this.unassignFormulary();
                            }
                        }
                        },this.L('Desasociar'))
                    ])
                }
            }
        ]

        columnsAvailableForms = [
            {
                title: this.L('Id'),
                key: 'id',
                width:70
            },
            {
                title: this.L('Name'),
                key: 'name'
            },
            {
                title:this.L('Actions'),
                key:'Actions',
                width:200,
                render:(h:any,params:any)=>{
                    return h('div',[
                        h('Button',{
                            props:{
                                type:'info',
                                size:'small'
                            },
                            style:{
                                marginRight:'5px'
                            },
                            on:{
                            click:()=>{
                                this.$store.commit('formulary/setFormIdAction',params.row.id);
                                this.downloadFile();
                            }
                        }
                        },this.L('Descargar')),
                        h('Button',{
                            props:{
                                type:'warning',
                                size:'small'
                            },
                            style:{
                                marginRight:'5px'
                            },
                            on:{
                            click:()=>{
                                this.$store.commit('formulary/setFormIdAction',params.row.id);
                                this.assignFormulary();
                            }
                        }
                        },this.L('Asociar'))
                    ])
                }
            }
        ]

        operationRule={
            operationTypeId :[
                { type: "number", required: true,message:this.L('FieldIsRequired',undefined,this.L('Tipo')), trigger: 'blur' }
            ],
            locationId      :[
                {type: "number", required: true,message:this.L('FieldIsRequired',undefined,this.L('Lugar')),trigger: 'blur'}
            ],
            date          :[
                {required: true,message:this.L('FieldIsRequired',undefined,this.L('Fecha y hora')),trigger: 'blur'}
            ],
            managerId       :[
                {type: "number", required: true,message:this.L('FieldIsRequired',undefined,this.L('Responsable')),trigger: 'blur'}
            ],
            commodity     :[
                {required: true,message:this.L('FieldIsRequired',undefined,this.L('Commodity')),trigger: 'blur'}
            ],
            package       :[
                {required: true,message:this.L('FieldIsRequired',undefined,this.L('Package')),trigger: 'blur'}
            ],
            nominatorId     :[
                {type: "number", required: true,message:this.L('FieldIsRequired',undefined,this.L('Nominador')),trigger: 'blur'}
            ] 
        }
    }
</script>
