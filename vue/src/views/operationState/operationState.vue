<template>
    <div>
        <Card dis-hover>
            <div class="page-body">
                <Form ref="queryForm" :label-width="80" label-position="left" inline>
                    <Row :gutter="16">
                        <Col span="6">
                        <FormItem :label="L('Keyword')+':'" style="width:100%">
                            <Input v-model="pagerequest.keyword" :placeholder="L('OperationStateName')"></Input>
                        </FormItem>
                        </Col>
                    </Row>
                    <Row>
                        <Button @click="create" icon="android-add" type="primary" size="large">{{L('Add')}}</Button>
                        <Button icon="ios-search" type="primary" size="large" @click="getpage" class="toolbar-btn">{{L('Find')}}</Button>
                    </Row>
                </Form>
                <div class="margin-top-10">
                    <Table :loading="loading" :columns="columns" no-data-text="No existen registros" border :data="list">
                    </Table>
                    <Page show-sizer class-name="fengpage" :total="totalCount" class="margin-top-10" @on-change="pageChange" @on-page-size-change="pagesizeChange" :page-size="pageSize" :current="currentPage"></Page>
                </div>
            </div>
        </Card>
        <create-operationState v-model="createModalShow" @save-success="getpage"></create-operationState>
        <edit-operationState v-model="editModalShow" @save-success="getpage"></edit-operationState>
    </div>
</template>
<script lang="ts">
    import { Component, Vue, Inject, Prop, Watch } from 'vue-property-decorator';
    import Util from '@/lib/util'
    import AbpBase from '@/lib/abpbase'
    import PageRequest from '@/store/entities/page-request'
    import CreateOperationState from './create-operationState.vue'
    import EditOperationState from './edit-operationState.vue'

    class PageOperationStateRequest extends PageRequest {
    }

    @Component({
        components: { CreateOperationState, EditOperationState }
    })
    export default class OperationStates extends AbpBase {
        edit(){
            this.editModalShow=true;
        }
        pagerequest: PageOperationStateRequest = new PageOperationStateRequest();
        
        createModalShow: boolean = false;
        editModalShow:boolean=false;
        get list() {
            return this.$store.state.operationState.list;
        };
        get loading() {
            return this.$store.state.operationState.loading;
        }

        create() {
            this.createModalShow = true;
        }
        pageChange(page: number) {
            this.$store.commit('operationState/setCurrentPage', page);
            this.getpage();
        }
        pagesizeChange(pagesize: number) {
            this.$store.commit('operationState/setPageSize', pagesize);
            this.getpage();
        }

        async getpage() {

            await this.$store.dispatch({
                type: 'operationState/getAll',
                data: this.pagerequest
            })
            console.log(this.pagerequest);
        }
        get pageSize() {
            return this.$store.state.operationState.pageSize;
        }
        get totalCount() {
            return this.$store.state.operationState.totalCount;
        }
        get currentPage() {
            return this.$store.state.operationState.currentPage;
        }
        columns = [
            {
                title: this.L('Name'),
                key: 'name'
            },{
            title:this.L('Actions'),
            key:'Actions',
            width:150,
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
                                this.$store.commit('operationState/edit',params.row);
                                this.edit();
                            }
                        }
                    },this.L('Edit')),
                    h('Button',{
                        props:{
                            type:'error',
                            size:'small'
                        },
                        on:{
                            click:async ()=>{
                                this.$Modal.confirm({
                                        title:this.L('Tips'),
                                        content:this.L('DeleteOperationStateConfirm'),
                                        okText:this.L('Yes'),
                                        cancelText:this.L('No'),
                                        onOk:async()=>{
                                            await this.$store.dispatch({
                                                type:'operationState/delete',
                                                data:params.row
                                            })
                                            await this.getpage();
                                        }
                                })
                            }
                        }
                    },this.L('Delete'))
                ])
            }
        }]
        async created() {
            this.getpage();
        }
    }
</script>