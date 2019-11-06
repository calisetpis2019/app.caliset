import { Store, Module, ActionContext } from 'vuex'
import ListModule from './list-module'
import ListState from './list-state'
import OperationState from '../entities/operationState'
import Ajax from '../../lib/ajax'
import PageResult from '@/store/entities/page-result';
import ListMutations from './list-mutations'
interface OperationStateState extends ListState<OperationState> {
    editOperationState: OperationState;
}
class OperationStateModule extends ListModule<OperationStateState, any, OperationState>{
    state = {
        totalCount: 0,
        currentPage: 1,
        pageSize: 10,
        list: new Array<OperationState>(),
        loading: false,
        editOperationState: new OperationState()
    }
    actions = {
        async getAll(context: ActionContext<OperationStateState, any>) {
            context.state.loading = true;
            let reponse = await Ajax.get('/api/services/app/OperationState/GetAll');
            context.state.loading = false;
            context.state.totalCount = reponse.data.result.length;
            context.state.list = reponse.data.result;
        },
        async create(context: ActionContext<OperationStateState, any>, payload: any) {
            await Ajax.post('/api/services/app/OperationState/Create', payload.data);
        },
        async update(context: ActionContext<OperationStateState, any>, payload: any) {
            await Ajax.put('/api/services/app/OperationState/Update', payload.data);
        },
        async delete(context: ActionContext<OperationStateState, any>, payload: any) {
            await Ajax.delete('/api/services/app/OperationState/Delete?Id=' + payload.data.id);
        },
        async get(context: ActionContext<OperationStateState, any>, payload: any) {
            let reponse = await Ajax.get('/api/services/app/OperationState/GetOperationStateById?Id=' + payload.id);
            return reponse.data.result as OperationState;
        }
    };
    mutations = {
        setCurrentPage(state: OperationStateState, page: number) {
            state.currentPage = page;
        },
        setPageSize(state: OperationStateState, pagesize: number) {
            state.pageSize = pagesize;
        },
        edit(state: OperationStateState, operationState: OperationState) {
            state.editOperationState = operationState;
        }
    }
}
const operationStateModule = new OperationStateModule();
export default operationStateModule;