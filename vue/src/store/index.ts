import Vue from 'vue';
import Vuex from 'vuex';
Vue.use(Vuex);
import app from './modules/app'
import session from './modules/session'
import account from './modules/account'
import user from './modules/user'
import role from './modules/role'
import tenant from './modules/tenant'
import operation from './modules/operation'
import operationType from './modules/operationType'
import operationState from './modules/operationState'
import location from './modules/location'
import client from './modules/client'
import assignation from './modules/assignation'
import comment from './modules/comment'
import changepasswordentity from './modules/change-password'
import hoursRecord from './modules/hoursRecord'
import locationRecord from './modules/locationRecord'
import userfile from './modules/user-file'
import formulary from './modules/formulary'

const store = new Vuex.Store({
    state: {
        //
    },
    mutations: {
        //
    },
    actions: {

    },
    modules: {
        app,
        session,
        account,
        user,
        role,
        tenant,
        operation,
        operationType,
        operationState,
        location,
        client,
        assignation,
        comment,
        changepasswordentity,
        hoursRecord,
        locationRecord,
        userfile,
        formulary
    }
});

export default store;