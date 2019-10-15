<template>
    <div>
        <Card dis-hover>
            <div class="page-body">
                <Form ref="queryForm" :label-width="80" label-position="left" inline>
                    <Row :gutter="16">
                        <Col span="6">
                        <FormItem :label="L('Keyword')+':'" style="width:100%">
                            <Input v-model="pagerequest.keyword" :placeholder="L('OperationTypeName')"></Input>
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
        <create-operationType v-model="createModalShow" @save-success="getpage"></create-operationType>
        <edit-operationType v-model="editModalShow" @save-success="getpage"></edit-operationType>
    </div>
</template>
<script lang="ts">
    import { Component, Vue, Inject, Prop, Watch } from 'vue-property-decorator';
    import Util from '@/lib/util'
    import AbpBase from '@/lib/abpbase'
    import PageRequest from '@/store/entities/page-request'
    import CreateOperationType from './create-operationType.vue'
    import EditOperationType from './edit-operationType.vue'

    class PageOperationTypeRequest extends PageRequest {
    }

    @Component({
        components: { CreateOperationType, EditOperationType }
    })
    export default class OperationTypes extends AbpBase {
        edit(){
            this.editModalShow=true;
        }
        pagerequest: PageOperationTypeRequest = new PageOperationTypeRequest();
        
        createModalShow: boolean = false;
        editModalShow:boolean=false;
        get list() {
            return this.$store.state.operationType.list;
        };
        get loading() {
            return this.$store.state.operationType.loading;
        }

        create() {
            this.createModalShow = true;
        }
        pageChange(page: number) {
            this.$store.commit('operationType/setCurrentPage', page);
            this.getpage();
        }
        pagesizeChange(pagesize: number) {
            this.$store.commit('operationType/setPageSize', pagesize);
            this.getpage();
        }

        async getpage() {

            await this.$store.dispatch({
                type: 'operationType/getAll',
                data: this.pagerequest
            })
        }
        get pageSize() {
            return this.$store.state.operationType.pageSize;
        }
        get totalCount() {
            return this.$store.state.operationType.totalCount;
        }
        get currentPage() {
            return this.$store.state.operationType.currentPage;
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
                                this.$store.commit('operationType/edit',params.row);
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
                                        content:this.L('DeleteOperationTypeConfirm'),
                                        okText:this.L('Yes'),
                                        cancelText:this.L('No'),
                                        onOk:async()=>{
                                            await this.$store.dispatch({
                                                type:'operationType/delete',
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