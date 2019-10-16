import Entity from './entity'

export default class ChangePasswordEntity extends Entity<number>{
    currentPassword:string;
    newPassword:number;
}