import {Store,Module,ActionContext} from 'vuex'
import ListModule from './list-module'
import ListState from './list-state'
import User from '../entities/user'
import Role from '../entities/role'
import Ajax from '../../lib/ajax'
import PageResult from '@/store/entities/page-result';
import ListMutations from './list-mutations'

interface UserState extends ListState<User>{
    viewUser:User,
    editUser:User,
    roles:Role[]
}
class UserMutations extends ListMutations<User>{

}
class UserModule extends ListModule<UserState,any,User>{
    state={
        totalCount:0,
        currentPage:1,
        pageSize:10,
        list: new Array<User>(),
        loading:false,
        viewUser:new User(),
        editUser:new User(),
        roles:new Array<Role>()
    }
    actions={
        async getAll(context:ActionContext<UserState,any>,payload:any){
            context.state.loading=true;
            let reponse=await Ajax.get('/api/services/app/User/GetAll',{params:payload.data});
            context.state.loading=false;
            
            for (let i = 0; i < reponse.data.result.items.length; i++) {
                if(reponse.data.result.items[i].lastLoginTime=="0001-01-01T00:00:00"){
                    reponse.data.result.items[i].lastLoginTime="Nunca";
                }
            }

            let page=reponse.data.result as PageResult<User>;
            context.state.totalCount=page.totalCount;
            context.state.list=page.items;
        },
        async getAllUsers(context:ActionContext<UserState,any>,payload:any){
            context.state.loading=true;
            let reponse=await Ajax.get('/api/services/app/User/GetAllUsers',{});
            context.state.loading=false;
            context.state.list = reponse.data.result;
        },
        async getEligibleUsers(context:ActionContext<UserState,any>,payload:any){
            context.state.loading=true;
            let reponse=await Ajax.get('/api/services/app/UserDeviceToken/GetAllUserElegible',{});
            context.state.loading=false;
            context.state.list = reponse.data.result;
        },
        async create(context: ActionContext<UserState, any>, payload: any) {
            payload.data.isActive=true;

            payload.data.roleNames = payload.data.roleNames.filter(function(value, index, arr){
                return value != null;
            });

            await Ajax.post('/api/services/app/User/Create',payload.data);
        },
        async update(context:ActionContext<UserState,any>,payload:any){
            payload.data.roleNames=null;
            payload.data.lastLoginTime=null;
            await Ajax.put('/api/services/app/User/Update',payload.data);
        },
        async delete(context:ActionContext<UserState,any>,payload:any){
            await Ajax.delete('/api/services/app/User/Delete?Id='+payload.data.id);
        },
        async get(context:ActionContext<UserState,any>,payload:any){
            let reponse=await Ajax.get('/api/services/app/User/Get?Id='+payload.id);
            return reponse.data.result as User;
        },
        async getRoles(context:ActionContext<UserState,any>){
            let reponse=await Ajax.get('/api/services/app/User/GetRoles');
            context.state.roles=reponse.data.result.items as Role[];
        },
        async changeLanguage(context:ActionContext<UserState,any>,payload:any){
            await Ajax.post('/api/services/app/User/ChangeLanguage',payload.data);
        }
    };
    mutations={
        setCurrentPage(state:UserState,page:number){
            state.currentPage=page;
        },
        setPageSize(state:UserState,pagesize:number){
            state.pageSize=pagesize;
        },
        edit(state:UserState,user:User){
            state.editUser=user;
        },
        view(state:UserState,user:User){
            state.viewUser=user;
        }
    }
}
const userModule=new UserModule();
export default userModule;