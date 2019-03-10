import { NgModule, ModuleWithProviders } from '@angular/core';
import { SignalRService } from './signalr.service';

@NgModule({

})
export class SharedModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: SharedModule,
      providers: [SignalRService]
    };
  }
 }
