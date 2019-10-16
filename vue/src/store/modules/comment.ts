import { Store, Module, ActionContext } from 'vuex'
import ListModule from './list-module'
import ListState from './list-state'
import Comment from '../entities/comment'
import Ajax from '../../lib/ajax'
import PageResult from '@/store/entities/page-result';
import ListMutations from './list-mutations'
interface CommentState extends ListState<Comment> {
    editComment: Comment;
    commentsOfOperation: Array<Comment>;
}
class CommentModule extends ListModule<CommentState, any, Comment>{
    state = {
        totalCount: 0,
        currentPage: 1,
        pageSize: 10,
        list: new Array<Comment>(),
        loading: false,
        editComment: new Comment(),
        commentsOfOperation: new Array<Comment>()
    }
    actions = {
        async getAll(context: ActionContext<CommentState, any>) {
            context.state.loading = true;
            let reponse = await Ajax.get('/api/services/app/Comments/GetAll');
            context.state.loading = false;
            context.state.totalCount = reponse.data.result.length;
            context.state.list = reponse.data.result;
        },
        async create(context: ActionContext<CommentState, any>, payload: any) {
            await Ajax.post('/api/services/app/Comments/Create', payload.data);
        },
        async update(context: ActionContext<CommentState, any>, payload: any) {
            await Ajax.put('/api/services/app/Comments/Update', payload.data);
        },
        async delete(context: ActionContext<CommentState, any>, payload: any) {
            await Ajax.delete('/api/services/app/Comments/Delete?Id=' + payload.data.id);
        },
        async get(context: ActionContext<CommentState, any>, payload: any) {
            let reponse = await Ajax.get('/api/services/app/Comments/GetCommentById?Id=' + payload.id);
            return reponse.data.result as Comment;
        },
        async getCommentsByOperation(context: ActionContext<CommentState, any>, payload: any) {
            let reponse = await Ajax.get('/api/services/app/Comments/GetCommentsByOperation?operationId=' + payload.data.id);
            context.state.commentsOfOperation = reponse.data.result as Array<Comment>;
            console.log(reponse.data.result);
        }
    };
    mutations = {
        setCurrentPage(state: CommentState, page: number) {
            state.currentPage = page;
        },
        setPageSize(state: CommentState, pagesize: number) {
            state.pageSize = pagesize;
        },
        edit(state: CommentState, comment: Comment) {
            state.editComment = comment;
        }
    }
}
const commentModule = new CommentModule();
export default commentModule;