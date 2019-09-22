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
    </div>
</template>
<script lang="ts">
    import { Component, Vue, Inject, Prop, Watch } from 'vue-property-decorator';
    import Util from '@/lib/util'
    import AbpBase from '@/lib/abpbase'
    import PageRequest from '@/store/entities/page-request'
    import CreateOperation from './create-operation.vue'
    import EditOperation from './edit-operation.vue'

    class PageOperationRequest extends PageRequest {
    }

    @Component({
        components: { CreateOperation, EditOperation }
    })
    export default class Operations extends AbpBase {
        edit(){
            this.editModalShow=true;
        }
        pagerequest: PageOperationRequest = new PageOperationRequest();
        
        createModalShow: boolean = false;
        editModalShow:boolean=false;
        get list() {
            return this.$store.state.operation.list;
        };
        get loading() {
            return this.$store.state.operation.loading;
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
                title: this.L('Location'),
                key: 'location'
            },
            {
                title: this.L('State'),
                key: 'state'
            },
            {
                title: this.L('Commodity'),
                key: 'commodity'
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
                                this.$store.commit('operation/edit',params.row);
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
                    },this.L('Delete'))
                ])
            }
        }]
        async created() {
            this.getpage();
        }
    }
</script>