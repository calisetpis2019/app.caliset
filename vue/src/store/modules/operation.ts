import { Store, Module, ActionContext } from 'vuex'
import ListModule from './list-module'
import ListState from './list-state'
import Operation from '../entities/operation'
import Ajax from '../../lib/ajax'
import PageResult from '@/store/entities/page-result';
import ListMutations from './list-mutations'
interface OperationState extends ListState<Operation> {
    editOperation: Operation;
    viewOperation: Object;
    commentOperation: Operation;
}
class OperationModule extends ListModule<OperationState, any, Operation>{
    state = {
        totalCount: 0,
        currentPage: 1,
        pageSize: 10,
        list: new Array<Operation>(),
        loading: false,
        editOperation: new Operation(),
        viewOperation: new Object(),
        commentOperation: new Operation()
    }
    actions = {
        async getAll(context: ActionContext<OperationState, any>) {
            context.state.loading = true;
            let reponse = await Ajax.get('/api/services/app/Operation/GetAll');
            context.state.loading = false;
            context.state.totalCount = reponse.data.result.length;
            context.state.list = reponse.data.result;
        },
        async getAllFilters(context: ActionContext<OperationState, any>, payload: any) {
            context.state.loading = true;
            let responseString = '';
            let filters = ['LocationId', 'OperationTypeId', 'NominatorId','ChargerId', 'OperationStateId', 'ManagerId','Keyword'];
            filters.forEach((filter)=>{
                if (payload.data[filter]){
                    responseString = responseString + filter + '=' + payload.data[filter] + '&';
                }
            });
            responseString = responseString.slice(0,-1);
            let reponse = await Ajax.get('/api/services/app/Operation/GetAllFilters?' + responseString);
            context.state.loading = false;
            context.state.totalCount = reponse.data.result.length;
            context.state.list = reponse.data.result;
        },
        async create(context: ActionContext<OperationState, any>, payload: any) {
            await Ajax.post('/api/services/app/Operation/Create', payload.data);
        },
        async update(context: ActionContext<OperationState, any>, payload: any) {
            await Ajax.put('/api/services/app/Operation/Update', payload.data);
        },
        async update_finished(context: ActionContext<OperationState, any>, payload: any) {
            await Ajax.put('/api/services/app/Operation/UpdateFinishedOperation', payload.data);
        },
        async delete(context: ActionContext<OperationState, any>, payload: any) {
            await Ajax.delete('/api/services/app/Operation/Delete?Id=' + payload.data.id);
        },
        async get(context: ActionContext<OperationState, any>, payload: any) {
            let reponse = await Ajax.get('/api/services/app/Operation/GetOperationById?Id=' + payload.data.id);
            return reponse.data.result as Operation;
        },
        async end(context: ActionContext<OperationState, any>, payload: any) {
            await Ajax.post('/api/services/app/Operation/EndOperation', payload);
        },
        async activate(context: ActionContext<OperationState, any>, payload: any) {
            await Ajax.post('/api/services/app/Operation/ActivateOperationById?id=' + payload.id);
        }

    };
    mutations = {
        setCurrentPage(state: OperationState, page: number) {
            state.currentPage = page;
        },
        setPageSize(state: OperationState, pagesize: number) {
            state.pageSize = pagesize;
        },
        edit(state: OperationState, operation: Operation) {
            state.editOperation = operation;
        },
        view(state: OperationState, operation: Object) {
            state.viewOperation = operation;
        },
        comment(state: OperationState, operation: Operation) {
            state.commentOperation = operation;
        }
    }
}
const operationModule = new OperationModule();
export default operationModule;