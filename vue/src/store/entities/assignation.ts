import Entity from './entity'

export default class Assignation extends Entity<number>{
    inspectorId:number;
    operationId:number;
    date: Date;
    dateFin: Date;
}