<template>
    <div>
        <Modal :title="L('Crear Operación')"
               :value="value"
               @on-ok="save"
               @on-visible-change="visibleChange">
            <Form ref="operationForm" label-position="top" :rules="operationRule" :model="operation">
                <Tabs value="detail">
                    <TabPane :label="L('Details')" name="detail">

                        <FormItem :label="L('Tipo')" prop="operationType">
                            <Select v-model="operation.operationTypeId" style="padding: 10px 0px 20px 0px;" filterable placeholder="">
                                <Option v-for="item in listOfOperationTypes" :value="item.id" :key="item.id">{{ item.name }}</Option>
                            </Select>
                        </FormItem>

                        <FormItem :label="L('Nominador')" prop="nominator">
                            <Select v-model="operation.nominatorId" style="padding: 10px 0px 20px 0px;" filterable placeholder="">
                                <Option v-for="item in listOfClients" :value="item.id" :key="item.id">{{ item.name }}</Option>
                            </Select>
                        </FormItem>

                        <FormItem :label="L('Cargador')" prop="loader">
                            <Select v-model="operation.chargerId" style="padding: 10px 0px 20px 0px;" filterable placeholder="">
                                <Option v-for="item in listOfClients" :value="item.id" :key="item.id">{{ item.name }}</Option>
                            </Select>
                        </FormItem>


                        <FormItem :label="L('Lugar')" prop="location">
                            <Select v-model="operation.locationId" style="padding: 10px 0px 20px 0px;" filterable placeholder="">
                                <Option v-for="item in listOfLocations" :value="item.id" :key="item.id">{{ item.name }}</Option>
                            </Select>
                        </FormItem>

                        <FormItem :label="L('Fecha y hora de inicio')" prop="date">
                            <VueCtkDateTimePicker v-model="operation.date" locale="es" right=true />
                        </FormItem>

                        <FormItem :label="L('Responsable')" prop="manager">
                            <Select v-model="operation.managerId" style="padding: 10px 0px 20px 0px;" filterable placeholder="">
                                <Option v-for="item in listOfUsers" :value="item.id" :key="item.id">{{ item.name }}</Option>
                            </Select>
                        </FormItem>

                        
                        <FormItem :label="L('Mercaderia')" prop="commodity">
                            <Input v-model="operation.commodity" :maxlength="32"></Input>
                        </FormItem>

                        <FormItem :label="L('Paquete')" prop="package">
                            <Input v-model="operation.package" :maxlength="32"></Input>
                        </FormItem>

                        <FormItem :label="L('Nombre del Barco')" prop="ship">
                            <Input v-model="operation.shipName" :maxlength="32"></Input>
                        </FormItem>

                        <span>Destino</span>
                        <FormItem :label="Destino" prop="destination">
                            <Input v-model="operation.destiny" :maxlength="32"></Input>
                        </FormItem>

                        <span>Referencia del Cliente</span>
                        <FormItem :label="Cliente" prop="clientReference">
                            <Input v-model="operation.clientReference" :maxlength="32"></Input>
                        </FormItem>

                        <span>Linea</span>
                        <FormItem :label="Linea" prop="line">
                            <Input v-model="operation.line" :maxlength="32"></Input>
                        </FormItem>

                        <span>Numero del Booking</span>
                        <FormItem :label="Booking" prop="bookingNumber">
                            <Input v-model="operation.bookingNumber" :maxlength="32"></Input>
                        </FormItem>
                        
                        <span>Notas</span>
                        <FormItem :label="Notas" prop="notes">
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
            console.log(this.$store.state.operationType.list);
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
            // operationType :[
            //     { required: true,message:this.L('FieldIsRequired',undefined,this.L('Tipo')), trigger: 'blur' }
            // ],
            // nominator     :[
            //     {required: true,message:this.L('FieldIsRequired',undefined,this.L('Nominador')),trigger: 'blur'}
            // ],
            // loader        :[
            //     {required: true,message:this.L('FieldIsRequired',undefined,this.L('Cargador')),trigger: 'blur'}
            // ],
            // location      :[
            //     {required: true,message:this.L('FieldIsRequired',undefined,this.L('Lugar')),trigger: 'blur'}
            // ],
            // date          :[
            //     {required: true,message:this.L('FieldIsRequired',undefined,this.L('Fecha y hora')),trigger: 'blur'}
            // ],
            // manager       :[
            //     {required: true,message:this.L('FieldIsRequired',undefined,this.L('Responsable')),trigger: 'blur'}
            // ],
            commodity     :[
                {required: true,message:this.L('FieldIsRequired',undefined,this.L('Commodity')),trigger: 'blur'}
            ],
            package       :[
                {required: true,message:this.L('FieldIsRequired',undefined,this.L('Package')),trigger: 'blur'}
            ] 
        }
    }
</script>
