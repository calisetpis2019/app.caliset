import { Store, Module, ActionContext } from 'vuex'
import ListModule from './list-module'
import ListState from './list-state'
import Operation from '../entities/operation'
import Ajax from '../../lib/ajax'
import PageResult from '@/store/entities/page-result';
import ListMutations from './list-mutations'
interface OperationState extends ListState<Operation> {
    editOperation: Operation;
}
class OperationModule extends ListModule<OperationState, any, Operation>{
    state = {
        totalCount: 0,
        currentPage: 1,
        pageSize: 10,
        list: new Array<Operation>(),
        loading: false,
        editOperation: new Operation()
    }
    actions = {
        async getAll(context: ActionContext<OperationState, any>) {
            context.state.loading = true;
            let reponse = await Ajax.get('/api/services/app/Operation/GetAll');
            context.state.loading = false;
            context.state.totalCount = reponse.data.result.length;
            context.state.list = reponse.data.result;
        },
        async create(context: ActionContext<OperationState, any>, payload: any) {
            await Ajax.post('/api/services/app/Operation/Create', payload.data);
        },
        async update(context: ActionContext<OperationState, any>, payload: any) {
            console.log("update: ");
            console.log(payload.data);
            await Ajax.put('/api/services/app/Operation/Update', payload.data);
        },
        async delete(context: ActionContext<OperationState, any>, payload: any) {
            await Ajax.delete('/api/services/app/Operation/Delete?Id=' + payload.data.id);
        },
        async get(context: ActionContext<OperationState, any>, payload: any) {
            let reponse = await Ajax.get('/api/services/app/Operation/GetOperationById?Id=' + payload.id);
            return reponse.data.result as Operation;
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
        }
    }
}
const operationModule = new OperationModule();
export default operationModule;