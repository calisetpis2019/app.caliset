<template>
    <div>
        <Modal title="Editar comentario"
               :value="value"
               @on-ok="save"
               @on-visible-change="visibleChange">
            <Form ref="editCommentForm" label-position="top" :rules="editCommentRule" :model="comment">
                <FormItem label="Comentario" prop="commodity">
                    <Input v-model="comment.commentary"></Input>
                </FormItem>
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
    import Comment from '@/store/entities/comment'

    class PageEditCommentOperationRequest extends PageRequest {
    }

    @Component
    export default class EditCommentOperation extends AbpBase{
        @Prop({type:Boolean,default:false}) value:boolean;
        comment:Comment=new Comment();
        created(){ }

        pagerequest: PageEditCommentOperationRequest = new PageEditCommentOperationRequest();

        save() {
            (this.$refs.editCommentForm as any).validate(async (valid:boolean)=>{
                if (valid) {
                    await this.$store.dispatch({
                        type:'comment/update',
                        data:this.comment
                    });
                    (this.$refs.editCommentForm as any).resetFields();
                    this.$emit('save-success');
                    this.$emit('input',false);
                }
            })
        }
        cancel(){
            (this.$refs.editCommentForm as any).resetFields();
            this.$emit('input',false);
        }
        visibleChange(value:boolean){
            if(!value){
                this.$emit('input',value);
            }
            else {
                this.comment = Util.extend(true, {}, this.$store.state.comment.editComment);
            }
        }

        editCommentRule={
            commentary     :[
                {required: true,message:this.L('FieldIsRequired',undefined,this.L('Commodity')),trigger: 'blur'}
            ]
        }
    }
</script>
