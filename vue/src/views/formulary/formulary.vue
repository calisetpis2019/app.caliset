<template>
    <div>
        <Card dis-hover>
            <div class="page-body">
                <Row>
                    <Button @click="uploadClick" icon="android-add" type="primary" size="large">{{L('Add')}}</Button>
                </Row>                
                <div class="margin-top-10">
                    <Table :loading="loading" :columns="columns" no-data-text="No existen registros" border :data="list">
                    </Table>
                </div>
            </div>
        </Card>
        <input style="visibility:hidden" id="uploadFileElement_formulary" type="file" @change="onFileChanged">
    </div>
</template>
<script lang="ts">
    import { Component, Vue, Inject, Prop, Watch } from 'vue-property-decorator';
    import Util from '@/lib/util'
    import AbpBase from '@/lib/abpbase'
    import PageRequest from '@/store/entities/page-request'
    import FormularyEntity from '@/store/entities/formulary'

    class PageFormularyRequest extends PageRequest { }

    @Component({ })
    export default class Formulary extends AbpBase {
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

        pagerequest: PageFormularyRequest = new PageFormularyRequest();

        get list() {
            return this.$store.state.formulary.list;
        };
        get loading() {
            return this.$store.state.formulary.loading;
        }

        //Form File Code
        selectedFileName:string;
        uploadButton:any;
        onFileChanged(e){
            var thisthis = this;
            if (e.target.files && e.target.files[0]) {
                this.selectedFileName=e.target.files[0].name;
                this.readFile(e.target.files[0], async function(e) {
                    let encoded = e.target.result.toString().replace(/^data:(.*,)?/, '');
                    if ((encoded.length % 4) > 0) {
                        encoded += '='.repeat(4 - (encoded.length % 4));
                    }
                    let pagerequest = { 
                        name: thisthis.selectedFileName,
                        photo: encoded
                    }
                    await thisthis.$store.dispatch({
                        type:'formulary/create',
                        data: pagerequest
                    });
                    thisthis.reloadImages();
                    thisthis.$Message.success({content:'Archivo subido exitosamente.',duration:4});
                });
            }
        }
        readFile(file, onLoadCallback){
            var reader = new FileReader();
            reader.onload = onLoadCallback;
            reader.readAsDataURL(file);
        }
        uploadClick(){
            this.uploadButton.click();
        }
        reloadImages(){
            this.$store.dispatch({
                type: 'formulary/getAll',
                data: ""
            });
        }
        async deleteForm(idFile){
            await this.$store.dispatch({
                type: 'formulary/delete',
                data: idFile
            });
            this.reloadImages();
        }
        //END Form File Code



        async getpage() {
            await this.$store.dispatch({
                type: 'formulary/getAll',
                data: this.pagerequest
            })
        }

        columns = [
            {
                title: this.L('Id'),
                key: 'id',
                width:80
            },
            {
                title: this.L('Name'),
                key: 'name'
            },
            {
                title:this.L('Actions'),
                key:'Actions',
                width:180,
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
                            click:async ()=>{
                                await this.$store.commit('formulary/setFormIdAction',params.row.id);
                                this.downloadFile();
                            }
                        }
                        },this.L('Descargar')),
                        h('Button',{
                            props:{
                                type:'error',
                                size:'small'
                            },
                            on:{
                                click:async ()=>{
                                    this.$Modal.confirm({
                                            title:"Atención",
                                            content:"¿Está seguro que desea eliminar el Formulario?",
                                            okText:this.L('Yes'),
                                            cancelText:this.L('No'),
                                            onOk:async()=>{
                                                this.deleteForm(params.row.id);
                                            }
                                    })
                                }
                            }
                        },this.L('Delete'))
                    ])
                }
            }
        ]
        async created() {
            this.getpage();
        }
        async mounted(){
            this.uploadButton=document.getElementById('uploadFileElement_formulary');
        }
    }
</script>