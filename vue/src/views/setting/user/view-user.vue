<template>
    <div>
        <Modal
         title="Ver Usuario"
         :value="value"
         @on-visible-change="visibleChange" 
         width="90%"       >
            <Form ref="userForm"  label-position="top" :model="user">
                <h2> {{user.name}} {{user.surname}} </h2>
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
                        <FormItem label="Documento" prop="document">
                            <input :readonly="true" v-model="user.document" style="width:100%"></input>
                        </FormItem>
                        <FormItem label="Teléfono" prop="phone">
                               <input :readonly="true" v-model="user.phone" style="width:100%"></input>
                        </FormItem>
                        <FormItem label="Fecha Nacimiento" prop="birthDate">
                            <input :readonly="true" v-model="user.birthDate" type="datetime" style="width:100%"></input>
                        </FormItem>
                        <FormItem label="Ciudad" prop="city">
                            <input :readonly="true" v-model="user.city" style="width:100%"></input>
                        </FormItem>
                        <FormItem label="Dirección" prop="adress">
                            <input :readonly="true" v-model="user.adress" style="width:100%"></input>
                        </FormItem>
                        <FormItem label="Especialidad" prop="speciality">
                            <input :readonly="true" v-model="user.specialty" style="width:100%"></input>
                        </FormItem>
                        <FormItem label="Rol">
                            <input :readonly="true" :value="single_role()" style="width:100%"></input>
                        </FormItem>
                        <FormItem>
                            <h2 v-if="user.isActive" style="color:green;">{{L('IsActive')}}</h2>
                            <h2 v-else style="color:red;" >Inactivo </h2>
                        </FormItem>
                    </TabPane>
                    <TabPane :label="L('Asignaciones')" name="assignations">
                        <Row :gutter="20" type="flex" align="middle" style="margin-bottom: 30px; margin-left: 20px">
                            <Col span="3">
                                Filtrar por estado:  
                            </Col>
                            <Col span="4">
                                <Select placeholder="Estado" @on-change="stateFilter">
                                    <Option :value=this.active>Activa</Option>
                                    <Option :value=this.future>Futura</Option>
                                    <Option :value=this.finished>Finalizada</Option>
                                </Select>
                            </Col>
                        </Row>
                        <!-- Aca van las asignaciones del usuario sobre operaciones -->
                        <Table  :loading="loadingAssignation" 
                                :columns="assColumns"
                                no-data-text="No existen registros" 
                                border 
                                :data="assignations"></Table>
                    </TabPane>
                    <TabPane :label="L('Adjuntos')" name="attachments">
                        <Table  :loading="loadingAttachment" 
                                :columns="fileColumns"
                                no-data-text="No se han subido archivos" 
                                border 
                                :data="fileList"></Table>
                    </TabPane>
                    <TabPane label="Registro de horas" name="hours">
                        
                        <Table  :loading="loadingHoursRecord" 
                                :columns="hoursColumns"
                                no-data-text="No se han ingresado horas" 
                                border 
                                :data="hoursRecords"></Table>
                    </TabPane>

                </Tabs>
            </Form>
            <div slot="footer">
                <Button @click="cancel">{{L('Cancel')}}</Button>
            </div>
        </Modal>
        <view-operation-dummy v-model="viewOperationModalShow" ></view-operation-dummy>
    </div>
</template>
<script lang="ts">

    import { Component, Vue,Inject, Prop,Watch } from 'vue-property-decorator';
    import Util from '../../../lib/util'
    import AbpBase from '../../../lib/abpbase'
    import User from '../../../store/entities/user'
    import ViewOperationDummy from '../../operation/view-operation-dummy.vue'
    import HoursRecord from '../../../store/entities/hoursRecord'
    import LocationRecord from '../../../store/entities/locationRecord'
    import FileRecord from '../../../store/entities/user-file'
    import moment from 'moment'

    @Component({
        components:{ ViewOperationDummy }
    })
    export default class ViewUser extends AbpBase{
        @Prop({type:Boolean,default:false}) value:boolean;
        user:User=new User();
        hoursRecord:HoursRecord=new HoursRecord();

        viewOperationModalShow:boolean = false;

        //datos hardcodeados en el backend:
        finished=3;
        active=2;
        future=1;
        normalize_role(role){
            return role[0]+role.substring(1,role.length).toLowerCase();
        }
        single_role(){
            var store=this.$store.state.user;
            if(typeof store.viewUser !== 'undefined' && typeof store.viewUser.roleNames !== 'undefined'){
                return this.normalize_role(store.viewUser.roleNames[0]);
            }
            else{
                return 'Default';
            }
        }

        get loadingAssignation(){
            return this.$store.state.assignation.loading;
        }
        get loadingAttachment(){
            return this.$store.state.userfile.loading;
        }
        get loading(){
            return this.$store.state.assignation.loading;
        }
        get loadingHoursRecord(){
            return this.$store.state.hoursRecord.loading;
        }

        get assignations(){
            let assignments = this.$store.state.assignation.assignmentsByUsers;

            var assignationsReturn = [];
            for(var i = 0; i < assignments.length; i++) {

                if ( (assignments[i].operation.operationState.id == this.future ) || 
                    (assignments[i].operation.operationState.id == this.active )  ) {
                    if (assignments[i].aware ==null){
                        assignments[i].aware = "Sin Respuesta";
                    }else if (assignments[i].aware) {
                        assignments[i].aware = "Confirmada";
                    }else {
                        assignments[i].aware = "Rechazada";
                    }

                    assignationsReturn.push(assignments[i]);
                }
            }
            return assignationsReturn;
        }

        get hoursRecords() {
            return this.$store.state.hoursRecord.list;
        }

        get fileList(){
            return this.$store.state.userfile.list;
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
                this.user=Util.extend(true,{},this.$store.state.user.viewUser);
                this.$store.dispatch({
                    type: 'assignation/getAssignationsByUser',
                    data: this.user,
                });

                let pagerequest = { 
                    id: this.user.id
                }

                this.$store.dispatch({
                    type: 'hoursRecord/getAllByUserWithLocationControl',
                    data: pagerequest
                });
                
                this.$store.dispatch({
                    type: 'userfile/getFileList',
                    data: this.user.id
                });
            }
        }

        async downloadFile(){
            await this.$store.dispatch({
                type: 'userfile/get',
                data: this.$store.state.userfile.retrievedFile.id
            })
            var uri = "data:application/octet-stream;charset=utf-16le;base64,"+this.$store.state.userfile.retrievedFile.photo;
            var link = document.createElement('a');
            link.setAttribute("download", this.$store.state.userfile.retrievedFile.name);
            link.setAttribute("href", uri);
            link.click();
        }
        
        async getOperation(pagerequest) {
            return await this.$store.dispatch({
                type: 'operation/get',
                data: pagerequest
            })
        }

        async getAssignationsByUserAndState(pagerequest) {
            return await this.$store.dispatch({
                type: 'assignation/getAssignationsByUserAndState',
                data: pagerequest
            })
        }

        async getLocationRecordByUserAndTime(pagerequest) {
            return await this.$store.dispatch({
                type: 'locationRecord/getLocationRecordByUserAndTime',
                data: pagerequest
            })
        }

        viewOperationDetail(){
            this.viewOperationModalShow = true;
        }

        stateFilter(val:number){
            let pagerequest = { 
                                id: this.user.id,
                                state: val
                            }
            this.getAssignationsByUserAndState(pagerequest);
        }

        assColumns=[
            {
                title:'Operación',
                render:(h:any,params:any)=>{
                    return h('span',params.row.operation.id) 
                    }
            },
            {
                title:'Commodity',
                render:(h:any,params:any)=>{
                    return h('span',params.row.operation.commodity)
                }
            },
            {
                title:'Destino',
                render:(h:any,params:any)=>{
                   return h('span',params.row.operation.destiny)
                }
            },
            {
                title:'Fecha creación',
                render:(h:any,params:any)=>{
                   return h('span',moment(params.row.operation.date).locale('es').format("DD/MM/YYYY, HH:mm"))
                }
            },
            {
                title:'Comienzo',
                render:(h:any,params:any)=>{
                   return h('span',moment(params.row.date).locale('es').format("DD/MM/YYYY, HH:mm"))
                }
            },
            {
                title:'Fin',
                render:(h:any,params:any)=>{
                   return h('span',moment(params.row.dateFin).locale('es').format("DD/MM/YYYY, HH:mm"))
                }
            },
            {
                title:'Estado',
                render:(h:any,params:any)=>{
                   return h('span',params.row.operation.operationState.name)
                }
            },
            {
                title:'Asignación',
                render:(h:any,params:any)=>{
                   return h('span',params.row.aware)
                }
            },
            {
                title:this.L('Actions'),
                key:'Actions',
                width:100,
                render:(h:any,params:any)=>{
                    var toRender = [
                            h('Button',{
                                props:{
                                    type: 'info',
                                    size:'small'
                                },
                                style:{
                                    marginRight:'5px'
                                },
                                on:{
                                    click:()=>{
                                        let pagerequest = { 
                                                id: params.row.operationId
                                            }
                                        this.getOperation(pagerequest).then(result =>{
                                            let operation = {
                                                id: result["id"],
                                                bookingNumber: result["bookingNumber"],
                                                chargerId: result["charger"]["id"],
                                                chargerName: result["charger"]["name"],
                                                clientReference: result["clientReference"],
                                                commodity: result["commodity"],
                                                date: result["date"],
                                                destiny: result["destiny"],
                                                line: result["line"],
                                                location: result["location"]["name"],
                                                locationId: result["location"]["id"],
                                                managerId: result["manager"]["id"],
                                                managerName: result["manager"]["name"],
                                                nominatorId: result["nominator"]["id"],
                                                nominatorName: result["nominator"]["name"],
                                                notes: result["notes"],
                                                operationState: result["operationState"]["name"],
                                                operationStateId: result["operationState"]["id"],
                                                operationTypeId: result["operationType"]["id"],
                                                operationType: result["operationType"]["name"],
                                                package: result["package"],
                                                shipName: result["shipName"]
                                            }
                                            this.$store.commit('operation/view',operation);
                                            this.viewOperationDetail();
                                        });
                                    }
                                }
                            },'Ver')
                        ]
                    
                    return toRender;
                }
            }
        ]

        hoursColumns=[
            {
                title:'Ubicacion',
                render:(h:any,params:any)=>{
                    return h('span',params.row.operation.location.name)
                }
            },
            {
                title:'Entrada',
                render:(h:any,params:any)=>{
                   return h('span',moment(params.row.startDate).locale('es').format("DD/MM/YYYY, HH:mm"))
                }
            },
            {
                title:'Salida',
                render:(h:any,params:any)=>{
                    return h('span',moment(params.row.endDate).locale('es').format("DD/MM/YYYY, HH:mm"))
                }
            },
            {
                title:'Ubicacion',
                render:(h:any,params:any)=>{
                    return h('span',params.row.operation.location.name)
                }
            },
            {
                title:'Mercaderia',
                render:(h:any,params:any)=>{
                    return h('span',params.row.operation.commodity)
                }
            },
            {
                title:'Se mantuvo en posicion',
                render:(h:any,params:any)=>{

                    let toRender;
                    let iconType = '';
                    let iconColor ='';
                    console.log(params);
                    if(params.row.isThere){
                        iconType = 'md-checkmark-circle';
                        iconColor = 'green';
                    }else {
                        iconType = 'md-close-circle';
                        iconColor = 'red';
                    }

                    return h('Icon', 
                            { 
                                attrs: 
                                    { 
                                        type : iconType,
                                        size : 25,
                                        color: iconColor
                                    }
                            });
                }

                
            }
        ]

        fileColumns=[
            {
                title:'Id',
                key: 'id'
            },
            {
                title:'Nombre',
                key: 'name'
            },
            {
                title:this.L('Actions'),
                render:(h:any,params:any)=>{
                    var toRender = [
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
                                    this.$store.commit('userfile/setDownloadFile',params.row);
                                    this.downloadFile();
                                }
                            }
                        },this.L('Descargar'))
                    ];
                    return toRender;
                }
            }
        ]

    }
</script>
