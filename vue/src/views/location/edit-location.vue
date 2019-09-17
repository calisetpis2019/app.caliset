<template>
    <div>
        <Modal :title="L('EditLocation')"
               :value="value"
               @on-ok="save"
               @on-visible-change="visibleChange">
            <Form ref="locationForm" label-position="top" :rules="locationRule" :model="location">
                <Tabs value="detail">
                    <TabPane :label="L('Details')" name="detail">
                        <FormItem :label="L('Name')" prop="name">
                            <Input v-model="location.name" :maxlength="32"></Input>
                        </FormItem>
                        <FormItem :label="L('Latitude')" prop="latitude">
                            <Input v-model="location.latitude" type="number"></Input>
                        </FormItem>
                        <FormItem :label="L('Longitude')" prop="longitude">
                            <Input v-model="location.longitude" type="number"></Input>
                        </FormItem>
                        <FormItem :label="L('Radius')" prop="radius">
                            <Input v-model="location.radius" type="number"></Input>
                        </FormItem>
                    </TabPane>
                </Tabs>
            </Form>
            <div slot="footer">
                <Button @click="cancel">{{L('Cancel')}}</Button>
                <Button @click="save" type="primary">{{L('OK')}}</Button>
            </div>
        </Modal>
    </div>
</template>

<script lang="ts">
    import { Component, Vue,Inject, Prop,Watch } from 'vue-property-decorator';
    import Util from '@/lib/util'
    import AbpBase from '@/lib/abpbase'
    import Location from '@/store/entities/location'
    @Component
    export default class EditLocation extends AbpBase{
        @Prop({type:Boolean,default:false}) value:boolean;
        location:Location=new Location();
        created(){

        }
        save() {
            (this.$refs.locationForm as any).validate(async (valid:boolean)=>{
                if (valid) {

                    await this.$store.dispatch({
                        type:'location/update',
                        data:this.location
                    });
                    (this.$refs.locationForm as any).resetFields();
                    this.$emit('save-success');
                    this.$emit('input',false);
                }
            })
        }
        cancel(){
            (this.$refs.locationForm as any).resetFields();
            this.$emit('input',false);
        }
        visibleChange(value:boolean){
            if(!value){
                this.$emit('input',value);
            } else {
                this.location = Util.extend(true, {}, this.$store.state.location.editLocation);
            }
        }

        locationRule={
            name:[{required: true,message:this.L('FieldIsRequired',undefined,this.L('Name')),trigger: 'blur'}]
        }
    }
</script>
