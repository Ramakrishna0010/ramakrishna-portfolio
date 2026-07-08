import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-footer',
  standalone: true,
  imports: [RouterLink],
  template: `
    <footer class="footer">
      <div class="footer-container">
        <div class="footer-brand">
          <span class="brand-icon">⚡</span>
          <span class="brand-text">Portfolio</span>
          <p>Building digital experiences that matter.</p>
        </div>
        <div class="footer-links">
          <h4>Quick Links</h4>
          <ul>
            <li><a routerLink="/about">About</a></li>
            <li><a routerLink="/projects">Projects</a></li>
            <li><a routerLink="/blogs">Blog</a></li>
            <li><a routerLink="/contact">Contact</a></li>
          </ul>
        </div>
        <div class="footer-links">
          <h4>More</h4>
          <ul>
            <li><a routerLink="/skills">Skills</a></li>
            <li><a routerLink="/experience">Experience</a></li>
            <li><a routerLink="/certificates">Certificates</a></li>
            <li><a routerLink="/achievements">Achievements</a></li>
          </ul>
        </div>
      </div>
      <div class="footer-bottom">
        <p>&copy; {{ year }} Portfolio. Built with Angular 18 & ASP.NET Core 8.</p>
      </div>
    </footer>
  `,
  styleUrl: './footer.component.scss'
})
export class FooterComponent {
  year = new Date().getFullYear();
}
