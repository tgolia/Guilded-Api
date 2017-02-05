import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { UniversalModule } from "angular2-universal";
import { SlimLoadingBarModule } from "ng2-slim-loading-bar";

import { AuthModule } from "./core/modules/auth";
import { ProgressBarService } from "./core/services/progress-bar";

import { HomeAppModule } from "./home/modules/app";
import { ForumsAppModule } from "./forums/modules/app";
import { AccountModule } from "./account/modules/app";

import { AppComponent } from "./app.component"
import { NavMenuComponent } from "./core/components/navmenu";

@NgModule({
    bootstrap: [
        AppComponent
    ],
    declarations: [
        AppComponent,
        NavMenuComponent,
    ],
    providers: [
        ProgressBarService,
    ],
    imports: [
        UniversalModule, // Must be first import. This automatically imports BrowserModule, HttpModule, and JsonpModule too.
        SlimLoadingBarModule.forRoot(),
        RouterModule.forRoot([]),
        AuthModule,
        HomeAppModule,
        ForumsAppModule,
        AccountModule,
    ]
})
export class AppModule {
}
