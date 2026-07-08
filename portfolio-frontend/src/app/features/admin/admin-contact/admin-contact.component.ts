import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PortfolioService } from '../../../core/services/portfolio.service';
import { ContactMessageDto } from '../../../core/models/portfolio.model';

@Component({
  selector: 'app-admin-contact',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="admin-page">
      <div class="page-header">
        <h2>Contact Messages</h2>
        <p>View and manage incoming messages</p>
      </div>
      @if (selected()) {
        <div class="message-detail glass-card">
          <div class="detail-header">
            <h3>{{ selected()!.subject }}</h3>
            <button class="btn-ghost" (click)="selected.set(null)">✕ Close</button>
          </div>
          <div class="detail-meta">
            <span>👤 {{ selected()!.name }}</span>
            <span>📧 {{ selected()!.email }}</span>
            @if (selected()!.phone) { <span>📞 {{ selected()!.phone }}</span> }
            @if (selected()!.company) { <span>🏢 {{ selected()!.company }}</span> }
            @if (selected()!.projectType) { <span>💼 {{ selected()!.projectType }}</span> }
            @if (selected()!.budget) { <span>💰 {{ selected()!.budget }}</span> }
            <span>📅 {{ selected()!.createdAt | date:'medium' }}</span>
          </div>
          <div class="message-body">{{ selected()!.message }}</div>
          <div class="status-update">
            <select [(ngModel)]="newStatus" class="status-select">
              <option [value]="1">New</option>
              <option [value]="2">Read</option>
              <option [value]="3">Replied</option>
              <option [value]="4">Archived</option>
            </select>
            <button class="btn-primary" (click)="updateStatus()">Update Status</button>
          </div>
        </div>
      }
      <div class="table-wrapper glass-card">
        <table class="admin-table">
          <thead><tr><th>Name</th><th>Email</th><th>Subject</th><th>Status</th><th>Date</th><th>Actions</th></tr></thead>
          <tbody>
            @for (msg of messages(); track msg.id) {
              <tr [class.unread]="msg.status === 1">
                <td>{{ msg.name }}</td>
                <td>{{ msg.email }}</td>
                <td>{{ msg.subject }}</td>
                <td>
                  <span class="badge"
                    [class.badge-warning]="msg.status===1"
                    [class.badge-primary]="msg.status===2"
                    [class.badge-success]="msg.status===3"
                    [class.badge-danger]="msg.status===4">
                    {{ msg.statusName }}
                  </span>
                </td>
                <td>{{ msg.createdAt | date:'MMM d, y' }}</td>
                <td>
                  <button class="btn-ghost" (click)="viewMessage(msg)">👁️ View</button>
                </td>
              </tr>
            }
          </tbody>
        </table>
      </div>
    </div>
  `,
  styleUrl: './admin-contact.component.scss'
})
export class AdminContactComponent implements OnInit {
  messages = signal<ContactMessageDto[]>([]);
  selected = signal<ContactMessageDto | null>(null);
  newStatus = 2;

  constructor(private portfolioService: PortfolioService) {}

  ngOnInit(): void { this.load(); }

  load(): void { this.portfolioService.getMessages().subscribe(r => { if (r.success) this.messages.set(r.data.items ?? []); }); }

  viewMessage(msg: ContactMessageDto): void {
    this.selected.set(msg);
    this.newStatus = msg.status === 1 ? 2 : msg.status;
    if (msg.status === 1) {
      this.portfolioService.updateMessageStatus(msg.id, { status: 2, adminNotes: '' }).subscribe(() => this.load());
    }
  }

  updateStatus(): void {
    if (!this.selected()) return;
    this.portfolioService.updateMessageStatus(this.selected()!.id, { status: this.newStatus, adminNotes: '' })
      .subscribe(r => { if (r.success) { this.load(); this.selected.set(null); } });
  }
}
