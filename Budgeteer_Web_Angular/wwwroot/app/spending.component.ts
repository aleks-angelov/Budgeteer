import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { Title } from "@angular/platform-browser";

import { CategoryViewModel } from "./category-view-model";
import { CategoryService } from "./category.service";
import { UserService } from "./user.service";
import { SpendingIncomeViewModel } from "./spending-income-view-model";

@Component({
    selector: "my-spending",
    templateUrl: "app/spending.component.html"
})
export class SpendingComponent implements OnInit {
    errorMessage: string;
    people: string[];
    categories: string[];
    spendingModel = new SpendingIncomeViewModel("Aleks Angelov", null, null, "Food");
    categoryModel = new CategoryViewModel("", true);
    active = true;

    topLeftChart: HighchartsChartObject;
    topRightChart: HighchartsChartObject;
    bottomLeftChart: HighchartsChartObject;
    bottomRightChart: HighchartsChartObject;

    constructor(
        private titleService: Title,
        private router: Router,
        private categoryService: CategoryService,
        private userService: UserService) {
    }

    ngOnInit() {
        this.titleService.setTitle("Spending - Budgeteer");
        this.getFormData();

        this.createCharts();
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
        this.topLeftChart = new Highcharts.Chart({
            chart: {
                type: "column",
                renderTo: "spendingTopLeftChart"
            },
            title: {
                text: "Monthly Average Rainfall"
            },
            xAxis: {
                categories: [
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
                ],
                crosshair: true
            },
            yAxis: {
                min: 0,
                title: {
                    text: "Rainfall (mm)"
                }
            },
            tooltip: {
                pointFormat: "{series.name}: <b>{point.y:.1f} mm</b>"
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
                    data: [49.9, 71.5, 106.4, 129.2, 144.0, 176.0, 135.6, 148.5, 216.4, 194.1, 95.6, 54.4]

                }, {
                    name: "New York",
                    data: [83.6, 78.8, 98.5, 93.4, 106.0, 84.5, 105.0, 104.3, 91.2, 83.5, 106.6, 92.3]

                }
            ]
        });

        this.topRightChart = new Highcharts.Chart({
            chart: {
                type: "column",
                renderTo: "spendingTopRightChart"
            },
            title: {
                text: "Monthly Average Rainfall"
            },
            xAxis: {
                categories: [
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
                ],
                crosshair: true
            },
            yAxis: {
                min: 0,
                title: {
                    text: "Rainfall (mm)"
                }
            },
            tooltip: {
                pointFormat: "{series.name}: <b>{point.y:.1f} mm</b>"
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
                    data: [49.9, 71.5, 106.4, 129.2, 144.0, 176.0, 135.6, 148.5, 216.4, 194.1, 95.6, 54.4]

                }, {
                    name: "New York",
                    data: [83.6, 78.8, 98.5, 93.4, 106.0, 84.5, 105.0, 104.3, 91.2, 83.5, 106.6, 92.3]

                }
            ]
        });

        this.bottomLeftChart = new Highcharts.Chart({
            chart: {
                type: "pie",
                renderTo: "spendingBottomLeftChart"
            },
            title: {
                text: "Browser market shares January, 2015 to May, 2015"
            },
            tooltip: {
                pointFormat: "{series.name}: <b>{point.percentage:.1f}%</b>"
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
                    name: "Brands",
                    data: [
                        {
                            name: "Microsoft Internet Explorer",
                            y: 56.33
                        }, {
                            name: "Chrome",
                            y: 24.03
                        }, {
                            name: "Firefox",
                            y: 10.38
                        }, {
                            name: "Safari",
                            y: 4.77
                        }, {
                            name: "Opera",
                            y: 0.91
                        }, {
                            name: "Proprietary or Undetectable",
                            y: 0.2
                        }
                    ]
                }
            ]
        });

        this.bottomRightChart = new Highcharts.Chart({
            chart: {
                type: "pie",
                renderTo: "spendingBottomRightChart"
            },
            title: {
                text: "Browser market shares January, 2015 to May, 2015"
            },
            tooltip: {
                pointFormat: "{series.name}: <b>{point.percentage:.1f}%</b>"
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
                    name: "Brands",
                    data: [
                        {
                            name: "Microsoft Internet Explorer",
                            y: 56.33
                        }, {
                            name: "Chrome",
                            y: 24.03
                        }, {
                            name: "Firefox",
                            y: 10.38
                        }, {
                            name: "Safari",
                            y: 4.77
                        }, {
                            name: "Opera",
                            y: 0.91
                        }, {
                            name: "Proprietary or Undetectable",
                            y: 0.2
                        }
                    ]
                }
            ]
        });
    }

    updateLeftCharts() {
        
    }

    updateRightCharts() {

    }
}