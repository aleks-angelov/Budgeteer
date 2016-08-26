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
    selector: "my-income",
    templateUrl: "app/income.component.html"
})
export class IncomeComponent implements OnInit, AfterViewInit {
    errorMessage: string;
    people: string[];
    categories: string[];
    incomeModel = new SpendingIncomeViewModel("Aleks Angelov", new Date(), new Date(), "Salary");
    categoryModel = new CategoryViewModel("", false);
    active = true;

    @ViewChild("selectPersonName") selectPersonName: ElementRef;
    @ViewChild("inputDateFrom") inputDateFrom: ElementRef;
    @ViewChild("inputDateUntil") inputDateUntil: ElementRef;
    @ViewChild("selectCategoryName") selectCategoryName: ElementRef;

    topLeftChart: HighchartsChartObject;
    topRightChart: HighchartsChartObject;
    bottomLeftChart: HighchartsChartObject;
    bottomRightChart: HighchartsChartObject;

    constructor(
        private titleService: Title,
        private router: Router,
        private categoryService: CategoryService,
        private userService: UserService,
        private chartService: ChartService) {
    }

    ngOnInit() {
        this.titleService.setTitle("Income - Budgeteer");
        this.getFormData();

        this.incomeModel.dateFrom.setMonth(this.incomeModel.dateFrom.getMonth() - 6);
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
    }

    getFormData() {
        this.userService.getUsers()
            .subscribe(
            response => this.people = response,
            error => this.errorMessage = (error as any));

        this.categoryService.getCategories(false)
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
        this.categoryModel = new CategoryViewModel("", false);
        this.active = false;
        setTimeout(() => this.active = true, 0);
    }

    createCharts() {
        this.chartService.getColumnChartData("IncomeTopLeftChart",
            this.incomeModel.dateFrom,
            this.incomeModel.dateUntil,
            this.incomeModel.personName,
            null)
            .subscribe(
            data => {
                this.topLeftChart = new Highcharts.Chart({
                    chart: {
                        type: "column",
                        renderTo: "incomeTopLeftChart"
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

        this.chartService.getColumnChartData("IncomeTopRightChart",
            this.incomeModel.dateFrom,
            this.incomeModel.dateUntil,
            null,
            this.incomeModel.categoryName)
            .subscribe(
            data => {
                this.topRightChart = new Highcharts.Chart({
                    chart: {
                        type: "column",
                        renderTo: "incomeTopRightChart"
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

        this.chartService.getPieChartData("IncomeBottomLeftChart",
            this.incomeModel.dateFrom,
            this.incomeModel.dateUntil,
            this.incomeModel.personName,
            null)
            .subscribe(
            data => {
                this.bottomLeftChart = new Highcharts.Chart({
                    chart: {
                        type: "pie",
                        renderTo: "incomeBottomLeftChart"
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

        this.chartService.getPieChartData("IncomeBottomRightChart",
            this.incomeModel.dateFrom,
            this.incomeModel.dateUntil,
            null,
            this.incomeModel.categoryName)
            .subscribe(
            data => {
                this.bottomRightChart = new Highcharts.Chart({
                    chart: {
                        type: "pie",
                        renderTo: "incomeBottomRightChart"
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
        this.chartService.getColumnChartData("IncomeTopLeftChart",
            this.incomeModel.dateFrom,
            this.incomeModel.dateUntil,
            this.incomeModel.personName,
            null)
            .subscribe(
            data => {
                this.topLeftChart.setTitle(data.title);
                this.topLeftChart.series[0].setData(data.series[0].data);
            },
            error => this.errorMessage = (error as any));

        this.chartService.getPieChartData("IncomeBottomLeftChart",
            this.incomeModel.dateFrom,
            this.incomeModel.dateUntil,
            this.incomeModel.personName,
            null)
            .subscribe(
            data => {
                this.bottomLeftChart.setTitle(data.title);
                this.bottomLeftChart.series[0].setData(data.series.data);
            },
            error => this.errorMessage = (error as any));
    }

    updateRightCharts() {
        this.chartService.getColumnChartData("IncomeTopRightChart",
            this.incomeModel.dateFrom,
            this.incomeModel.dateUntil,
            null,
            this.incomeModel.categoryName)
            .subscribe(
            data => {
                this.topRightChart.setTitle(data.title);
                this.topRightChart.series[0].setData(data.series[0].data);
            },
            error => this.errorMessage = (error as any));

        this.chartService.getPieChartData("IncomeBottomRightChart",
            this.incomeModel.dateFrom,
            this.incomeModel.dateUntil,
            null,
            this.incomeModel.categoryName)
            .subscribe(
            data => {
                this.bottomRightChart.setTitle(data.title);
                this.bottomRightChart.series[0].setData(data.series.data);
            },
            error => this.errorMessage = (error as any));
    }
}