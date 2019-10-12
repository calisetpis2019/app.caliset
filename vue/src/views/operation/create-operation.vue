<template>
    <div>
        <Modal :title="L('Crear Operación')"
               :value="value"
               @on-ok="save"
               @on-visible-change="visibleChange">
            
            <Form ref="operationForm" label-position="top" :rules="operationRule" :model="operation">
                <Tabs value="detail">
                    <TabPane label="Details" name="detail">

                        

                        <FormItem label="Tipo" prop="operationTypeId">
                            <v-select v-model="operation.operationTypeId" label="name" :reduce="name => name.id" :options="listOfOperationTypes">
                                <span slot="no-options">No existen opciones para la busqueda ingresada.</span>
                            </v-select>
                        </FormItem> 


                        <FormItem label="Nominador" prop="nominatorId">
                            <v-select v-model="operation.nominatorId" label="name" :reduce="name => name.id" :options="listOfClients">
                                <span slot="no-options">No existen opciones para la busqueda ingresada.</span>
                            </v-select>
                        </FormItem>

                        <FormItem label="Cargador" prop="chargerId">
                            <v-select v-model="operation.chargerId" label="name" :reduce="name => name.id" :options="listOfClients">
                                <span slot="no-options">No existen opciones para la busqueda ingresada.</span>
                            </v-select>
                        </FormItem>


                        <FormItem label="Lugar" prop="locationId">
                            <v-select v-model="operation.locationId" label="name" :reduce="name => name.id" :options="listOfLocations">
                                <span slot="no-options">No existen opciones para la busqueda ingresada.</span>
                            </v-select>
                        </FormItem>

                        <FormItem label="Fecha y hora de inicio" prop="date">
                            <VueCtkDateTimePicker label="Seleccionar" hint=" " v-model="operation.date" locale="es" v-bind:right="true" />
                        </FormItem>

                        <FormItem label="Responsable" prop="managerId">
                            <v-select v-model="operation.managerId" label="name" :reduce="name => name.id" :options="listOfUsers">
                                <span slot="no-options">No existen opciones para la busqueda ingresada.</span>
                            </v-select>
                        </FormItem>

                        
                        <FormItem label="Mercaderia" prop="commodity">
                            <Input v-model="operation.commodity" :maxlength="32"></Input>
                        </FormItem>

                        <FormItem label="Empaque" prop="package">
                            <Input v-model="operation.package" :maxlength="32"></Input>
                        </FormItem>

                        <FormItem label="Nombre del Barco" prop="ship">
                            <Input v-model="operation.shipName" :maxlength="32"></Input>
                        </FormItem>

                        
                        <FormItem label="Destino" prop="destination">
                            <Input v-model="operation.destiny" :maxlength="32"></Input>
                        </FormItem>

                        
                        <FormItem label="Referencia cliente" prop="clientReference">
                            <Input v-model="operation.clientReference" :maxlength="32"></Input>
                        </FormItem>

                        
                        <FormItem label="Linea" prop="line">
                            <Input v-model="operation.line" :maxlength="32"></Input>
                        </FormItem>

                        
                        <FormItem label="Booking" prop="bookingNumber">
                            <Input v-model="operation.bookingNumber" :maxlength="32"></Input>
                        </FormItem>
                        
                        
                        <FormItem label="Notas" prop="notes">
                            <Input v-model="operation.notes" :maxlength="32"></Input>
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
    import PageRequest from '@/store/entities/page-request'
    import Operation from '@/store/entities/operation'
    import OperationState from '@/store/entities/operationState'
    import Location from '@/store/entities/location'

    class PageCreateOperationRequest extends PageRequest {
    }

    @Component
    export default class CreateOperation extends AbpBase{
        @Prop({type:Boolean,default:false}) value:boolean;
        operation:Operation=new Operation();

        pagerequest: PageCreateOperationRequest = new PageCreateOperationRequest();


        get listOfLocations() {
            return this.$store.state.location.list;
        };

        get listOfOperationTypes() {
            return this.$store.state.operationType.list;
        };

        get listOfClients() {
            return this.$store.state.client.list;
        };

        get listOfUsers() {
            return this.$store.state.user.list;
        };


        save() {
            (this.$refs.operationForm as any).validate(async (valid:boolean)=>{
                if (valid) {

                    await this.$store.dispatch({
                        type:'operation/create',
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
            }
            this.getOperationTypes()
            this.getClients()
            this.getLocations()
            this.getUsers()
        }

        async getOperationTypes() {

            await this.$store.dispatch({
                type: 'operationType/getAll',
                data: this.pagerequest
            })

        }

        async getClients() {

            await this.$store.dispatch({
                type: 'client/getAll',
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
                type: 'user/getAll',
                data: this.pagerequest
            })

        }

        operationRule={
            operationTypeId :[
                { type: "number", required: true,message:this.L('FieldIsRequired',undefined,this.L('Tipo')), trigger: 'blur' }
            ],
            nominatorId     :[
                {type: "number", required: true,message:this.L('FieldIsRequired',undefined,this.L('Nominador')),trigger: 'blur'}
            ],
            chargerId        :[
                {type: "number", required: true,message:this.L('FieldIsRequired',undefined,this.L('Cargador')),trigger: 'blur'}
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
            ] 
        }

        test(e){
            console.log(e);
            console.log(event);
            // console.log(n);
            // if (t == false ){
            //     if (this.$refs['operationTypeSel']['value'] != ""){
            //         console.log('valido');
                    
            //     }else {
            //         console.log('invalido');
            //         (this.$refs['operationTypeSel'] as any).reset();
            //     }
            // }
        }

    }
</script>
