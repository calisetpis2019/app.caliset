import Entity from './entity'
export default class User extends Entity<number>{
    password: string;
    userName: string;
    emailAddress: string;
}