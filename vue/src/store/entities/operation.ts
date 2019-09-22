import Entity from './entity'

export default class Operation extends Entity<number>{
    date: Date;
    commodity: string;
    package: string;
    shipName: string;
    destiny: string;
    line: string;
    bookingNumber: string;
    idLocation: number;
    operationType: number;
    nominador: number;
    cargador: number;
    operationState: number;
}