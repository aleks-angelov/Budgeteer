import { Injectable } from "@angular/core";
import { Headers, Http, Response, RequestOptions } from "@angular/http";

import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/map";
import "rxjs/add/operator/catch";

import { CategoryViewModel } from "./category-view-model";
import { HelperService } from "./helper.service";

@Injectable()
export class CategoryService {
    private categoriesUrl = "api/categories"; // URL to web api

    constructor(
        private http: Http,
        private helperService: HelperService) {
    }

    getCategories(isDebit: boolean): Observable<string[]> {
        return (this.http.get(this.categoriesUrl + (isDebit ? "?debit=true" : "?debit=false"))
            .map(this.helperService.extractData)
            .catch(this.helperService.handleError));
    }

    postCategory(cvm: CategoryViewModel): Observable<CategoryViewModel> {
        const body = JSON.stringify(cvm);
        const headers = new Headers({ 'Content-Type': "application/json" });
        const options = new RequestOptions({ headers: headers });

        return (this.http.post(this.categoriesUrl, body, options).catch(this.helperService.handleError));
    }
}