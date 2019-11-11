declare global {
    interface RouterMeta {
        title: string;
    }
    interface Router {
        path: string;
        name: string;
        icon?: string;
        permission?: string;
        meta?: RouterMeta;
        component: any;
        children?: Array<Router>;
    }
    interface System {
        import(request: string): Promise<any>
    }
    var System: System
}
import login from '../views/login.vue'
import home from '../views/home/home.vue'
import main from '../views/main.vue'

export const locking = {
    path: '/locking',
    name: 'locking',
    component: () => import('../components/lockscreen/components/locking-page.vue')
};
export const loginRouter: Router = {
    path: '/',
    name: 'login',
    meta: {
        title: 'LogIn'
    },
    component: () => import('../views/login.vue')
};
export const otherRouters: Router = {
    path: '/main',
    name: 'main',
    permission: '',
    meta: { title: 'ManageMenu' },
    component: main,
    children: [
        { path: 'home', meta: { title: 'HomePage' }, name: 'home', component: () => import('../views/home/home.vue') }
    ]
}
export const appRouters: Array<Router> = [{
    path: '/setting',
    name: 'setting',
    permission: '',
    meta: { title: 'Configuración' },
    icon: '&#xe68a;',
    component: main,
    children: [
        { path: 'user', permission: 'Pages.Operador', meta: { title: 'Users' }, name: 'user', component: () => import('../views/setting/user/user.vue') },
        { path: 'role', permission: 'Pages.Administrador', meta: { title: 'Roles' }, name: 'role', component: () => import('../views/setting/role/role.vue') },
        { path: 'operationType', permission: 'Pages.Administrador', meta: { title: 'OperationTypes' }, name: 'operationType', component: () => import('../views/operationType/operationType.vue') },
        { path: 'operationState', permission: 'Pages.Administrador', meta: { title: 'OperationStates' }, name: 'operationState', component: () => import('../views/operationState/operationState.vue') },
        { path: 'location', permission: 'Pages.Administrador', meta: { title: 'Locations' }, name: 'location', component: () => import('../views/location/location.vue') },
        { path: 'client', permission: 'Pages.Administrador', meta: { title: 'Clients' }, name: 'client', component: () => import('../views/client/client.vue') },
    ]
},
{
    path: '/operations',
    name: 'operations',
    permission: '',
    meta: { title: 'Operation' },
    icon: '&#xe68a;',
    component: main,
    children: [
        { path: 'operation', permission: 'Pages.Operador', meta: { title: 'Operation' }, name: 'operation', component: () => import('../views/operation/operation.vue') },
	{ path: 'operationEnded', permission: 'Pages.Operador', meta: { title: 'Finalizadas' }, name: 'operationEnded', component: () => import('../views/operation/operation-ended.vue') },
	{ path: 'formulary', permission: 'Pages.Operador', meta: { title: 'Formularios' }, name: 'formulary', component: () => import('../views/formulary/formulary.vue') }
    ]
}]
export const routers = [
    loginRouter,
    locking,
    ...appRouters,
    otherRouters
];
