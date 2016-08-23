import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { FormsModule } from "@angular/forms";
import { HttpModule } from "@angular/http";

import { AppComponent } from "./app.component";
import { routing } from "./app.routing";
import { OverviewComponent } from "./overview.component";
import { TransactionService } from "./transaction.service";

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
        TransactionService
    ],
    bootstrap: [AppComponent]
})
export class AppModule {
}