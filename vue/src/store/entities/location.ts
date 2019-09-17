import Entity from './entity'

export default class Location extends Entity<number>{
    name: string;
    latitude: number;
    longitude: number;
    radius: number;
}