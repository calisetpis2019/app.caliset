import { Store, Module, ActionContext } from 'vuex'
import ListModule from './list-module'
import ListState from './list-state'
import Location from '../entities/location'
import Ajax from '../../lib/ajax'
import PageResult from '@/store/entities/page-result';
import ListMutations from './list-mutations'
interface LocationState extends ListState<Location> {
    editLocation: Location;
}
class LocationModule extends ListModule<LocationState, any, Location>{
    state = {
        totalCount: 0,
        currentPage: 1,
        pageSize: 10,
        list: new Array<Location>(),
        loading: false,
        editLocation: new Location()
    }
    actions = {
        async getAll(context: ActionContext<LocationState, any>) {
            context.state.loading = true;
            let reponse = await Ajax.get('/api/services/app/Location/GetAll');
            context.state.loading = false;
            context.state.totalCount = reponse.data.result.length;
            context.state.list = reponse.data.result;
        },
        async create(context: ActionContext<LocationState, any>, payload: any) {
            await Ajax.post('/api/services/app/Location/Create', payload.data);
        },
        async update(context: ActionContext<LocationState, any>, payload: any) {
            console.log("update: ");
            console.log(payload.data);
            await Ajax.put('/api/services/app/Location/Update', payload.data);
        },
        async delete(context: ActionContext<LocationState, any>, payload: any) {
            await Ajax.delete('/api/services/app/Location/Delete?Id=' + payload.data.id);
        },
        async get(context: ActionContext<LocationState, any>, payload: any) {
            let reponse = await Ajax.get('/api/services/app/Location/GetLocationById?Id=' + payload.id);
            return reponse.data.result as Location;
        }
    };
    mutations = {
        setCurrentPage(state: LocationState, page: number) {
            state.currentPage = page;
        },
        setPageSize(state: LocationState, pagesize: number) {
            state.pageSize = pagesize;
        },
        edit(state: LocationState, location: Location) {
            state.editLocation = location;
        }
    }
}
const locationModule = new LocationModule();
export default locationModule;