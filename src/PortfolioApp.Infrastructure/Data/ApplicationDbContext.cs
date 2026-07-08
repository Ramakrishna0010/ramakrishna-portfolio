using Microsoft.EntityFrameworkCore;
using PortfolioApp.Domain.Entities;

namespace PortfolioApp.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<ApplicationUser> Users => Set<ApplicationUser>();
    public DbSet<About> Abouts => Set<About>();
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<Experience> Experiences => Set<Experience>();
    public DbSet<Education> Educations => Set<Education>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<ProjectImage> ProjectImages => Set<ProjectImage>();
    public DbSet<Certificate> Certificates => Set<Certificate>();
    public DbSet<Achievement> Achievements => Set<Achievement>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<Testimonial> Testimonials => Set<Testimonial>();
    public DbSet<Blog> Blogs => Set<Blog>();
    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();
    public DbSet<Resume> Resumes => Set<Resume>();
    public DbSet<SocialLink> SocialLinks => Set<SocialLink>();
    public DbSet<MediaFile> MediaFiles => Set<MediaFile>();
    public DbSet<HomeSection> HomeSections => Set<HomeSection>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Global query filters for soft delete
        modelBuilder.Entity<ApplicationUser>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<About>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Skill>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Experience>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Education>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Project>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Certificate>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Achievement>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Service>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Testimonial>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Blog>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<ContactMessage>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Resume>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<SocialLink>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<MediaFile>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<HomeSection>().HasQueryFilter(e => !e.IsDeleted);

        // Indexes
        modelBuilder.Entity<ApplicationUser>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<Project>().HasIndex(p => p.Slug).IsUnique();
        modelBuilder.Entity<Blog>().HasIndex(b => b.Slug).IsUnique();

        // Relationships
        modelBuilder.Entity<ProjectImage>()
            .HasOne(pi => pi.Project)
            .WithMany(p => p.Images)
            .HasForeignKey(pi => pi.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Blog>()
            .HasOne(b => b.Author)
            .WithMany()
            .HasForeignKey(b => b.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Column precision
        modelBuilder.Entity<Service>()
            .Property(s => s.StartingPrice)
            .HasPrecision(18, 2);
    }
}
