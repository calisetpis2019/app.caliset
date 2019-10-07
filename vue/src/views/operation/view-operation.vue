<template>
    <div>
        <Modal :title="L('Ver Operación')"
               :value="value"
               @on-ok="save"
               @on-visible-change="visibleChange">
            <Divider> Tipo de operación </Divider>
            <Span>{{ this.operation.operationType }}</Span>
            <Divider> Nominador </Divider>
            <Span>{{ this.operation.nominatorName }}</Span>
            <Divider> Cargador </Divider>
            <Span>{{ this.operation.chargerName }}</Span>
            <Divider> Lugar </Divider>
            <Span>{{ this.location.name }}</Span>
            <Divider> Fecha y hora de inicio </Divider>
            <Span>{{ this.operation.date }}</Span>
            <Divider> Responsable de la operación </Divider>
            <Span>{{ this.operation.managerName }}</Span>
            <Divider> Mercaderia </Divider>
            <Span>{{ this.operation.commodity }}</Span>
            <Divider> Empaque </Divider>
            <Span>{{ this.operation.package }}</Span>
            <Divider> Nombre del barco </Divider>
            <Span>{{ this.operation.shipName }}</Span>
            <Divider> Destino </Divider>
            <Span>{{ this.operation.destination }}</Span>
            <Divider> Referencia del cliente </Divider>
            <Span>{{ this.operation.clientReference }}</Span>
            <Divider> Línea </Divider>
            <Span>{{ this.operation.line }}</Span>
            <Divider> Número de Booking</Divider>
            <Span>{{ this.operation.bookingNumber }}</Span>
            <Divider> Notas </Divider>
            <Span>{{ this.operation.notes }}</Span>
        </Modal>
    </div>
</template>

<script lang="ts">
    import { Component, Vue,Inject, Prop,Watch } from 'vue-property-decorator';
    import Util from '@/lib/util'
    import AbpBase from '@/lib/abpbase'
    import UserRequest from '@/store/entities/user-request'
    import Operation from '@/store/entities/operation'
    import Client from '@/store/entities/client'
    import User from '@/store/entities/user'
    import Location from '@/store/entities/location'
    import OperationState from '@/store/entities/operationState'

    class PageViewOperationRequest extends UserRequest {
    }

    @Component
    export default class ViewOperation extends AbpBase{
        @Prop({type:Boolean,default:false}) value:boolean;
        operation:Operation=new Operation();

        pagerequest: PageViewOperationRequest = new PageViewOperationRequest();
        nominatorRequest: PageViewOperationRequest = new PageViewOperationRequest();
        chargerRequest: PageViewOperationRequest = new PageViewOperationRequest();
        managerRequest: PageViewOperationRequest = new PageViewOperationRequest();
        locationRequest: PageViewOperationRequest = new PageViewOperationRequest();
        
        response:Operation = new Operation();
        nominator:Client = new Client();
        charger:Client = new Client();
        manager:User = new User();
        location:Location = new Location();

        visibleChange(value:boolean){
            if(!value){
                this.$emit('input',value);
            } else {
                this.operation = Util.extend(true, {}, this.$store.state.operation.viewOperation);
            }

            this.locationRequest["id"] = this.operation.locationId;
            this.getLocation();


        }


        async getLocation() {
            console.log(this.locationRequest);
            this.location = await this.$store.dispatch({
                type: 'location/get',
                data: this.locationRequest
            })
        }

    }
</script>
