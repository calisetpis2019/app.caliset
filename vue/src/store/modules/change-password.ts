import { Store, Module, ActionContext } from 'vuex'
import ListModule from './list-module'
import ListState from './list-state'
import ChangePasswordEntity from '../entities/change-password'
import Ajax from '../../lib/ajax'
import PageResult from '@/store/entities/page-result';
import ListMutations from './list-mutations'
interface ChangePasswordState extends ListState<ChangePasswordEntity> {}
class ChangePasswordModule extends ListModule<ChangePasswordState, any, ChangePasswordEntity>{
    state = {
        totalCount: 0,
        currentPage: 1,
        pageSize: 10,
        list: new Array<ChangePasswordEntity>(),
        loading: false
    }
    actions = {
        async update(context: ActionContext<ChangePasswordState, any>, payload: any) {
            await Ajax.post('/api/services/app/User/ChangePassword', payload.data);
        }
    }
}
const instanceChangePasswordModule = new ChangePasswordModule();
export default instanceChangePasswordModule;