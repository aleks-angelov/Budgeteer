import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";

import { TransactionViewModel } from "./transaction-view-model";
import { TransactionService } from "./transaction.service";
import { CategoryService } from "./category.service";
import { UserService } from "./user.service";

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

    constructor(
        private router: Router,
        private transactionService: TransactionService,
        private categoryService: CategoryService,
        private userService: UserService) {
    }

    ngOnInit() {
        this.getFormData();
        this.getTransactions();
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
}