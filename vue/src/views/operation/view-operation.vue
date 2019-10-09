<template>
    <div>
        <Modal :title="L('Datos de la Operación')"
               :value="value"
               :width="500"
               @on-visible-change="visibleChange">
               <Row>
                   <Col span="12">
                       Tipo de operación: {{ this.operation.operationType }} 
                   </Col>
                   <Col span="12">
                       Nominador: {{ this.operation.nominatorName }} 
                   </Col>
               </Row>
               <Row>
                   <Col span="12">
                       Cargador: {{ this.operation.chargerName }} 
                   </Col>
                   <Col span="12">
                       Lugar: {{ this.location.name }} 
                   </Col>
               </Row>
               <Row>
                   <Col span="12">
                       Fecha y hora de inicio: {{ this.operation.date }} 
                   </Col>
                   <Col span="12">
                       Responsable de la operación: {{ this.operation.managerName }} 
                   </Col>
               </Row>
               <Row>
                   <Col span="12">
                       Mercaderia: {{ this.operation.commodity }} 
                   </Col>
                   <Col span="12">
                       Empaque: {{ this.operation.package }} 
                   </Col>
               </Row>
               <Row>
                   <Col span="12">
                       Nombre del barco: {{ this.operation.shipName }} 
                   </Col>
                   <Col span="12">
                       Destino: {{ this.operation.destination }} 
                   </Col>
               </Row>
               <Row>
                   <Col span="12">
                       Referencia del cliente: {{ this.operation.clientReference }} 
                   </Col>
                   <Col span="12">
                       Línea: {{ this.operation.line }} 
                   </Col>
               </Row>
               <Row>
                   <Col span="12">
                       Número de Booking: {{ this.operation.bookingNumber }} 
                   </Col>
                   <Col span="12">
                       Notas: {{ this.operation.notes }} 
                   </Col>
               </Row>
               <Button type="info" :long="true" v-if="operatorRenderOnly">{{L('Usuarios asignados')}}</Button>
            <div slot="footer">
            </div>
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

        locationRequest: PageViewOperationRequest = new PageViewOperationRequest();
        operatorRenderOnly: boolean = Util.abp.auth.hasPermission('Pages.Operador');

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
            this.location = await this.$store.dispatch({
                type: 'location/get',
                data: this.locationRequest
            })
        }

    }
</script>
