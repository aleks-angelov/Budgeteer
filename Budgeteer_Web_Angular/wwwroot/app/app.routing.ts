import { Routes, RouterModule } from "@angular/router";

import { HomeComponent } from "./home.component";
import { OverviewComponent } from "./overview.component";
import { SpendingComponent } from "./spending.component";

const appRoutes: Routes = [
    {
        path: "home",
        component: HomeComponent
    },
    {
        path: "",
        redirectTo: "/home",
        pathMatch: "full"
    },
    {
        path: "overview",
        component: OverviewComponent
    },
    {
        path: "spending",
        component: SpendingComponent
    }
];

export const routing = RouterModule.forRoot(appRoutes);