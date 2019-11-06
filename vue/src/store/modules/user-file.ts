import { Store, Module, ActionContext } from 'vuex'
import ListModule from './list-module'
import ListState from './list-state'
import File from '../entities/user-file'
import Ajax from '../../lib/ajax'
import PageResult from '@/store/entities/page-result';
import ListMutations from './list-mutations'

interface FileState extends ListState<File> {
    retrievedFile:File,
}
class FileModule extends ListModule<FileState, any, File>{
    state = {
        totalCount: 0,
        currentPage: 1,
        pageSize: 10,
        list: new Array<File>(),
        loading: false,
        retrievedFile: new File
    }
    actions = {
        async getFileList(context:ActionContext<FileState,any>,payload:any){
            context.state.loading=true;
            let reponse=await Ajax.get('/api/services/app/UserFile/GetAllByUser?idUser='+payload.data);
            context.state.list = reponse.data.result;
            context.state.loading=false;
        },
        async get(context:ActionContext<FileState,any>,payload:any){
            context.state.loading=true;
            let reponse=await Ajax.get('/api/services/app/UserFile/GetUserFileById?input='+payload.data);
            context.state.retrievedFile = reponse.data.result;
            context.state.loading=false;
        },
        async delete(context: ActionContext<FileState, any>, payload: any) {
            await Ajax.delete('/api/services/app/Assignation/Delete?Id=' + payload.data.id);
        }
    };
    mutations = {
        setCurrentPage(state: FileState, page: number) {
            state.currentPage = page;
        },
        setPageSize(state: FileState, pagesize: number) {
            state.pageSize = pagesize;
        },
        setDownloadFile(state:FileState,file:File){
            state.retrievedFile=file;
        }
    }
}
const fileModule = new FileModule();
export default fileModule;