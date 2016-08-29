import { Component, ViewChild, ElementRef, OnInit, AfterViewInit } from "@angular/core";
import { Router } from "@angular/router";
import { Title } from "@angular/platform-browser";

import { CategoryViewModel } from "./category-view-model";
import { CategoryService } from "./category.service";
import { UserService } from "./user.service";
import { SpendingIncomeViewModel } from "./spending-income-view-model";
import { ChartService } from "./chart.service";
import { ColumnData, PieData } from "./chart-view-model";

@Component({
    selector: "my-spending",
    templateUrl: "app/spending.component.html"
})
export class SpendingComponent implements OnInit, AfterViewInit {
    errorMessage: string;
    people: string[];
    categories: string[];
    spendingModel = new SpendingIncomeViewModel("Aleks Angelov", new Date(), new Date(), "Food");
    categoryModel = new CategoryViewModel("", true);
    active = true;

    @ViewChild("selectPersonName")
    selectPersonName: ElementRef;
    @ViewChild("inputDateFrom")
    inputDateFrom: ElementRef;
    @ViewChild("inputDateUntil")
    inputDateUntil: ElementRef;
    @ViewChild("selectCategoryName")
    selectCategoryName: ElementRef;

    topLeftChart: HighchartsChartObject;
    topRightChart: HighchartsChartObject;
    bottomLeftChart: HighchartsChartObject;
    bottomRightChart: HighchartsChartObject;

    constructor(
        private titleService: Title,
        private router: Router,
        private categoryService: CategoryService,
        private userService: UserService,
        private chartService: ChartService,
        private elementRef: ElementRef) {
    }

    ngOnInit() {
        this.titleService.setTitle("Spending - Budgeteer");
        this.getFormData();

        this.spendingModel.dateFrom.setMonth(this.spendingModel.dateFrom.getMonth() - 6);
        this.createCharts();
    }

    ngAfterViewInit() {
        $(this.selectPersonName.nativeElement)
            .change(() => this.updateLeftCharts());

        $(this.inputDateFrom.nativeElement)
            .change(() => this.updateAllCharts());

        $(this.inputDateUntil.nativeElement)
            .change(() => this.updateAllCharts());

        $(this.selectCategoryName.nativeElement)
            .change(() => this.updateRightCharts());

        const scr = document.createElement("script");
        scr.type = "text/javascript";
        scr.src = "./default-dates.js";
        this.elementRef.nativeElement.appendChild(scr);
    }

    getFormData() {
        this.userService.getUsers()
            .subscribe(
                response => this.people = response,
                error => this.errorMessage = (error as any));

        this.categoryService.getCategories(true)
            .subscribe(
                response => this.categories = response,
                error => this.errorMessage = (error as any));
    }

    postCategory(cvm: CategoryViewModel) {
        this.categoryService.postCategory(cvm)
            .subscribe(
                () => {
                    if (!this.findCategoryName(cvm.name)) {
                        this.categories.unshift(cvm.name);
                    }
                    this.newCategory();
                },
                error => this.errorMessage = (error as any));
    }

    findCategoryName(newCatName: string) {
        newCatName = newCatName.toLowerCase();
        for (let catName of this.categories) {
            if (newCatName === catName.toLowerCase()) {
                return true;
            }
        }
        return false;
    }

    newCategory() {
        this.categoryModel = new CategoryViewModel("", true);
        this.active = false;
        setTimeout(() => this.active = true, 0);
    }

    createCharts() {
        this.chartService.getColumnChartData("SpendingTopLeftChart",
                this.spendingModel.dateFrom,
                this.spendingModel.dateUntil,
                this.spendingModel.personName,
                null)
            .subscribe(
                data => {
                    this.topLeftChart = new Highcharts.Chart({
                        chart: {
                            type: "column",
                            renderTo: "spendingTopLeftChart"
                        },
                        title: data.title,
                        xAxis: {
                            categories: data.xAxisCategories,
                            crosshair: true
                        },
                        yAxis: {
                            min: 0,
                            title: {
                                text: "Total (Bulgarian lev)"
                            }
                        },
                        tooltip: {
                            pointFormat: "{series.name}: <b>BGN {point.y:.2f}</b>"
                        },
                        plotOptions: {
                            column: {
                                pointPadding: 0.2,
                                borderWidth: 0
                            }
                        },
                        series: data.series
                    });
                },
                error => this.errorMessage = (error as any));

        this.chartService.getColumnChartData("SpendingTopRightChart",
                this.spendingModel.dateFrom,
                this.spendingModel.dateUntil,
                null,
                this.spendingModel.categoryName)
            .subscribe(
                data => {
                    this.topRightChart = new Highcharts.Chart({
                        chart: {
                            type: "column",
                            renderTo: "spendingTopRightChart"
                        },
                        title: data.title,
                        xAxis: {
                            categories: data.xAxisCategories,
                            crosshair: true
                        },
                        yAxis: {
                            min: 0,
                            title: {
                                text: "Total (Bulgarian lev)"
                            }
                        },
                        tooltip: {
                            pointFormat: "{series.name}: <b>BGN {point.y:.2f}</b>"
                        },
                        plotOptions: {
                            column: {
                                pointPadding: 0.2,
                                borderWidth: 0
                            }
                        },
                        series: data.series
                    });
                },
                error => this.errorMessage = (error as any));

        this.chartService.getPieChartData("SpendingBottomLeftChart",
                this.spendingModel.dateFrom,
                this.spendingModel.dateUntil,
                this.spendingModel.personName,
                null)
            .subscribe(
                data => {
                    this.bottomLeftChart = new Highcharts.Chart({
                        chart: {
                            type: "pie",
                            renderTo: "spendingBottomLeftChart"
                        },
                        title: data.title,
                        tooltip: {
                            headerFormat: "",
                            pointFormat: "{point.name}: <b>BGN {point.y:.2f}</b> ({point.percentage:.1f}%)"
                        },
                        plotOptions: {
                            pie: {
                                allowPointSelect: true,
                                cursor: "pointer",
                                dataLabels: {
                                    enabled: false
                                },
                                showInLegend: true
                            }
                        },
                        series: [
                            {
                                name: data.series.name,
                                data: data.series.data
                            }
                        ]
                    });
                },
                error => this.errorMessage = (error as any));

        this.chartService.getPieChartData("SpendingBottomRightChart",
                this.spendingModel.dateFrom,
                this.spendingModel.dateUntil,
                null,
                this.spendingModel.categoryName)
            .subscribe(
                data => {
                    this.bottomRightChart = new Highcharts.Chart({
                        chart: {
                            type: "pie",
                            renderTo: "spendingBottomRightChart"
                        },
                        title: data.title,
                        tooltip: {
                            headerFormat: "",
                            pointFormat: "{point.name}: <b>BGN {point.y:.2f}</b> ({point.percentage:.1f}%)"
                        },
                        plotOptions: {
                            pie: {
                                allowPointSelect: true,
                                cursor: "pointer",
                                dataLabels: {
                                    enabled: false
                                },
                                showInLegend: true
                            }
                        },
                        series: [
                            {
                                name: data.series.name,
                                data: data.series.data
                            }
                        ]
                    });
                },
                error => this.errorMessage = (error as any));
    }

    updateAllCharts() {
        this.updateLeftCharts();
        this.updateRightCharts();
    }

    updateLeftCharts() {
        this.chartService.getColumnChartData("SpendingTopLeftChart",
                this.spendingModel.dateFrom,
                this.spendingModel.dateUntil,
                this.spendingModel.personName,
                null)
            .subscribe(
                data => {
                    this.topLeftChart.setTitle(data.title);
                    this.topLeftChart.series[0].setData(data.series[0].data);
                },
                error => this.errorMessage = (error as any));

        this.chartService.getPieChartData("SpendingBottomLeftChart",
                this.spendingModel.dateFrom,
                this.spendingModel.dateUntil,
                this.spendingModel.personName,
                null)
            .subscribe(
                data => {
                    this.bottomLeftChart.setTitle(data.title);
                    this.bottomLeftChart.series[0].setData(data.series.data);
                },
                error => this.errorMessage = (error as any));
    }

    updateRightCharts() {
        this.chartService.getColumnChartData("SpendingTopRightChart",
                this.spendingModel.dateFrom,
                this.spendingModel.dateUntil,
                null,
                this.spendingModel.categoryName)
            .subscribe(
                data => {
                    this.topRightChart.setTitle(data.title);
                    this.topRightChart.series[0].setData(data.series[0].data);
                },
                error => this.errorMessage = (error as any));

        this.chartService.getPieChartData("SpendingBottomRightChart",
                this.spendingModel.dateFrom,
                this.spendingModel.dateUntil,
                null,
                this.spendingModel.categoryName)
            .subscribe(
                data => {
                    this.bottomRightChart.setTitle(data.title);
                    this.bottomRightChart.series[0].setData(data.series.data);
                },
                error => this.errorMessage = (error as any));
    }
}