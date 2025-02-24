import {Component, OnInit} from '@angular/core';
import {NgForOf} from '@angular/common';

@Component({
  selector: 'app-themes',
  imports: [
    NgForOf
  ],
  templateUrl: './themes.component.html'
})
export class ThemesComponent implements OnInit {
  themes: string[] = [
    'light', 'dark', 'cupcake', 'bumblebee', 'emerald', 'corporate',
    'synthwave', 'retro', 'cyberpunk', 'valentine', 'halloween',
    'garden', 'forest', 'aqua', 'lofi', 'pastel', 'fantasy',
    'wireframe', 'black', 'luxury', 'dracula', 'cmyk', 'autumn',
    'business', 'acid', 'lemonade', 'night', 'coffee', 'winter'
  ];

  ngOnInit() {
    // Check if user has a theme preference in localStorage
    const savedTheme = localStorage.getItem('theme');
    if (savedTheme) {
      this.setTheme(savedTheme);
    }
    // this.isDarkTheme = savedTheme === 'dark';
    // this.applyTheme();
  }

  setTheme(theme: string): void {
    document.documentElement.setAttribute('data-theme', theme);
  }
}
