<template>
    <div>
        <Card dis-hover>
            <div class="page-body">
                <Form ref="queryForm" :label-width="80" label-position="left" inline>
                    <Row :gutter="16">
                        <Col span="6">
                        <FormItem :label="L('Keyword')+':'" style="width:100%">
                            <Input v-model="pagerequest.keyword" :placeholder="L('OperationName')"></Input>
                        </FormItem>
                        </Col>
                    </Row>
                    <Row>
                        <Button @click="create" icon="android-add" type="primary" size="large">{{L('Add')}}</Button>
                        <Button icon="ios-search" type="primary" size="large" @click="getpage" class="toolbar-btn">{{L('Find')}}</Button>
                    </Row>
                </Form>
                <div class="margin-top-10">
                    <Table :loading="loading" :columns="columns" :no-data-text="L('NoDatas')" border :data="list">
                    </Table>
                    <Page show-sizer class-name="fengpage" :total="totalCount" class="margin-top-10" @on-change="pageChange" @on-page-size-change="pagesizeChange" :page-size="pageSize" :current="currentPage"></Page>
                </div>
            </div>
        </Card>
        <create-operation v-model="createModalShow" @save-success="getpage"></create-operation>
        <edit-operation v-model="editModalShow" @save-success="getpage"></edit-operation>
        <view-operation v-model="viewModalShow" @save-success="getpage"></view-operation>
        <assign-operation v-model="assignModalShow" @save-success="getpage"></assign-operation>
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
    import Operation from '../../store/entities/operation'
    import Location from '../../store/entities/location'
    import AssignOperation from './assign-operation.vue'
    import Assignation from '../../store/entities/assignation'

    class PageOperationRequest extends PageRequest {
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
        components: { CreateOperation, EditOperation, ViewOperation, AssignOperation }
    })
    export default class Operations extends AbpBase {
        edit(){
            this.editModalShow=true;
        }
        assign(){
            this.assignModalShow=true;
        }
        pagerequest: PageOperationRequest = new PageOperationRequest();
        
        createModalShow: boolean = false;
        editModalShow:boolean=false;
        viewModalShow: boolean = false;
        assignModalShow: boolean = false;
        
        get list() {
            var auxOperations:Operation[];
            var auxLocations:Location[];
            var result = [];

            auxOperations = this.$store.state.operation.list;
            auxLocations = this.$store.state.location.list;

            auxOperations.forEach( (element) => {
                
                result.push({
                                id: element["id"],
                                bookingNumber: element["bookingNumber"],
                                chargerName: element["charger"]["name"],
                                clientReference: element["clientReference"],
                                commodity: element["commodity"],
                                date: element["date"],
                                destination: element["destiny"],
                                line: element["line"],
                                location: auxLocations[+element["location"]["id"] - 1]["name"],
                                locationId: element["location"]["id"],
                                managerId: element["manager"]["id"],
                                managerName: element["manager"]["name"],
                                nominatorId: element["nominator"]["id"],
                                nominatorName: element["nominator"]["name"],
                                notes: element["notes"],
                                operationState: element["operationState"]["name"],
                                operationTypeId: element["operationType"]["id"],
                                operationType: element["operationType"]["name"],
                                package: element["package"],
                                shipName: element["shipName"]
                });
            })
            return result;
        };
        get loading() {
            return this.$store.state.operation.loading;
        }

        view() {
            this.viewModalShow = true;
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
                type: 'operation/getAll',
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
                title: this.L('Date'),
                key: 'date'
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
            width:200,
            render:(h:any,params:any)=>{
                return h('div',[
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
                                this.edit();
                            }
                        }
                    },this.L('Edit')),
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
                    },this.L('Assign')),
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
                    },this.L('Delete')),
                    h('Button',{
                        props:{
                            type:'primary',
                            size:'small'
                        },
                        on:{
                            click:()=>{
                                this.$store.commit('operation/view',params.row);
                                this.view();
                            }
                        }
                    },'Ver')
                ])
            }
        }]
        async created() {
            this.getpage();
        }
    }
</script>