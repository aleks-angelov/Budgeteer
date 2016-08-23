import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";

import { TransactionViewModel } from "./transaction-view-model";
import { TransactionService } from "./transaction.service";

@Component({
    selector: "my-overview",
    templateUrl: "app/overview.component.html"
})
export class OverviewComponent {
    errorMessage: string;
    transactions: TransactionViewModel[];

    constructor(
        private router: Router,
        private transactionService: TransactionService) {
    }

    ngOnInit() {
        this.getTransactions();
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
}