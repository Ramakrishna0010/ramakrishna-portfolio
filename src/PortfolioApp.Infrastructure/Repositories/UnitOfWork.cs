using Microsoft.EntityFrameworkCore.Storage;
using PortfolioApp.Domain.Entities;
using PortfolioApp.Domain.Interfaces;
using PortfolioApp.Infrastructure.Data;
using PortfolioApp.Infrastructure.Repositories;

namespace PortfolioApp.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;

    private IRepository<ApplicationUser>? _users;
    private IRepository<About>? _abouts;
    private IRepository<Skill>? _skills;
    private IRepository<Experience>? _experiences;
    private IRepository<Education>? _educations;
    private IRepository<Project>? _projects;
    private IRepository<ProjectImage>? _projectImages;
    private IRepository<Certificate>? _certificates;
    private IRepository<Achievement>? _achievements;
    private IRepository<Domain.Entities.Service>? _services;
    private IRepository<Testimonial>? _testimonials;
    private IRepository<Blog>? _blogs;
    private IRepository<ContactMessage>? _contactMessages;
    private IRepository<Resume>? _resumes;
    private IRepository<SocialLink>? _socialLinks;
    private IRepository<MediaFile>? _mediaFiles;
    private IRepository<HomeSection>? _homeSections;

    public UnitOfWork(ApplicationDbContext context) => _context = context;

    public IRepository<ApplicationUser> Users => _users ??= new Repository<ApplicationUser>(_context);
    public IRepository<About> Abouts => _abouts ??= new Repository<About>(_context);
    public IRepository<Skill> Skills => _skills ??= new Repository<Skill>(_context);
    public IRepository<Experience> Experiences => _experiences ??= new Repository<Experience>(_context);
    public IRepository<Education> Educations => _educations ??= new Repository<Education>(_context);
    public IRepository<Project> Projects => _projects ??= new Repository<Project>(_context);
    public IRepository<ProjectImage> ProjectImages => _projectImages ??= new Repository<ProjectImage>(_context);
    public IRepository<Certificate> Certificates => _certificates ??= new Repository<Certificate>(_context);
    public IRepository<Achievement> Achievements => _achievements ??= new Repository<Achievement>(_context);
    public IRepository<Domain.Entities.Service> Services => _services ??= new Repository<Domain.Entities.Service>(_context);
    public IRepository<Testimonial> Testimonials => _testimonials ??= new Repository<Testimonial>(_context);
    public IRepository<Blog> Blogs => _blogs ??= new Repository<Blog>(_context);
    public IRepository<ContactMessage> ContactMessages => _contactMessages ??= new Repository<ContactMessage>(_context);
    public IRepository<Resume> Resumes => _resumes ??= new Repository<Resume>(_context);
    public IRepository<SocialLink> SocialLinks => _socialLinks ??= new Repository<SocialLink>(_context);
    public IRepository<MediaFile> MediaFiles => _mediaFiles ??= new Repository<MediaFile>(_context);
    public IRepository<HomeSection> HomeSections => _homeSections ??= new Repository<HomeSection>(_context);

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

    public async Task BeginTransactionAsync()
        => _transaction = await _context.Database.BeginTransactionAsync();

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
