import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { Title } from "@angular/platform-browser";

import { TransactionViewModel } from "./transaction-view-model";
import { CategoryViewModel } from "./category-view-model";
import { TransactionService } from "./transaction.service";
import { CategoryService } from "./category.service";
import { UserService } from "./user.service";
import { SpendingIncomeViewModel } from "./spending-income-view-model";

@Component({
    selector: "my-spending",
    templateUrl: "app/spending.component.html"
})
export class SpendingComponent implements OnInit {
    errorMessage: string;
    transactions: TransactionViewModel[];

    people: string[];
    categories: string[];
    spendingModel = new SpendingIncomeViewModel("Aleks Angelov", null, null, "Food");
    categoryModel = new CategoryViewModel("", true);
    active = true;

    constructor(
        private titleService: Title,
        private router: Router,
        private transactionService: TransactionService,
        private categoryService: CategoryService,
        private userService: UserService) {
    }

    ngOnInit() {
        this.titleService.setTitle("Spending - Budgeteer");
        this.getFormData();
        this.getTransactions();

        const chart = new Highcharts.Chart({
            chart: {
                type: "bar",
                renderTo: "myChart"
            },
            title: {
                text: "Fruit Consumption"
            },
            xAxis: {
                categories: ["Apples", "Bananas", "Oranges"]
            },
            yAxis: {
                title: {
                    text: "Fruit eaten"
                }
            },
            series: [
                {
                    name: "Jane",
                    data: [1, 0, 4]
                }, {
                    name: "John",
                    data: [5, 7, 3]
                }
            ]
        });
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
}