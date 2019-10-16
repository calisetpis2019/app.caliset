import { Store, Module, ActionContext } from 'vuex'
import ListModule from './list-module'
import ListState from './list-state'
import Client from '../entities/client'
import Ajax from '../../lib/ajax'
import PageResult from '@/store/entities/page-result';
import ListMutations from './list-mutations'
interface ClientState extends ListState<Client> {
    editClient: Client;
}
class ClientModule extends ListModule<ClientState, any, Client>{
    state = {
        totalCount: 0,
        currentPage: 1,
        pageSize: 10,
        list: new Array<Client>(),
        loading: false,
        editClient: new Client()
    }
    actions = {
        async getAll(context: ActionContext<ClientState, any>) {
            context.state.loading = true;
            let reponse = await Ajax.get('/api/services/app/Client/GetAll');
            context.state.loading = false;
            context.state.totalCount = reponse.data.result.length;
            context.state.list = reponse.data.result;
        },
        async create(context: ActionContext<ClientState, any>, payload: any) {
            await Ajax.post('/api/services/app/Client/Create', payload.data);
        },
        async update(context: ActionContext<ClientState, any>, payload: any) {
            await Ajax.put('/api/services/app/Client/Update', payload.data);
        },
        async delete(context: ActionContext<ClientState, any>, payload: any) {
            await Ajax.delete('/api/services/app/Client/Delete?Id=' + payload.data.id);
        },
        async get(context: ActionContext<ClientState, any>, payload: any) {
            let reponse = await Ajax.get('/api/services/app/Client/GetClientById?Id=' + payload.id);
            return reponse.data.result as Client;
        }
    };
    mutations = {
        setCurrentPage(state: ClientState, page: number) {
            state.currentPage = page;
        },
        setPageSize(state: ClientState, pagesize: number) {
            state.pageSize = pagesize;
        },
        edit(state: ClientState, client: Client) {
            state.editClient = client;
        }
    }
}
const clientModule = new ClientModule();
export default clientModule;