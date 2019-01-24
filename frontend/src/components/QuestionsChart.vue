<template>
    <GChart
      type="ScatterChart"
      :options="chartOptions"
      @ready="onChartReady"
    />
</template>

<script lang="ts">
import { Component, Vue, Prop, Watch } from 'types-vue';

@Component
export default class QuestionsChart extends Vue {
    @Prop({ required: true})
    public data!: Array<Array<(string | number)>>;

    public chart: any;

    public google: any;

    public chartOptions = {
        legend: 'none',
        height: 500,
        hAxis: {title: 'Crazy', minValue: 0, maxValue: 10},
        vAxis: {title: 'Usefull', minValue: 0, maxValue: 10}
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
        dataTable.addColumn('number', 'Usefull');
        dataTable.addColumn('number', 'Crazy');
        dataTable.addColumn({type: 'string', role: 'tooltip'});
        dataTable.addRows(d.slice(1));

        this.chart.draw(dataTable, this.chartOptions);
    }
}
</script>

<style>
#google-visualization-errors-all-1 {
    display: none !important;
}
</style>