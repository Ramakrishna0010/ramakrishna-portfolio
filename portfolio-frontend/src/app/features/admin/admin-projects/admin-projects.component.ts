import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PortfolioService } from '../../../core/services/portfolio.service';
import { ProjectDto } from '../../../core/models/portfolio.model';

@Component({
  selector: 'app-admin-projects',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './admin-projects.component.html',
  styleUrl: '../admin-skills/admin-skills.component.scss'
})
export class AdminProjectsComponent implements OnInit {
  projects = signal<ProjectDto[]>([]);
  form!: FormGroup;
  editingId = signal<number | null>(null);
  showForm = signal(false);
  loading = signal(false);

  statuses = [{ value: 1, label: 'In Progress' }, { value: 2, label: 'Completed' }, { value: 3, label: 'On Hold' }, { value: 4, label: 'Archived' }];

  constructor(private fb: FormBuilder, private portfolioService: PortfolioService) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      title: ['', Validators.required], shortDescription: ['', Validators.required],
      description: [''], technologyStack: [''], architecture: [''],
      gitHubUrl: [''], liveDemoUrl: [''], videoDemoUrl: [''], thumbnailUrl: [''],
      features: [''], responsibilities: [''], challenges: [''], outcomes: [''],
      startDate: ['', Validators.required], endDate: [''], duration: [''],
      status: [2], category: [''], displayOrder: [0], isFeatured: [false]
    });
    this.load();
  }

  load(): void { this.portfolioService.getProjects({ pageSize: 100 }).subscribe(r => { if (r.success) this.projects.set(r.data.items); }); }
  openCreate(): void { this.editingId.set(null); this.form.reset({ status: 2, displayOrder: 0 }); this.showForm.set(true); }
  openEdit(p: ProjectDto): void {
    this.editingId.set(p.id);
    this.form.patchValue({ ...p, startDate: p.startDate?.substring(0, 10), endDate: p.endDate?.substring(0, 10) });
    this.showForm.set(true);
  }
  onSubmit(): void {
    if (this.form.invalid) return;
    this.loading.set(true);
    const action = this.editingId() ? this.portfolioService.updateProject(this.editingId()!, this.form.value) : this.portfolioService.createProject(this.form.value);
    action.subscribe({ next: r => { if (r.success) { this.load(); this.showForm.set(false); } this.loading.set(false); }, error: () => this.loading.set(false) });
  }
  delete(id: number): void { if (!confirm('Delete?')) return; this.portfolioService.deleteProject(id).subscribe(r => { if (r.success) this.load(); }); }
}
