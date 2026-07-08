using PortfolioApp.Domain.Entities;

namespace PortfolioApp.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<ApplicationUser> Users { get; }
    IRepository<About> Abouts { get; }
    IRepository<Skill> Skills { get; }
    IRepository<Experience> Experiences { get; }
    IRepository<Education> Educations { get; }
    IRepository<Project> Projects { get; }
    IRepository<ProjectImage> ProjectImages { get; }
    IRepository<Certificate> Certificates { get; }
    IRepository<Achievement> Achievements { get; }
    IRepository<Service> Services { get; }
    IRepository<Testimonial> Testimonials { get; }
    IRepository<Blog> Blogs { get; }
    IRepository<ContactMessage> ContactMessages { get; }
    IRepository<Resume> Resumes { get; }
    IRepository<SocialLink> SocialLinks { get; }
    IRepository<MediaFile> MediaFiles { get; }
    IRepository<HomeSection> HomeSections { get; }
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
