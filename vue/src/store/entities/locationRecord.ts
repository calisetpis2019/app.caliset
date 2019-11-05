import Entity from './entity'

export default class LocationRecord extends Entity<number>{
    latitude: number;
    longitude: number;
    when: Date;
}