import { Component, ElementRef, OnInit, AfterViewInit } from "@angular/core";
import { Router } from "@angular/router";
import { Title } from "@angular/platform-browser";

import { TransactionViewModel } from "./transaction-view-model";
import { TransactionService } from "./transaction.service";
import { CategoryService } from "./category.service";
import { UserService } from "./user.service";
import { ChartService } from "./chart.service";
import { ColumnData, PieData } from "./chart-view-model";

@Component({
    selector: "my-overview",
    templateUrl: "app/overview.component.html"
})
export class OverviewComponent implements OnInit, AfterViewInit {
    errorMessage: string;
    transactions: TransactionViewModel[];
    pageNumbers: string[];
    currentPage = 1;
    people: string[];
    categories: string[];
    overviewModel = new TransactionViewModel(new Date(), 0, "Aleks Angelov", "Food", true, "");
    active = true;

    dateFrom = new Date();
    dateUntil = new Date();
    leftChart: HighchartsChartObject;
    rightChart: HighchartsChartObject;

    constructor(
        private titleService: Title,
        private router: Router,
        private transactionService: TransactionService,
        private categoryService: CategoryService,
        private userService: UserService,
        private chartService: ChartService,
        private elementRef: ElementRef) {
    }

    ngOnInit() {
        this.titleService.setTitle("Overview - Budgeteer");
        this.getFormData();
        this.getTotalPages();
        this.getTransactions(1);

        this.dateFrom.setMonth(this.dateFrom.getMonth() - 6);
        this.createCharts();
    }

    ngAfterViewInit() {
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

    getTotalPages() {
        this.transactionService.getTotalPages()
            .subscribe(
            response => this.pageNumbers = response,
            error => this.errorMessage = (error as any));
    }

    isCurrentPage(n: number): boolean {
        return this.currentPage === n;
    }

    getTransactions(page: number) {
        this.currentPage = page;
        this.transactionService.getTransactions(page)
            .subscribe(
            response => this.transactions = response,
            error => this.errorMessage = (error as any));
    }

    postTransaction(tvm: TransactionViewModel) {
        this.transactionService.postTransaction(tvm)
            .subscribe(
            () => {
                this.getTransactions(1);
                this.newTransaction();
                this.updateCharts();
            },
            error => this.errorMessage = (error as any));
    }

    setTransactionType(isDebit: boolean) {
        this.overviewModel.isDebit = isDebit;

        this.categoryService.getCategories(isDebit)
            .subscribe(
            response => this.categories = response,
            error => this.errorMessage = (error as any));

        this.overviewModel.categoryName = isDebit ? "Food" : "Bonus";
    }

    newTransaction() {
        this.overviewModel = new TransactionViewModel(this.overviewModel.date,
            0,
            this.overviewModel.personName,
            "Food",
            true,
            "");
        this.active = false;
        setTimeout(() => this.active = true, 0);
    }

    createCharts() {
        this.chartService.getColumnChartData("OverviewLeftChart", this.dateFrom, this.dateUntil)
            .subscribe(
            data => {
                this.leftChart = new Highcharts.Chart({
                    chart: {
                        type: "column",
                        renderTo: "overviewLeftChart"
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

        this.chartService.getPieChartData("OverviewRightChart", this.dateFrom, this.dateUntil)
            .subscribe(
            data => {
                this.rightChart = new Highcharts.Chart({
                    chart: {
                        type: "pie",
                        renderTo: "overviewRightChart"
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

    updateCharts() {
        this.chartService.getColumnChartData("OverviewLeftChart", this.dateFrom, this.dateUntil)
            .subscribe(
            data => {
                this.leftChart.series[0].setData(data.series[0].data);
                this.leftChart.series[1].setData(data.series[1].data);
            },
            error => this.errorMessage = (error as any));

        this.chartService.getPieChartData("OverviewRightChart", this.dateFrom, this.dateUntil)
            .subscribe(
            data => this.rightChart.series[0].setData(data.series.data),
            error => this.errorMessage = (error as any));
    }
}