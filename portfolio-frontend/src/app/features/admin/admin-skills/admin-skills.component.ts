import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PortfolioService } from '../../../core/services/portfolio.service';
import { SkillDto } from '../../../core/models/portfolio.model';

@Component({
  selector: 'app-admin-skills',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './admin-skills.component.html',
  styleUrl: './admin-skills.component.scss'
})
export class AdminSkillsComponent implements OnInit {
  skills = signal<SkillDto[]>([]);
  form!: FormGroup;
  editingId = signal<number | null>(null);
  showForm = signal(false);
  loading = signal(false);
  saved = signal(false);

  categories = [
    { value: 1, label: 'Frontend' }, { value: 2, label: 'Backend' },
    { value: 3, label: 'Database' }, { value: 4, label: 'DevOps' },
    { value: 5, label: 'Cloud' }, { value: 6, label: 'Mobile' },
    { value: 7, label: 'Tools' }, { value: 8, label: 'Soft Skills' }, { value: 9, label: 'Other' }
  ];

  constructor(private fb: FormBuilder, private portfolioService: PortfolioService) {}

  ngOnInit(): void {
    this.initForm();
    this.loadSkills();
  }

  initForm(): void {
    this.form = this.fb.group({
      name: ['', Validators.required],
      category: [1, Validators.required],
      percentage: [80, [Validators.required, Validators.min(0), Validators.max(100)]],
      iconClass: [''],
      iconUrl: [''],
      color: [''],
      displayOrder: [0],
      isFeatured: [false],
      description: [''],
      yearsOfExperience: [0]
    });
  }

  loadSkills(): void {
    this.portfolioService.getAllSkills().subscribe(r => { if (r.success) this.skills.set(r.data); });
  }

  openCreate(): void { this.editingId.set(null); this.form.reset({ category: 1, percentage: 80, displayOrder: 0, isFeatured: false, yearsOfExperience: 0 }); this.showForm.set(true); }

  openEdit(skill: SkillDto): void {
    this.editingId.set(skill.id);
    this.form.patchValue(skill);
    this.showForm.set(true);
  }

  onSubmit(): void {
    if (this.form.invalid) return;
    this.loading.set(true);
    const action = this.editingId()
      ? this.portfolioService.updateSkill(this.editingId()!, this.form.value)
      : this.portfolioService.createSkill(this.form.value);

    action.subscribe({
      next: r => {
        if (r.success) { this.loadSkills(); this.showForm.set(false); this.saved.set(true); setTimeout(() => this.saved.set(false), 2000); }
        this.loading.set(false);
      },
      error: () => this.loading.set(false)
    });
  }

  delete(id: number): void {
    if (!confirm('Delete this skill?')) return;
    this.portfolioService.deleteSkill(id).subscribe(r => { if (r.success) this.loadSkills(); });
  }

  getCategoryLabel(val: number): string {
    return this.categories.find(c => c.value === val)?.label ?? '';
  }
}
