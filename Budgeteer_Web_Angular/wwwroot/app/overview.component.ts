import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";

import { TransactionViewModel } from "./transaction-view-model";
import { TransactionService } from "./transaction.service";
import { CategoryService } from "./category.service";
import { CategoryViewModel } from "./category-view-model";

@Component({
    selector: "my-overview",
    templateUrl: "app/overview.component.html"
})
export class OverviewComponent implements OnInit {
    errorMessage: string;
    transactions: TransactionViewModel[];

    people: string[];
    categories: string[];
    model = new TransactionViewModel();
    submitted = false;

    onSubmit() { this.submitted = true; }

    active = true;

    constructor(
        private router: Router,
        private transactionService: TransactionService,
        private categoryService: CategoryService) {
    }

    ngOnInit() {
        this.getFormData();
        this.getTransactions();
    }

    getFormData() {


        this.categoryService.getCategories()
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
            transaction => this.transactions.push(transaction),
            error => this.errorMessage = (error as any));
    }

    newTransaction() {
        this.model = new TransactionViewModel();
        this.active = false;
        setTimeout(() => this.active = true, 0);
    }
}