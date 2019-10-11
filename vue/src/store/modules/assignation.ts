import { Store, Module, ActionContext } from 'vuex'
import ListModule from './list-module'
import ListState from './list-state'
import Assignation from '../entities/assignation'
import Ajax from '../../lib/ajax'
import PageResult from '@/store/entities/page-result';
import ListMutations from './list-mutations'
interface AssignationState extends ListState<Assignation> {}
class AssignationModule extends ListModule<AssignationState, any, Assignation>{

    actions = {
        async create(context: ActionContext<AssignationState, any>, payload: any) {
            await Ajax.post('/api/services/app/Assignation/Create', payload.data);
        }
    };
}
const assignationModule = new AssignationModule();
export default assignationModule;