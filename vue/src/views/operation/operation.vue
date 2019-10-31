<template>
    <div>
        <Card dis-hover>
            <div class="page-body">
                <Row :gutter="30">
                    <Col span="8" >
                        <Form ref="queryForm" :label-width="80" label-position="left" inline>
                            <Row :gutter="10">
                                <Col span="24">
                                <FormItem :label="L('Keyword')+':'" style="width:100%">
                                    <Input v-model="pagerequest.Keyword" :placeholder="L('OperationName')"></Input>
                                </FormItem>
                                </Col>
                            </Row>
                            <Row>
                                <Button @click="create" icon="android-add" type="primary" size="large" v-if="operatorRenderOnly">{{L('Add')}}</Button>
                                <Button icon="ios-search" type="primary" size="large" @click="filter" class="toolbar-btn">{{L('Find')}}</Button>
                            </Row>
                        </Form>
                    </Col>
                    <Col span="16" style="border-left: 2px solid #e8eaec">
                        <Form ref="filterForm" :label-width="80" label-position="left" inline>
                            Filtrar por:
                            <Row :gutter="10">
                                
                                <Col span="8">
                                    <FormItem label="Cargador" style="width:100%">
                                        <Select v-model="pagerequest.ChargerId"  filterable clearable>
                                            <Option v-for="item in listOfClients" :value="item.id" :key="item.id">{{ item.name }}</Option>
                                        </Select>
                                    </FormItem>
                                    <FormItem label="Tipo" style="width:100%">
                                        <Select v-model="pagerequest.OperationTypeId"  filterable clearable>
                                            <Option v-for="item in listOfOperationTypes" :value="item.id" :key="item.id">{{ item.name }}</Option>
                                        </Select>
                                    </FormItem>
                                </Col>
                                <Col span="8">
                                    <FormItem label="Lugar" style="width:100%">
                                        <Select v-model="pagerequest.LocationId"  filterable clearable>
                                            <Option v-for="item in listOfLocations" :value="item.id" :key="item.id">{{ item.name }}</Option>
                                        </Select>
                                    </FormItem>
                                    <FormItem label="Estado" style="width:100%">
                                        <Select v-model="pagerequest.OperationStateId"  filterable clearable>
                                            <Option v-for="item in listOfOperationStates" :value="item.id" :key="item.id">{{ item.name }}</Option>
                                        </Select>
                                    </FormItem>
                                </Col>
                                <Col span="8">
                                    <FormItem label="Nominador" style="width:100%">
                                        <Select v-model="pagerequest.NominatorId"  filterable clearable>
                                            <Option v-for="item in listOfClients" :value="item.id" :key="item.id">{{ item.name }}</Option>
                                        </Select>
                                    </FormItem>
                                    <FormItem label="Responsable" style="width:100%">
                                        <Select @on-change="getpagefilters" v-model="pagerequest.ManagerId"  filterable clearable>
                                            <Option v-for="item in listOfUsers" :value="item.id" :key="item.id">{{ item.name }}</Option>
                                        </Select>
                                    </FormItem>
                                </Col>
                            </Row>
                            <Button @click="filter" icon="ios-search" type="primary" size="large" >Filtrar</Button>

                        </Form>
                    </Col>
                </Row>
                <div class="margin-top-10">
                    <h4>Operaciones Activas</h4>
                    <Table :loading="loading" :columns="columns" no-data-text="No existen registros" border :data="list_active.sort(dynamicSort_asc('date'))">
                    </Table>
                </div>
                <div class="margin-top-10">
                    <h4>Operaciones Futuras</h4>
                    <Table :loading="loading" :columns="columns" no-data-text="No existen registros" border :data="list_future.sort(dynamicSort_asc('date'))">
                    </Table>
                </div>
                <div class="margin-top-10">
                    <h4>Operaciones Finalizadas</h4>
                    <Table :loading="loading" :columns="columns" no-data-text="No existen registros" border :data="list_finished.sort(dynamicSort_desc('date'))">
                    </Table>
                </div>
                <div><Page show-sizer class-name="fengpage" :total="totalCount" class="margin-top-10" @on-change="pageChange" @on-page-size-change="pagesizeChange" :page-size="pageSize" :current="currentPage"></Page></div>
            </div>
        </Card>
        <create-operation v-model="createModalShow"  @save-success="getpage"></create-operation>
        <edit-operation v-model="editModalShow"  @save-success="getpage"></edit-operation>
        <view-operation v-model="viewModalShow" @save-success="getpage"></view-operation>
        <assign-operation v-model="assignModalShow" @save-success="getpage"></assign-operation>
        <comment-operation v-model="commentModalShow" ></comment-operation>
    </div>
</template>
<script lang="ts">
    import { Component, Vue, Inject, Prop, Watch } from 'vue-property-decorator';
    import Util from '@/lib/util'
    import AbpBase from '@/lib/abpbase'
    import PageRequest from '@/store/entities/page-request'
    import CreateOperation from './create-operation.vue'
    import EditOperation from './edit-operation.vue'
    import ViewOperation from './view-operation.vue'
    import CommentOperation from './comment-operation.vue'
    import Operation from '../../store/entities/operation'
    import Location from '../../store/entities/location'
    import User from '../../store/entities/user'
    import Client from '../../store/entities/client'
    import OperationState from '../../store/entities/OperationState'
    import AssignOperation from './assign-operation.vue'
    import Assignation from '../../store/entities/assignation'
    import Comment from '../../store/entities/comment'
    import moment from 'moment'

    class PageOperationRequest extends PageRequest {
        LocationId: number;
        OperationTypeId: number;
    }

    class OperationForListing {
        location: string;
        date: Date;
        operationState: string;
        commodity: string;
        constructor(l: string, d: Date, s: string, c: string) {
            this.location = l;
            this.date = d;
            this.operationState = s;
            this.commodity = c;
        }
    }

    @Component({
        components: { CreateOperation, EditOperation, ViewOperation, AssignOperation, CommentOperation }
    })
    export default class Operations extends AbpBase {
        edit(){
            this.editModalShow=true;
        }
        assign(){
            this.assignModalShow=true;
        }
        pagerequest: PageOperationRequest = new PageOperationRequest();
        operatorRenderOnly: boolean = Util.abp.auth.hasPermission('Pages.Operador');
        administratorRenderOnly: boolean = Util.abp.auth.hasPermission('Pages.Administrador');

        createModalShow: boolean = false;
        editModalShow:boolean=false;
        viewModalShow: boolean = false;
       	assignModalShow: boolean = false;
        commentModalShow: boolean = false;

        //datos hardcodeados en el backend:
        ended=3;
        active=2;
        future=1;
        
        get list_finished() {
            var auxOperations:Operation[];
            var auxLocations:Location[];
            var result = [];
            auxOperations = this.$store.state.operation.list;
            auxLocations = this.$store.state.location.list;
            auxOperations.forEach( (element) => {
                //Finished==3
                if(element["operationState"]["id"]==3){
                    let locationName = "";
                    auxLocations.forEach( (location) =>{
                        if (location.id == element["location"]["id"]){
                            locationName = location.name;
                        }
                    })                    
                    result.push({
                        id: element["id"],
                        bookingNumber: element["bookingNumber"],
                        chargerId: element["charger"]["id"],
                        chargerName: element["charger"]["name"],
                        clientReference: element["clientReference"],
                        commodity: element["commodity"],
                        date: element["date"],
                        destiny: element["destiny"],
                        line: element["line"],
                        location: locationName,
                        locationId: element["location"]["id"],
                        managerId: element["manager"]["id"],
                        managerName: element["manager"]["name"],
                        nominatorId: element["nominator"]["id"],
                        nominatorName: element["nominator"]["name"],
                        notes: element["notes"],
                        operationState: element["operationState"]["name"],
                        operationStateId: element["operationState"]["id"],
                        operationTypeId: element["operationType"]["id"],
                        operationType: element["operationType"]["name"],
                        package: element["package"],
                        shipName: element["shipName"]
                    });
                }
            })
            return result;
        };

    	get list_active() {
            var auxOperations:Operation[];
            var auxLocations:Location[];
            var result = [];
            auxOperations = this.$store.state.operation.list;
            auxLocations = this.$store.state.location.list;
            auxOperations.forEach( (element) => {
                //Active==2
                if(element["operationState"]["id"]==2){
                    let locationName = "";
                    auxLocations.forEach( (location) =>{
                        if (location.id == element["location"]["id"]){
                            locationName = location.name;
                        }
                    })                    
                    result.push({
                        id: element["id"],
                        bookingNumber: element["bookingNumber"],
                        chargerId: element["charger"]["id"],
                        chargerName: element["charger"]["name"],
                        clientReference: element["clientReference"],
                        commodity: element["commodity"],
                        date: element["date"],
                        destiny: element["destiny"],
                        line: element["line"],
                        location: locationName,
                        locationId: element["location"]["id"],
                        managerId: element["manager"]["id"],
                        managerName: element["manager"]["name"],
                        nominatorId: element["nominator"]["id"],
                        nominatorName: element["nominator"]["name"],
                        notes: element["notes"],
                        operationState: element["operationState"]["name"],
                        operationStateId: element["operationState"]["id"],
                        operationTypeId: element["operationType"]["id"],
                        operationType: element["operationType"]["name"],
                        package: element["package"],
                        shipName: element["shipName"]
                    });
                }
            })
            return result;
        };

        get list_future() {
            var auxOperations:Operation[];
            var auxLocations:Location[];
            var result = [];
            auxOperations = this.$store.state.operation.list;
            auxLocations = this.$store.state.location.list;
            auxOperations.forEach( (element) => {
                //Future==1
                if(element["operationState"]["id"]==1){
                    let locationName = "";
                    auxLocations.forEach( (location) =>{
                        if (location.id == element["location"]["id"]){
                            locationName = location.name;
                        }
                    })                    
                    result.push({
                        id: element["id"],
                        bookingNumber: element["bookingNumber"],
                        chargerId: element["charger"]["id"],
                        chargerName: element["charger"]["name"],
                        clientReference: element["clientReference"],
                        commodity: element["commodity"],
                        date: element["date"],
                        destiny: element["destiny"],
                        line: element["line"],
                        location: locationName,
                        locationId: element["location"]["id"],
                        managerId: element["manager"]["id"],
                        managerName: element["manager"]["name"],
                        nominatorId: element["nominator"]["id"],
                        nominatorName: element["nominator"]["name"],
                        notes: element["notes"],
                        operationState: element["operationState"]["name"],
                        operationStateId: element["operationState"]["id"],
                        operationTypeId: element["operationType"]["id"],
                        operationType: element["operationType"]["name"],
                        package: element["package"],
                        shipName: element["shipName"]
                    });
                }
            })
            return result;
        };

        dynamicSort_asc(property:string) {
            var sortOrder = 1;
            if(property[0] === "-") {
                sortOrder = -1;
                property = property.substr(1);
            }
            return function (a,b) {
                var result = (a[property] < b[property]) ? -1 : (a[property] > b[property]) ? 1 : 0;
                return result * sortOrder;
            }
        };

        dynamicSort_desc(property:string) {
            var sortOrder = 1;
            if(property[0] === "-") {
                sortOrder = -1;
                property = property.substr(1);
            }
            return function (a,b) {
                var result = (a[property] < b[property]) ? 1 : (a[property] > b[property]) ? -1 : 0;
                return result * sortOrder;
            }
        };

        get loading() {
            return this.$store.state.operation.loading;
        }
        get listOfLocations(){
            return this.$store.state.location.list;
        }
        get listOfClients(){
            return this.$store.state.client.list;
        }
        get listOfOperationTypes(){
            return this.$store.state.operationType.list;
        }
        get listOfOperationStates(){
            return this.$store.state.operationState.list;
        }
        get listOfUsers(){
            return this.$store.state.user.list;
        }

        view() {
            this.viewModalShow = true;
        }

        comment(){
            this.commentModalShow = true;
        }

        create() {
            this.createModalShow = true;
        }
        pageChange(page: number) {
            this.$store.commit('operation/setCurrentPage', page);
            this.getpage();
        }
        pagesizeChange(pagesize: number) {
            this.$store.commit('operation/setPageSize', pagesize);
            this.getpage();
        }

        async getpage() {
            await this.$store.dispatch({
                type: 'location/getAll',
                data: this.pagerequest
            })

            await this.$store.dispatch({
                type: 'operationType/getAll',
                data: this.pagerequest
            })

            await this.$store.dispatch({
                type: 'operation/getAll',
                data: this.pagerequest
            })

            await this.$store.dispatch({
                type: 'client/getAll',
                data: this.pagerequest
            })

            await this.$store.dispatch({
                type: 'operationState/getAll',
                data: this.pagerequest
            })

            await this.$store.dispatch({
                type: 'user/getAll',
                data: this.pagerequest
            })

        }

        filter() {
            (this.$refs.filterForm as any).resetFields();
            this.getpagefilters();
        }

        async getpagefilters(){
            await this.$store.dispatch({
                type: 'operation/getAllFilters',
                data: this.pagerequest
            })
        }

        get pageSize() {
            return this.$store.state.operation.pageSize;
        }
        get totalCount() {
            return this.$store.state.operation.totalCount;
        }
        get currentPage() {
            return this.$store.state.operation.currentPage;
        }
        columns = [
            {
                title: 'Fecha',
                key: 'date',
                render: (h, params) => {
                    return h('div', [
                        h('span', moment(params.row.date).locale('es').format("DD/MM/YYYY"))
                    ]);
                }
            },
            {
                title: this.L('Ubicación'),
                key: 'location'
            },
            {
                title: this.L('Estado'),
                key: 'operationState'
            },
            {
                title: this.L('Mercadería'),
                key: 'commodity'
            },{
            title:this.L('Actions'),
            key:'Actions',
            width:420,
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
                                this.$store.commit('operation/view',params.row);
                                this.view();
                            }
                        }
                    },'Ver')
                ];
                var botonEditar=h('Button',{
                                    props:{
                                        type:'success',
                                        size:'small'
                                    },
                                    style:{
                                        marginRight:'5px'
                                    },
                                    on:{
                                        click:()=>{
                                            this.$store.commit('operation/edit',params.row);
                                            this.edit();
                                        }
                                    }
                                },'Editar');
                if(this.operatorRenderOnly == true && params.row.operationStateId != this.ended){
                    toRender.push(botonEditar)
                }
                else if(this.administratorRenderOnly){
                    toRender.push(botonEditar)
                }
                if(this.administratorRenderOnly == true && params.row.operationStateId != this.ended){
                    toRender.push(
                        h('Button',{
                            props:{
                                type:'error',
                                size:'small'
                            },
                            style:{
                                marginRight:'5px'
                            },
                            on:{
                                click:async ()=>{
                                    this.$Modal.confirm({
                                        title:this.L('Tips'),
                                        content:this.L('DeleteOperationConfirm'),
                                        okText:this.L('Yes'),
                                        cancelText:this.L('No'),
                                        onOk:async()=>{
                                            await this.$store.dispatch({
                                                type:'operation/delete',
                                                data:params.row
                                            })
                                            await this.getpage();
                                        }
                                    })
                                }
                            }
                        },'Eliminar')
                    );
                }
                if(this.operatorRenderOnly == true && params.row.operationStateId == 1){
                    toRender.push(
                        h('Button',{
                            props:{
                                type:'warning',
                                size:'small',
                            },
                            style:{
                                marginRight:'5px'
                            },
                            on:{
                                click:async ()=>{
                                    this.$store.dispatch('operation/activate',params.row);
                                    await this.getpage();
                                }
                            }
                        },'Activar')
                    );
                }
                if(this.operatorRenderOnly == true && params.row.operationStateId != 3){
                    toRender.push(
                        h('Button',{
                            props:{
                                type:'primary',
                                size:'small'
                            },
                            style:{
                                marginRight:'5px'
                            },
                            on:{
                                click:()=>{
                                    this.$store.commit('operation/edit',params.row);
                                    this.assign();
                                }
                            }
                        },'Asignar')
                    );
                }
                if (params.row.operationStateId != 3){
                    toRender.push(
                        h('Button',{
                            props:{
                                type:'error',
                                size:'small',
                            },
                            style:{
                                marginRight:'5px'
                            },
                            on:{
                                click:async ()=>{
                                    this.$store.dispatch('operation/end',params.row);
                                    await this.getpage();
                                }
                            }
                        },'Finalizar')
                    );
                }

                if (Util.abp.auth.hasPermission('Pages.Administrador') || params.row.operationStateId != 3){
                    toRender.push(
                        h('Button',{
                            props:{
                                type:'warning',
                                size:'small',
                            },
                            style:{
                                marginRight:'5px'
                            },
                            on:{
                                click:() =>{
                                    this.$store.commit('operation/edit',params.row);
                                    this.comment();
                                }
                            }
                        },'Comentar')
                    );    
                }
                return h('div', toRender);    
            }
        }]
        async created() {
            this.getpage();
        }
    }
</script>
