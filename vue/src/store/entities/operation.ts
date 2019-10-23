import Entity from './entity'

export default class Operation extends Entity<number>{
    operationTypeId: number;
    nominatorId: number;
    chargerId: number;
    locationId: number;
    date: Date;
    managerId: number;
    commodity: string;
    package: string;
    shipName: string;
    destiny: string;
    clientReference: string;
    line: string;
    bookingNumber: string;
    notes: string;
    operationStateId:number;
}