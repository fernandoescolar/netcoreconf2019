<template>
    <GChart
      type="ColumnChart"
      :data="data"
      :options="chartOptions"
    />
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'types-vue';

@Component
export default class ColumnsChart extends Vue {
    @Prop({ required: true})
    public data!: Array<Array<(string | number)>>;

    public chartOptions = {
        legend: 'none',
        height: 300
    };
}
</script>