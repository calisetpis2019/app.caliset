import Entity from './entity'

export default class File extends Entity<number>{
    id: number;
    name: string;
    photo: string; 
}