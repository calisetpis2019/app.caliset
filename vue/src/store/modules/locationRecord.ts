import { Store, Module, ActionContext } from 'vuex'
import ListModule from './list-module'
import ListState from './list-state'
import LocationRecord from '../entities/locationRecord'
import Ajax from '../../lib/ajax'
import PageResult from '@/store/entities/page-result';
import ListMutations from './list-mutations'
interface LocationRecordState extends ListState<LocationRecord> {
    editLocationRecord: LocationRecord;
}
class LocationRecordModule extends ListModule<LocationRecordState, any, LocationRecord>{
    state = {
        totalCount: 0,
        currentPage: 1,
        pageSize: 10,
        list: new Array<LocationRecord>(),
        loading: false,
        editLocationRecord: new LocationRecord()
    }
    actions = {
        async getAll(context: ActionContext<LocationRecordState, any>) {
            context.state.loading = true;
            let reponse = await Ajax.get('/api/services/app/LocationRecords/GetAll');
            context.state.loading = false;
            context.state.totalCount = reponse.data.result.length;
            context.state.list = reponse.data.result;
        },
        async create(context: ActionContext<LocationRecordState, any>, payload: any) {
            await Ajax.post('/api/services/app/LocationRecords/Create', payload.data);
        },
        async update(context: ActionContext<LocationRecordState, any>, payload: any) {
            await Ajax.put('/api/services/app/LocationRecords/Update', payload.data);
        },
        async delete(context: ActionContext<LocationRecordState, any>, payload: any) {
            await Ajax.delete('/api/services/app/LocationRecords/Delete?Id=' + payload.data.id);
        },
        async get(context: ActionContext<LocationRecordState, any>, payload: any) {
            let reponse = await Ajax.get('/api/services/app/LocationRecords/GetLocationRecordById?Id=' + payload.data.id);
            return reponse.data.result as LocationRecord;
        },
        async getLocationRecordByUserAndTime(context: ActionContext<LocationRecordState, any>, payload: any) {
            let responseString = "?Id=" + payload.data.id;
            if(payload.data["begin"]){
                responseString = responseString +"&begin=" + payload.data.begin;
            }
            if(payload.data["end"]){
                responseString = responseString +"&end=" + payload.data.end;
            }
            let reponse = await Ajax.get('/api/services/app/LocationRecords/GetLocationRecordByUserAndTime' + responseString);
            return reponse.data.result as LocationRecord;
        }
    };
    mutations = {
        setCurrentPage(state: LocationRecordState, page: number) {
            state.currentPage = page;
        },
        setPageSize(state: LocationRecordState, pagesize: number) {
            state.pageSize = pagesize;
        },
        edit(state: LocationRecordState, locationRecord: LocationRecord) {
            state.editLocationRecord = locationRecord;
        }
    }
}
const locationRecordModule = new LocationRecordModule();
export default locationRecordModule;