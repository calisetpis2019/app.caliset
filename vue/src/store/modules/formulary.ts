import { Store, Module, ActionContext } from 'vuex'
import ListModule from './list-module'
import ListState from './list-state'
import Formulary from '../entities/formulary'
import Ajax from '../../lib/ajax'
import PageResult from '@/store/entities/page-result';
import ListMutations from './list-mutations'
interface FormularyState extends ListState<Formulary> {
    formularyId: number;
    retrievedForm:Formulary;
    loadingAvailableForms: boolean;
    listAvailableForms:Array<Formulary>;
    loadingAssignedForms:boolean;
    listAssignedForms:Array<Formulary>;
}
class FormularyModule extends ListModule<FormularyState, any, Formulary>{
    state = {
        totalCount: 0,
        currentPage: 1,
        pageSize: 10,
        list: new Array<Formulary>(),
        loading: false,
        formularyId: 0,
        retrievedForm: new Formulary,
        loadingAvailableForms: false,
        listAvailableForms: new Array<Formulary>(),
        loadingAssignedForms: false,
        listAssignedForms: new Array<Formulary>(),
    }
    actions = {
        async getAll(context: ActionContext<FormularyState, any>) {
            context.state.loading = true;
            let reponse = await Ajax.get('/api/services/app/Form/GetAll');
            context.state.list = reponse.data.result;
            context.state.loading = false;
        },
        async create(context: ActionContext<FormularyState, any>, payload: any) {
            await Ajax.post('/api/services/app/Form/Create', payload.data);
        },
        async delete(context: ActionContext<FormularyState, any>, payload: any) {
            context.state.loading = true;
            await Ajax.delete('/api/services/app/Form/Delete?input=' + payload.data);
            context.state.loading = false;
        },
        async get(context: ActionContext<FormularyState, any>, payload: any) {
            let reponse = await Ajax.get('/api/services/app/Form/GetFormById?input=' + payload.data);
            context.state.retrievedForm = reponse.data.result;
        },
        async getAvailable(context: ActionContext<FormularyState, any>, payload: any) {
            context.state.loadingAvailableForms = true;
            let reponse = await Ajax.post('/api/services/app/Form/AntiGetAllFormByOperation?idOperation='+payload.data);
            context.state.listAvailableForms = reponse.data.result;
            context.state.loadingAvailableForms = false;
        },
        async getAssigned(context: ActionContext<FormularyState, any>, payload: any) {
            context.state.loadingAssignedForms = true;
            let reponse = await Ajax.get('/api/services/app/Form/GetAllFormByOperation?idOperation='+payload.data);
            context.state.listAssignedForms = reponse.data.result;
            context.state.loadingAssignedForms = false;
        },
        async associateForm(context: ActionContext<FormularyState, any>, payload: any) {
            context.state.loadingAssignedForms = true;
            context.state.loadingAvailableForms = true;
            let reponse = await Ajax.post('/api/services/app/Form/AddFormToOperation',payload.data);
            context.state.loadingAssignedForms = false;
            context.state.loadingAvailableForms = false;
        },
        async unassociateForm(context: ActionContext<FormularyState, any>, payload: any) {
            context.state.loadingAssignedForms = true;
            context.state.loadingAvailableForms = true;
            let reponse = await Ajax.post('/api/services/app/Form/AntiAddFormToOperation',payload.data);
            context.state.loadingAssignedForms = false;
            context.state.loadingAvailableForms = false;
        }
    };
    mutations = {
        setCurrentPage(state: FormularyState, page: number) {
            state.currentPage = page;
        },
        setPageSize(state: FormularyState, pagesize: number) {
            state.pageSize = pagesize;
        },
        setFormIdAction(state: FormularyState, formId: number) {
            state.formularyId = formId;
        }
    }
}
const operationTypeModule = new FormularyModule();
export default operationTypeModule;