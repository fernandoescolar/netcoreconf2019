<template>
    <GChart
      type="ComboChart"
       @ready="onChartReady"
      :options="chartOptions"
    />
</template>

<script lang="ts">
import { Component, Vue, Prop, Watch } from 'types-vue';

@Component
export default class VickyMendozaChart extends Vue {
    @Prop({ required: true})
    public data!: Array<Array<(string | number)>>;

    public chart: any;

    public google: any;

    public chartOptions = {
        legend: 'none',
        height: 300,
        vAxis: {title: 'Value', minValue: -10, maxValue: 10},
        seriesType: 'bars'
    };

    @Watch('data', { immediate: true })
    public onDataChanged(): void {
        if (this.data && this.google && this.chart) {
            this.reloadCharData();
        }
    }

    public onChartReady(chart: any, google: any): void {
        this.google = google;
        this.chart = chart;

        if (this.data && this.google && this.chart) {
            this.reloadCharData();
        }
    }

    private reloadCharData(): void {
        const d = this.data.slice(0);
        const dataTable = new this.google.visualization.DataTable();
        dataTable.addColumn('string', d[0]);
        dataTable.addColumn('number', d[1]);
        dataTable.addColumn({type: 'string', role: 'style'});
        dataTable.addRows(d.slice(1));

        this.chart.draw(dataTable, this.chartOptions);
    }
}
</script>
