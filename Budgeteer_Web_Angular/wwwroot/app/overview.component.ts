import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { Title } from "@angular/platform-browser";

import { TransactionViewModel } from "./transaction-view-model";
import { TransactionService } from "./transaction.service";
import { CategoryService } from "./category.service";
import { UserService } from "./user.service";
import { ChartService } from "./chart.service";
import { ChartData, ColumnData, PieData } from "./chart-view-model";

@Component({
    selector: "my-overview",
    templateUrl: "app/overview.component.html"
})
export class OverviewComponent implements OnInit {
    errorMessage: string;
    transactions: TransactionViewModel[];

    people: string[];
    categories: string[];
    model = new TransactionViewModel(null, 0, "Aleks Angelov", "Food", true, "");
    active = true;

    leftChart: HighchartsChartObject;
    rightChart: HighchartsChartObject;

    constructor(
        private titleService: Title,
        private router: Router,
        private transactionService: TransactionService,
        private categoryService: CategoryService,
        private userService: UserService,
        private chartService: ChartService) {
    }

    ngOnInit() {
        this.titleService.setTitle("Overview - Budgeteer");
        this.getFormData();
        this.getTransactions();
        this.updateCharts();
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

    getTransactions() {
        this.transactionService.getTransactions()
            .subscribe(
                response => this.transactions = response,
                error => this.errorMessage = (error as any));
    }

    postTransaction(tvm: TransactionViewModel) {
        this.transactionService.postTransaction(tvm)
            .subscribe(
                () => {
                    this.transactions.unshift(tvm);
                    this.transactions.pop();
                    this.newTransaction();
                },
                error => this.errorMessage = (error as any));
    }

    setTransactionType(isDebit: boolean) {
        this.model.isDebit = isDebit;

        this.categoryService.getCategories(isDebit)
            .subscribe(
                response => this.categories = response,
                error => this.errorMessage = (error as any));

        this.model.categoryName = isDebit ? "Food" : "Salary";
    }

    newTransaction() {
        this.model = new TransactionViewModel(this.model.date, 0, this.model.personName, "Food", true, "");
        this.active = false;
        setTimeout(() => this.active = true, 0);
    }

    updateCharts() {
        const dateFrom = new Date();
        dateFrom.setMonth(dateFrom.getMonth() - 6);
        const dateUntil = new Date();

        let leftChartData: ColumnData;
        this.chartService.getColumnChartData("OverviewLeftChart", dateFrom, dateUntil).subscribe(
            data => leftChartData = data,
            error => this.errorMessage = (error as any));

        const colCats: string[] = [
            "Jan",
            "Feb",
            "Mar",
            "Apr",
            "May",
            "Jun",
            "Jul",
            "Aug",
            "Sep",
            "Oct",
            "Nov",
            "Dec"
        ];

        const colDat: number[] = [49.8, 71.5, 106.4, 129.2, 144.0, 176.0, 135.6, 148.5, 216.4, 194.1, 95.6, 54.4];

        this.leftChart = new Highcharts.Chart({
            chart: {
                type: "column",
                renderTo: "overviewLeftChart"
            },
            title: {
                text: "Monthly Average Rainfall"
            },
            xAxis: {
                categories: colCats,
                crosshair: true
            },
            yAxis: {
                min: 0,
                title: {
                    text: "Rainfall (mm)"
                }
            },
            tooltip: {
                pointFormat: "{series.name}: <b>{point.y:.2f} mm</b>"
            },
            plotOptions: {
                column: {
                    pointPadding: 0.2,
                    borderWidth: 0
                }
            },
            series: [
                {
                    name: "Tokyo",
                    data: colDat

                }, {
                    name: "New York",
                    data: [83.6, 78.8, 98.5, 93.4, 106.0, 84.5, 105.0, 104.3, 91.2, 83.5, 106.6, 92.3]

                }
            ]
        });

        const pieDat = [
            new MyPieData("Internet Explorer", 56.33),
            new MyPieData("Chrome", 24.03),
            new MyPieData("Firefox", 0.2),
            new MyPieData("Safari", 4.77),
            new MyPieData("Opera", 0.2),
            new MyPieData("Proprietary or Undetectable", 0.91)
        ];

        this.rightChart = new Highcharts.Chart({
            chart: {
                type: "pie",
                renderTo: "overviewRightChart"
            },
            title: {
                text: "Browser market shares January, 2015 to May, 2015"
            },
            tooltip: {
                headerFormat: "",
                pointFormat: "{point.name}: <b>{point.y:.2f}</b> ({point.percentage:.1f}%)"
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
                    name: "Share",
                    data: pieDat
                }
            ]
        });
    }
}

class MyPieData {
    constructor(
        public name: string,
        public y: number) {
    }
}