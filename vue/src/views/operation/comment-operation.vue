<template>
    <div>
        <Modal :title="L('Comentar operación')"
               :value="value"
               @on-ok="save"
               @on-visible-change="visibleChange">
            <Form ref="commentForm" label-position="top" :rules="commentRule" :model="comment">
                    <FormItem label="Comentario" prop="commentary">
                        <Input v-model="comment.commentary" ></Input>
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
    import Operation from '@/store/entities/operation'
    import Comment from '@/store/entities/comment'

    class PageCommentOperationRequest extends PageRequest {
    }

    @Component
    export default class CommentOperation extends AbpBase{
        @Prop({type:Boolean,default:false}) value:boolean;
        operation:Operation=new Operation();
        comment:Comment=new Comment();
        created(){

        }

        pagerequest: PageCommentOperationRequest = new PageCommentOperationRequest();

        save() {
            this.comment.operationId = this.operation.id;
            (this.$refs.commentForm as any).validate(async (valid:boolean)=>{
                if (valid) {
                    if(this.operation.operationStateId == 3){
                        await this.$store.dispatch({
                            type:'comment/createFinished',
                            data:this.comment
                        });
                    }
                    else{
                        await this.$store.dispatch({
                            type:'comment/create',
                            data:this.comment
                        });
                    }                   
                    (this.$refs.commentForm as any).resetFields();
                    this.$emit('save-success');
                    this.$emit('input',false);
                    this.$Message.success({content:'Comentario creado exitosamente.',duration:3.5});
                }
            })
        }
        cancel(){
            (this.$refs.commentForm as any).resetFields();
            this.$emit('input',false);
        }

        visibleChange(value:boolean){
            if(!value){
                this.$emit('input',value);
            } else {
                this.operation = Util.extend(true, {}, this.$store.state.operation.editOperation);
            }
        }

        commentRule={
            commentary :[
                { required: true,message:this.L('FieldIsRequired',undefined,this.L('Tipo')), trigger: 'blur' }
            ] 
        }
    }
</script>
