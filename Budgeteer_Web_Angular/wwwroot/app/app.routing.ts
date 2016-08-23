import { Routes, RouterModule } from "@angular/router";

import { HomeComponent } from "./home.component";
import { OverviewComponent } from "./overview.component";

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
    }
];

export const routing = RouterModule.forRoot(appRoutes);