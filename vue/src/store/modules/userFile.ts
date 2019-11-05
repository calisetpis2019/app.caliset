import {Store,Module,ActionContext} from 'vuex'
import ListModule from './list-module'
import ListState from './list-state'
import UserFile from '../entities/userFile'
import Role from '../entities/role'
import Ajax from '../../lib/ajax'
import PageResult from '@/store/entities/page-result';
import ListMutations from './list-mutations'
import update from 'immutability-helper';

interface UserFileState extends ListState<UserFile>{
    viewUserFile:UserFile,
    editUserFile:UserFile,
    roles:Role[]
}
class UserFileMutations extends ListMutations<UserFile>{

}
class UserFileModule extends ListModule<UserFileState,any,UserFile>{
    state={
        totalCount:0,
        currentPage:1,
        pageSize:10,
        list: new Array<UserFile>(),
        loading:false,
        viewUserFile:new UserFile(),
        editUserFile:new UserFile(),
        roles:new Array<Role>()
    }
    actions={
        async getAllByUser(context:ActionContext<UserFileState,any>,payload:any){
            context.state.loading=true;
            let reponse=await Ajax.get('/api/services/app/UserFile/GetAllByUser?Id=' + payload.data.id,{});
            context.state.loading=false;
            context.state.list = reponse.data.result;
        },
        async create(context: ActionContext<UserFileState, any>, payload: any) {
            payload.data.isActive=true;

            var newRole=payload.data.roleNames;
            var newPayload=update(payload.data,{roleNames:{$set:[newRole]}});

            await Ajax.post('/api/services/app/UserFile/Create',newPayload);
        },
        async delete(context:ActionContext<UserFileState,any>,payload:any){
            await Ajax.delete('/api/services/app/UserFile/Delete?Id='+payload.data.id);
        },
        async get(context:ActionContext<UserFileState,any>,payload:any){
            let reponse=await Ajax.get('/api/services/app/UserFile/GetUserFileById?Id='+payload.data.id);
            return reponse.data.result as UserFile;
        }
    };
    mutations={
        setCurrentPage(state:UserFileState,page:number){
            state.currentPage=page;
        },
        setPageSize(state:UserFileState,pagesize:number){
            state.pageSize=pagesize;
        },
        edit(state:UserFileState,userFile:UserFile){
            state.editUserFile=userFile;
        },
        view(state:UserFileState,userFile:UserFile){
            state.viewUserFile=userFile;
        }
    }
}
const userFileModule=new UserFileModule();
export default userFileModule;