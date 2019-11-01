import { Store, Module, ActionContext } from 'vuex'
import ListModule from './list-module'
import ListState from './list-state'
import HoursRecord from '../entities/hoursRecord'
import Ajax from '../../lib/ajax'
import PageResult from '@/store/entities/page-result';
import ListMutations from './list-mutations'
interface HoursRecordState extends ListState<HoursRecord> {
    editHoursRecord: HoursRecord;
}
class HoursRecordModule extends ListModule<HoursRecordState, any, HoursRecord>{
    state = {
        totalCount: 0,
        currentPage: 1,
        pageSize: 10,
        list: new Array<HoursRecord>(),
        loading: false,
        editHoursRecord: new HoursRecord()
    }
    actions = {
        async getAll(context: ActionContext<HoursRecordState, any>) {
            context.state.loading = true;
            let reponse = await Ajax.get('/api/services/app/HoursRecord/GetAll');
            context.state.loading = false;
            context.state.totalCount = reponse.data.result.length;
            context.state.list = reponse.data.result;
        },
        async getAllByUser(context: ActionContext<HoursRecordState, any>, payload: any) {
            context.state.loading = true;
            let reponse = await Ajax.get('/api/services/app/HoursRecord/GetAllByUser?IdUser=' + payload.data.id);
            context.state.loading = false;
            context.state.totalCount = reponse.data.result.length;
            context.state.list = reponse.data.result;
        },
        async create(context: ActionContext<HoursRecordState, any>, payload: any) {
            await Ajax.post('/api/services/app/HoursRecord/Create', payload.data);
        },
        async update(context: ActionContext<HoursRecordState, any>, payload: any) {
            await Ajax.put('/api/services/app/HoursRecord/Update', payload.data);
        },
        async delete(context: ActionContext<HoursRecordState, any>, payload: any) {
            await Ajax.delete('/api/services/app/HoursRecord/Delete?Id=' + payload.data.id);
        },
        async get(context: ActionContext<HoursRecordState, any>, payload: any) {
            let reponse = await Ajax.get('/api/services/app/HoursRecord/GetHoursRecordById?Id=' + payload.data.id);
            return reponse.data.result as HoursRecord;
        }
    };
    mutations = {
        setCurrentPage(state: HoursRecordState, page: number) {
            state.currentPage = page;
        },
        setPageSize(state: HoursRecordState, pagesize: number) {
            state.pageSize = pagesize;
        },
        edit(state: HoursRecordState, hoursRecord: HoursRecord) {
            state.editHoursRecord = hoursRecord;
        }
    }
}
const hoursRecordModule = new HoursRecordModule();
export default hoursRecordModule;