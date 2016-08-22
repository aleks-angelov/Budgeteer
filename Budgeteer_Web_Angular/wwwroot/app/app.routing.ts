import { Routes, RouterModule } from "@angular/router";

import { OverviewComponent } from "./overview.component";

const appRoutes: Routes = [
    {
        path: "overview",
        component: OverviewComponent
    },
    {
        path: "",
        redirectTo: "/overview",
        pathMatch: "full"
    }
];

export const routing = RouterModule.forRoot(appRoutes);