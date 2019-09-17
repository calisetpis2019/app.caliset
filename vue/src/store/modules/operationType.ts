import { Store, Module, ActionContext } from 'vuex'
import ListModule from './list-module'
import ListState from './list-state'
import OperationType from '../entities/operationType'
import Ajax from '../../lib/ajax'
import PageResult from '@/store/entities/page-result';
import ListMutations from './list-mutations'
interface OperationTypeState extends ListState<OperationType> {
    editOperationType: OperationType;
}
class OperationTypeModule extends ListModule<OperationTypeState, any, OperationType>{
    state = {
        totalCount: 0,
        currentPage: 1,
        pageSize: 10,
        list: new Array<OperationType>(),
        loading: false,
        editOperationType: new OperationType()
    }
    actions = {
        async getAll(context: ActionContext<OperationTypeState, any>) {
            context.state.loading = true;
            let reponse = await Ajax.get('/api/services/app/OperationType/GetAll');
            context.state.loading = false;
            context.state.totalCount = reponse.data.result.length;
            context.state.list = reponse.data.result;
        },
        async create(context: ActionContext<OperationTypeState, any>, payload: any) {
            await Ajax.post('/api/services/app/OperationType/Create', payload.data);
        },
        async update(context: ActionContext<OperationTypeState, any>, payload: any) {
            console.log("update: ");
            console.log(payload.data);
            await Ajax.put('/api/services/app/OperationType/Update', payload.data);
        },
        async delete(context: ActionContext<OperationTypeState, any>, payload: any) {
            await Ajax.delete('/api/services/app/OperationType/Delete?Id=' + payload.data.id);
        },
        async get(context: ActionContext<OperationTypeState, any>, payload: any) {
            let reponse = await Ajax.get('/api/services/app/OperationType/GetOperationTypeById?Id=' + payload.id);
            return reponse.data.result as OperationType;
        }
    };
    mutations = {
        setCurrentPage(state: OperationTypeState, page: number) {
            state.currentPage = page;
        },
        setPageSize(state: OperationTypeState, pagesize: number) {
            state.pageSize = pagesize;
        },
        edit(state: OperationTypeState, operationType: OperationType) {
            state.editOperationType = operationType;
        }
    }
}
const operationTypeModule = new OperationTypeModule();
export default operationTypeModule;