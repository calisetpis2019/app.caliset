import Entity from './entity'

export default class HoursRecord extends Entity<number>{
    startDate: Date;
    endDate: Date;
    operationId: number;
}