import { Store, Module, ActionContext } from 'vuex'
import ListModule from './list-module'
import ListState from './list-state'
import Assignation from '../entities/assignation'
import Ajax from '../../lib/ajax'
import PageResult from '@/store/entities/page-result';
import ListMutations from './list-mutations'
import User from '../entities/user'

interface AssignationState extends ListState<Assignation> {
	usersAssignedToOperation: Array<User>;
    assignmentsOfOperation: Array<Assignation>;
    assignmentsByUsers: Array<Assignation>;
}
class AssignationModule extends ListModule<AssignationState, any, Assignation>{
	state = {
        totalCount: 0,
        currentPage: 1,
        pageSize: 10,
        list: new Array<Assignation>(),
        loading: false,
        usersAssignedToOperation: new Array<User>(),
        assignmentsOfOperation: new Array<Assignation>(),
        assignmentsByUsers: new Array<Assignation>()
    }
    actions = {
        async create(context: ActionContext<AssignationState, any>, payload: any) {
            await Ajax.post('/api/services/app/Assignation/Create', payload.data);
        },
        async getUsersByOperation(context: ActionContext<AssignationState, any>, payload: any) {
            context.state.loading = true;
            let reponse = await Ajax.get('/api/services/app/Assignation/GetUsersByOperation?operationId=' + payload.data.id);
            context.state.loading = false;
            context.state.totalCount = reponse.data.result.length;
            context.state.usersAssignedToOperation = reponse.data.result;
        },
        async getAssignmentsByOperation(context: ActionContext<AssignationState, any>, payload: any) {
            context.state.loading = true;
            let reponse = await Ajax.get('/api/services/app/Assignation/GetAssignmentsByOperation?operationId=' + payload.data.id);
            context.state.loading = false;
            context.state.totalCount = reponse.data.result.length;
            context.state.assignmentsOfOperation = reponse.data.result;
        },

        async getAssignationsByUser(context: ActionContext<AssignationState, any>, payload: any){
            context.state.loading = true;
            let response = await Ajax.get('/api/services/app/Assignation/GetAssignmentsByUser?userId=' + payload.data.id);
            context.state.loading = false;
            context.state.assignmentsByUsers = response.data.result;  
        }
        


    };
}
const assignationModule = new AssignationModule();
export default assignationModule;