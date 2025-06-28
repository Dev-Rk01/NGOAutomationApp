import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
    username = '';
    password = '';
    errorMessage = '';

    constructor(private auth: AuthService, private router: Router) {}

    login() {
      this.auth.login({ username: this.username, password: this.password }).subscribe({
        next: res => {
          this.auth.saveToken(res.token);
          this.router.navigate(['/dashboard']);
        },
        error: err => {
          this.errorMessage = 'Login failed. Please check your credentials.';
          console.error('Login error:', err);
        }
      })
    }
}
