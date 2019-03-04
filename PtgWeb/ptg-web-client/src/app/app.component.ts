import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'ptg-web-client';

  constructor(private router: Router) {

  }

  // tslint:disable-next-line:use-life-cycle-interface
  ngOnInit(): void {
    this.router.navigate(['']);
  }
}
