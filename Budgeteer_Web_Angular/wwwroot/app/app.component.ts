import { Component, OnInit } from "@angular/core";
import { Http, Headers, Response, RequestOptions } from "@angular/http";

import { Observable } from "rxjs/Observable";

@Component({
    selector: "my-app",
    templateUrl: "app/app.component.html"
})
export class AppComponent implements OnInit {
    constructor(private http: Http) {}

    ngOnInit() {
        //    this.addCategory().subscribe();
        //    this.addTransaction().subscribe();
    }

    //addCategory(): Observable<Response> {
    //    const cvm = new CategoryViewModel();
    //    cvm.Name = "Credit Test";
    //    cvm.IsDebit = false;

    //    const body = JSON.stringify(cvm);
    //    const headers = new Headers({ 'Content-Type': "application/json" });
    //    const options = new RequestOptions({ headers: headers });

    //    return this.http.post("api/categories", body, options);
    //}

    //addTransaction(): Observable<Response> {
    //    const tvm = new TransactionViewModel();
    //    tvm.Date = new Date();
    //    console.log(tvm.Date);
    //    tvm.Amount = 10.05;
    //    tvm.Note = "Test";
    //    tvm.PersonName = "Aleks Angelov";
    //    tvm.CategoryName = "Food";

    //    const body = JSON.stringify(tvm);
    //    const headers = new Headers({ 'Content-Type': "application/json" });
    //    const options = new RequestOptions({ headers: headers });

    //    return this.http.post("api/transactions", body, options);
    //}
}