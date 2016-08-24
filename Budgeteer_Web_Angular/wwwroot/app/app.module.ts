import { NgModule } from "@angular/core";
import { BrowserModule, Title } from "@angular/platform-browser";
import { FormsModule } from "@angular/forms";
import { HttpModule } from "@angular/http";

import { AppComponent } from "./app.component";
import { routing } from "./app.routing";
import { HomeComponent } from "./home.component";
import { OverviewComponent } from "./overview.component";
import { SpendingComponent } from "./spending.component";
import { IncomeComponent } from "./income.component";
import { TransactionService } from "./transaction.service";
import { CategoryService } from "./category.service";
import { UserService } from "./user.service";
import { HelperService } from "./helper.service";

@NgModule({
    imports: [
        BrowserModule,
        FormsModule,
        routing,
        HttpModule
    ],
    declarations: [
        AppComponent,
        HomeComponent,
        OverviewComponent,
        SpendingComponent,
        IncomeComponent
    ],
    providers: [
        Title,
        TransactionService,
        CategoryService,
        UserService,
        HelperService
    ],
    bootstrap: [AppComponent]
})
export class AppModule {
}