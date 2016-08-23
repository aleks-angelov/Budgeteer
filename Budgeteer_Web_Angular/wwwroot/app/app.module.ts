import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { FormsModule } from "@angular/forms";
import { HttpModule } from "@angular/http";

import { AppComponent } from "./app.component";
import { routing } from "./app.routing";
import { OverviewComponent } from "./overview.component";
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
        OverviewComponent
    ],
    providers: [
        TransactionService,
        CategoryService,
        UserService,
        HelperService
    ],
    bootstrap: [AppComponent]
})
export class AppModule {
}