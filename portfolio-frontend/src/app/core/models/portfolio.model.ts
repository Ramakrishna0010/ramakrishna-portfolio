export interface AboutDto {
  id: number;
  fullName: string;
  title: string;
  subtitle: string;
  bio: string;
  shortBio: string;
  profileImageUrl: string;
  heroImageUrl: string;
  email: string;
  phone: string;
  location: string;
  nationality: string;
  languages: string;
  yearsOfExperience: number;
  projectsCompleted: number;
  happyClients: number;
  resumeUrl: string;
  isAvailableForWork: boolean;
  availabilityStatus: string;
  metaTitle: string;
  metaDescription: string;
}

export interface SkillDto {
  id: number;
  name: string;
  category: number;
  categoryName: string;
  percentage: number;
  iconClass: string;
  iconUrl: string;
  color: string;
  displayOrder: number;
  isFeatured: boolean;
  description: string;
  yearsOfExperience: number;
  isActive: boolean;
}

export interface SkillGroupDto {
  category: string;
  skills: SkillDto[];
}

export interface ExperienceDto {
  id: number;
  companyName: string;
  companyLogoUrl: string;
  companyWebsite: string;
  designation: string;
  employmentType: string;
  location: string;
  isRemote: boolean;
  startDate: string;
  endDate?: string;
  isCurrentJob: boolean;
  description: string;
  responsibilities: string;
  technologiesUsed: string;
  achievements: string;
  displayOrder: number;
  isFeatured: boolean;
  isActive: boolean;
  duration: string;
}

export interface EducationDto {
  id: number;
  institutionName: string;
  institutionLogoUrl: string;
  degree: string;
  fieldOfStudy: string;
  grade: string;
  description: string;
  activities: string;
  startDate: string;
  endDate?: string;
  isCurrentlyStudying: boolean;
  location: string;
  displayOrder: number;
  isFeatured: boolean;
  isActive: boolean;
}

export interface ProjectDto {
  id: number;
  title: string;
  slug: string;
  shortDescription: string;
  description: string;
  technologyStack: string;
  architecture: string;
  gitHubUrl: string;
  liveDemoUrl: string;
  videoDemoUrl: string;
  thumbnailUrl: string;
  features: string;
  responsibilities: string;
  challenges: string;
  outcomes: string;
  startDate: string;
  endDate?: string;
  duration: string;
  status: number;
  statusName: string;
  category: string;
  displayOrder: number;
  isFeatured: boolean;
  viewCount: number;
  likeCount: number;
  isActive: boolean;
  images: ProjectImageDto[];
}

export interface ProjectImageDto {
  id: number;
  imageUrl: string;
  caption: string;
  displayOrder: number;
  isThumbnail: boolean;
}

export interface BlogDto {
  id: number;
  title: string;
  slug: string;
  summary: string;
  content: string;
  thumbnailUrl: string;
  coverImageUrl: string;
  tags: string;
  category: string;
  status: number;
  statusName: string;
  publishedAt?: string;
  viewCount: number;
  likeCount: number;
  readTimeMinutes: number;
  metaTitle: string;
  metaDescription: string;
  metaKeywords: string;
  isFeatured: boolean;
  isActive: boolean;
  authorName: string;
  createdAt: string;
}

export interface BlogListDto {
  id: number;
  title: string;
  slug: string;
  summary: string;
  thumbnailUrl: string;
  tags: string;
  category: string;
  publishedAt?: string;
  viewCount: number;
  readTimeMinutes: number;
  isFeatured: boolean;
  authorName: string;
}

export interface CertificateDto {
  id: number;
  name: string;
  issuedBy: string;
  issuedByLogoUrl: string;
  issueDate: string;
  expiryDate?: string;
  doesNotExpire: boolean;
  credentialId: string;
  credentialUrl: string;
  certificateImageUrl: string;
  description: string;
  skills: string;
  displayOrder: number;
  isFeatured: boolean;
  isActive: boolean;
}

export interface AchievementDto {
  id: number;
  title: string;
  description: string;
  iconClass: string;
  iconUrl: string;
  category: string;
  achievedDate: string;
  issuedBy: string;
  awardUrl: string;
  imageUrl: string;
  displayOrder: number;
  isFeatured: boolean;
  isActive: boolean;
}

export interface TestimonialDto {
  id: number;
  clientName: string;
  clientTitle: string;
  clientCompany: string;
  clientImageUrl: string;
  clientLinkedIn: string;
  content: string;
  rating: number;
  projectWorkedOn: string;
  date: string;
  displayOrder: number;
  isFeatured: boolean;
  isApproved: boolean;
  isActive: boolean;
}

export interface ServiceDto {
  id: number;
  title: string;
  description: string;
  shortDescription: string;
  iconClass: string;
  iconUrl: string;
  serviceType: number;
  serviceTypeName: string;
  features: string;
  technologiesUsed: string;
  startingPrice?: number;
  pricingUnit: string;
  displayOrder: number;
  isFeatured: boolean;
  color: string;
  isActive: boolean;
}

export interface ContactMessageDto {
  id: number;
  name: string;
  email: string;
  phone: string;
  subject: string;
  message: string;
  company: string;
  budget: string;
  projectType: string;
  status: number;
  statusName: string;
  createdAt: string;
  readAt?: string;
  repliedAt?: string;
  adminNotes: string;
}

export interface ResumeDto {
  id: number;
  fileName: string;
  fileUrl: string;
  version: string;
  description: string;
  fileSizeBytes: number;
  fileSizeFormatted: string;
  contentType: string;
  isCurrentVersion: boolean;
  downloadCount: number;
  uploadedAt: string;
}

export interface SocialLinkDto {
  id: number;
  platform: string;
  url: string;
  iconClass: string;
  iconUrl: string;
  color: string;
  username: string;
  displayOrder: number;
  showInHeader: boolean;
  showInFooter: boolean;
  showInAbout: boolean;
  isActive: boolean;
}
