<template>
    <Modal
        :value="value"
        title="Atención"
        @on-ok="close"
        @on-cancel="close">
        <p>La asignación que acaba de realizar se superpone con otra asignación del inspector</p>
    </Modal>
</template>

<script lang="ts">
    import { Component, Vue,Inject, Prop,Watch } from 'vue-property-decorator';
    import Util from '@/lib/util'
    import AbpBase from '@/lib/abpbase'

    @Component
    export default class OverlapAssignationModal extends AbpBase{
        @Prop({type:Boolean,default:false}) value:boolean;

        close(){
            this.$emit('input',false);
        }
    }
</script>