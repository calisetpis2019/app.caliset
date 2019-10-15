import Entity from './entity'

export default class Comment extends Entity<number>{
    commentary: string;
    operationId: number;
}