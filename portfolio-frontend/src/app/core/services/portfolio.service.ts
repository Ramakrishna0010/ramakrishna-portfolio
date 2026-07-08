import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ApiResponse, PagedResponse } from '../models/api-response.model';
import {
  AboutDto, SkillDto, SkillGroupDto, ExperienceDto, EducationDto,
  ProjectDto, BlogDto, BlogListDto, CertificateDto, AchievementDto,
  TestimonialDto, ServiceDto, ContactMessageDto, ResumeDto, SocialLinkDto
} from '../models/portfolio.model';

@Injectable({ providedIn: 'root' })
export class PortfolioService {
  private api = environment.apiUrl;

  constructor(private http: HttpClient) {}

  // About
  getAbout(): Observable<ApiResponse<AboutDto>> {
    return this.http.get<ApiResponse<AboutDto>>(`${this.api}/about`);
  }
  createAbout(dto: Partial<AboutDto>): Observable<ApiResponse<AboutDto>> {
    return this.http.post<ApiResponse<AboutDto>>(`${this.api}/about`, dto);
  }
  updateAbout(id: number, dto: Partial<AboutDto>): Observable<ApiResponse<AboutDto>> {
    return this.http.put<ApiResponse<AboutDto>>(`${this.api}/about/${id}`, dto);
  }

  // Skills
  getSkills(): Observable<ApiResponse<SkillGroupDto[]>> {
    return this.http.get<ApiResponse<SkillGroupDto[]>>(`${this.api}/skills`);
  }
  getAllSkills(): Observable<ApiResponse<PagedResponse<SkillDto>>> {
    return this.http.get<ApiResponse<PagedResponse<SkillDto>>>(`${this.api}/skills`, { params: new HttpParams().set('pageSize', '1') });
  }
  createSkill(dto: Partial<SkillDto>): Observable<ApiResponse<SkillDto>> {
    return this.http.post<ApiResponse<SkillDto>>(`${this.api}/skills`, dto);
  }
  updateSkill(id: number, dto: Partial<SkillDto>): Observable<ApiResponse<SkillDto>> {
    return this.http.put<ApiResponse<SkillDto>>(`${this.api}/skills/${id}`, dto);
  }
  deleteSkill(id: number): Observable<ApiResponse<boolean>> {
    return this.http.delete<ApiResponse<boolean>>(`${this.api}/skills/${id}`);
  }

  // Experience
  getExperiences(): Observable<ApiResponse<ExperienceDto[]>> {
    return this.http.get<ApiResponse<ExperienceDto[]>>(`${this.api}/experience`);
  }
  createExperience(dto: Partial<ExperienceDto>): Observable<ApiResponse<ExperienceDto>> {
    return this.http.post<ApiResponse<ExperienceDto>>(`${this.api}/experience`, dto);
  }
  updateExperience(id: number, dto: Partial<ExperienceDto>): Observable<ApiResponse<ExperienceDto>> {
    return this.http.put<ApiResponse<ExperienceDto>>(`${this.api}/experience/${id}`, dto);
  }
  deleteExperience(id: number): Observable<ApiResponse<boolean>> {
    return this.http.delete<ApiResponse<boolean>>(`${this.api}/experience/${id}`);
  }

  // Education
  getEducations(): Observable<ApiResponse<EducationDto[]>> {
    return this.http.get<ApiResponse<EducationDto[]>>(`${this.api}/education`);
  }
  createEducation(dto: Partial<EducationDto>): Observable<ApiResponse<EducationDto>> {
    return this.http.post<ApiResponse<EducationDto>>(`${this.api}/education`, dto);
  }
  updateEducation(id: number, dto: Partial<EducationDto>): Observable<ApiResponse<EducationDto>> {
    return this.http.put<ApiResponse<EducationDto>>(`${this.api}/education/${id}`, dto);
  }
  deleteEducation(id: number): Observable<ApiResponse<boolean>> {
    return this.http.delete<ApiResponse<boolean>>(`${this.api}/education/${id}`);
  }

  // Projects
  getProjects(params?: { category?: string; search?: string; page?: number; pageSize?: number }): Observable<ApiResponse<PagedResponse<ProjectDto>>> {
    let httpParams = new HttpParams();
    if (params?.category) httpParams = httpParams.set('category', params.category);
    if (params?.search) httpParams = httpParams.set('search', params.search);
    if (params?.page) httpParams = httpParams.set('pageNumber', params.page.toString());
    if (params?.pageSize) httpParams = httpParams.set('pageSize', params.pageSize.toString());
    return this.http.get<ApiResponse<PagedResponse<ProjectDto>>>(`${this.api}/projects`, { params: httpParams });
  }
  getProjectBySlug(slug: string): Observable<ApiResponse<ProjectDto>> {
    return this.http.get<ApiResponse<ProjectDto>>(`${this.api}/projects/${slug}`);
  }
  createProject(dto: Partial<ProjectDto>): Observable<ApiResponse<ProjectDto>> {
    return this.http.post<ApiResponse<ProjectDto>>(`${this.api}/projects`, dto);
  }
  updateProject(id: number, dto: Partial<ProjectDto>): Observable<ApiResponse<ProjectDto>> {
    return this.http.put<ApiResponse<ProjectDto>>(`${this.api}/projects/${id}`, dto);
  }
  deleteProject(id: number): Observable<ApiResponse<boolean>> {
    return this.http.delete<ApiResponse<boolean>>(`${this.api}/projects/${id}`);
  }

  // Blogs
  getBlogs(params?: { search?: string; category?: string; page?: number; pageSize?: number }): Observable<ApiResponse<PagedResponse<BlogListDto>>> {
    let httpParams = new HttpParams();
    if (params?.search) httpParams = httpParams.set('search', params.search);
    if (params?.category) httpParams = httpParams.set('category', params.category);
    if (params?.page) httpParams = httpParams.set('pageNumber', params.page.toString());
    if (params?.pageSize) httpParams = httpParams.set('pageSize', params.pageSize.toString());
    return this.http.get<ApiResponse<PagedResponse<BlogListDto>>>(`${this.api}/blogs`, { params: httpParams });
  }
  getBlogBySlug(slug: string): Observable<ApiResponse<BlogDto>> {
    return this.http.get<ApiResponse<BlogDto>>(`${this.api}/blogs/${slug}`);
  }
  createBlog(dto: Partial<BlogDto>): Observable<ApiResponse<BlogDto>> {
    return this.http.post<ApiResponse<BlogDto>>(`${this.api}/blogs`, dto);
  }
  updateBlog(id: number, dto: Partial<BlogDto>): Observable<ApiResponse<BlogDto>> {
    return this.http.put<ApiResponse<BlogDto>>(`${this.api}/blogs/${id}`, dto);
  }
  deleteBlog(id: number): Observable<ApiResponse<boolean>> {
    return this.http.delete<ApiResponse<boolean>>(`${this.api}/blogs/${id}`);
  }

  // Certificates
  getCertificates(): Observable<ApiResponse<CertificateDto[]>> {
    return this.http.get<ApiResponse<CertificateDto[]>>(`${this.api}/certificates`);
  }
  createCertificate(dto: Partial<CertificateDto>): Observable<ApiResponse<CertificateDto>> {
    return this.http.post<ApiResponse<CertificateDto>>(`${this.api}/certificates`, dto);
  }
  updateCertificate(id: number, dto: Partial<CertificateDto>): Observable<ApiResponse<CertificateDto>> {
    return this.http.put<ApiResponse<CertificateDto>>(`${this.api}/certificates/${id}`, dto);
  }
  deleteCertificate(id: number): Observable<ApiResponse<boolean>> {
    return this.http.delete<ApiResponse<boolean>>(`${this.api}/certificates/${id}`);
  }

  // Achievements
  getAchievements(): Observable<ApiResponse<AchievementDto[]>> {
    return this.http.get<ApiResponse<AchievementDto[]>>(`${this.api}/achievements`);
  }
  createAchievement(dto: Partial<AchievementDto>): Observable<ApiResponse<AchievementDto>> {
    return this.http.post<ApiResponse<AchievementDto>>(`${this.api}/achievements`, dto);
  }
  updateAchievement(id: number, dto: Partial<AchievementDto>): Observable<ApiResponse<AchievementDto>> {
    return this.http.put<ApiResponse<AchievementDto>>(`${this.api}/achievements/${id}`, dto);
  }
  deleteAchievement(id: number): Observable<ApiResponse<boolean>> {
    return this.http.delete<ApiResponse<boolean>>(`${this.api}/achievements/${id}`);
  }

  // Testimonials
  getTestimonials(): Observable<ApiResponse<TestimonialDto[]>> {
    return this.http.get<ApiResponse<TestimonialDto[]>>(`${this.api}/testimonials`);
  }
  createTestimonial(dto: Partial<TestimonialDto>): Observable<ApiResponse<TestimonialDto>> {
    return this.http.post<ApiResponse<TestimonialDto>>(`${this.api}/testimonials`, dto);
  }
  updateTestimonial(id: number, dto: Partial<TestimonialDto>): Observable<ApiResponse<TestimonialDto>> {
    return this.http.put<ApiResponse<TestimonialDto>>(`${this.api}/testimonials/${id}`, dto);
  }
  deleteTestimonial(id: number): Observable<ApiResponse<boolean>> {
    return this.http.delete<ApiResponse<boolean>>(`${this.api}/testimonials/${id}`);
  }

  // Services
  getServices(): Observable<ApiResponse<ServiceDto[]>> {
    return this.http.get<ApiResponse<ServiceDto[]>>(`${this.api}/services`);
  }
  createService(dto: Partial<ServiceDto>): Observable<ApiResponse<ServiceDto>> {
    return this.http.post<ApiResponse<ServiceDto>>(`${this.api}/services`, dto);
  }
  updateService(id: number, dto: Partial<ServiceDto>): Observable<ApiResponse<ServiceDto>> {
    return this.http.put<ApiResponse<ServiceDto>>(`${this.api}/services/${id}`, dto);
  }
  deleteService(id: number): Observable<ApiResponse<boolean>> {
    return this.http.delete<ApiResponse<boolean>>(`${this.api}/services/${id}`);
  }

  // Contact
  sendMessage(dto: Partial<ContactMessageDto>): Observable<ApiResponse<ContactMessageDto>> {
    return this.http.post<ApiResponse<ContactMessageDto>>(`${this.api}/contact`, dto);
  }
  getMessages(): Observable<ApiResponse<PagedResponse<ContactMessageDto>>> {
    return this.http.get<ApiResponse<PagedResponse<ContactMessageDto>>>(`${this.api}/contact`, { params: new HttpParams().set('pageSize', '100') });
  }
  updateMessageStatus(id: number, dto: { status: number; adminNotes: string }): Observable<ApiResponse<ContactMessageDto>> {
    return this.http.patch<ApiResponse<ContactMessageDto>>(`${this.api}/contact/status`, { id, ...dto });
  }

  // Resume
  getResumes(): Observable<ApiResponse<ResumeDto[]>> {
    return this.http.get<ApiResponse<ResumeDto[]>>(`${this.api}/resume/versions`);
  }
  uploadResume(formData: FormData): Observable<ApiResponse<ResumeDto>> {
    return this.http.post<ApiResponse<ResumeDto>>(`${this.api}/resume/upload`, formData);
  }
  downloadResume(): Observable<Blob> {
    return this.http.get(`${this.api}/resume/download`, { responseType: 'blob' });
  }

  // Social Links
  getSocialLinks(): Observable<ApiResponse<SocialLinkDto[]>> {
    return this.http.get<ApiResponse<SocialLinkDto[]>>(`${this.api}/sociallinks`);
  }
  createSocialLink(dto: Partial<SocialLinkDto>): Observable<ApiResponse<SocialLinkDto>> {
    return this.http.post<ApiResponse<SocialLinkDto>>(`${this.api}/sociallinks`, dto);
  }
  updateSocialLink(id: number, dto: Partial<SocialLinkDto>): Observable<ApiResponse<SocialLinkDto>> {
    return this.http.put<ApiResponse<SocialLinkDto>>(`${this.api}/sociallinks/${id}`, dto);
  }
  deleteSocialLink(id: number): Observable<ApiResponse<boolean>> {
    return this.http.delete<ApiResponse<boolean>>(`${this.api}/sociallinks/${id}`);
  }
}
