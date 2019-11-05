import Entity from './entity'
export default class UserFile extends Entity<number>{
	name: string,
	photo: string,
	userId: number;
}