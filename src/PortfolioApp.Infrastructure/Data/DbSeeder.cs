using Microsoft.EntityFrameworkCore;
using PortfolioApp.Domain.Entities;
using PortfolioApp.Domain.Enums;

namespace PortfolioApp.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        await context.Database.MigrateAsync();

        if (!context.Users.IgnoreQueryFilters().Any())
        {
            var adminUser = new ApplicationUser
            {
                FirstName = "Admin",
                LastName = "User",
                Email = "admin@portfolio.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123!"),
                Role = "Admin",
                IsActive = true,
                IsEmailVerified = true,
                CreatedAt = DateTime.UtcNow
            };
            context.Users.Add(adminUser);
            await context.SaveChangesAsync();
        }

        if (!context.HomeSections.IgnoreQueryFilters().Any())
        {
            context.HomeSections.Add(new HomeSection
            {
                HeroTitle = "Hi, I'm",
                HeroSubtitle = "Full Stack Developer",
                HeroDescription = "I build scalable, modern web applications with .NET Core and Angular.",
                CtaPrimaryText = "View My Work",
                CtaPrimaryUrl = "#projects",
                CtaSecondaryText = "Contact Me",
                CtaSecondaryUrl = "#contact",
                TypewriterTexts = "Full Stack Developer,ASP.NET Core Expert,Angular Developer,Cloud Architect",
                BackgroundType = "gradient",
                ShowParticles = true,
                ShowScrollIndicator = true
            });
        }

        if (!context.Abouts.IgnoreQueryFilters().Any())
        {
            context.Abouts.Add(new About
            {
                FullName = "John Developer",
                Title = "Senior Full Stack Developer",
                Subtitle = "Building Digital Experiences",
                Bio = "Passionate full stack developer with 5+ years of experience building enterprise-grade web applications using ASP.NET Core, Angular, and Azure cloud services.",
                ShortBio = "Full Stack Developer | .NET | Angular | Azure",
                Email = "john@portfolio.com",
                Phone = "+1 (555) 000-0000",
                Location = "New York, USA",
                Nationality = "American",
                Languages = "English, Spanish",
                YearsOfExperience = 5,
                ProjectsCompleted = 30,
                HappyClients = 20,
                IsAvailableForWork = true,
                AvailabilityStatus = "Open to opportunities",
                MetaTitle = "John Developer - Full Stack Developer Portfolio",
                MetaDescription = "Portfolio of John Developer, a Senior Full Stack Developer specializing in ASP.NET Core and Angular."
            });
        }

        if (!context.Skills.IgnoreQueryFilters().Any())
        {
            var skills = new List<Skill>
            {
                new() { Name = "ASP.NET Core", Category = SkillCategory.Backend, Percentage = 95, IconClass = "devicon-dotnetcore-plain", Color = "#512BD4", DisplayOrder = 1, IsFeatured = true, YearsOfExperience = 5 },
                new() { Name = "C#", Category = SkillCategory.Backend, Percentage = 95, IconClass = "devicon-csharp-plain", Color = "#239120", DisplayOrder = 2, IsFeatured = true, YearsOfExperience = 5 },
                new() { Name = "Angular", Category = SkillCategory.Frontend, Percentage = 90, IconClass = "devicon-angularjs-plain", Color = "#DD0031", DisplayOrder = 3, IsFeatured = true, YearsOfExperience = 4 },
                new() { Name = "TypeScript", Category = SkillCategory.Frontend, Percentage = 88, IconClass = "devicon-typescript-plain", Color = "#3178C6", DisplayOrder = 4, IsFeatured = true, YearsOfExperience = 4 },
                new() { Name = "SQL Server", Category = SkillCategory.Database, Percentage = 90, IconClass = "devicon-microsoftsqlserver-plain", Color = "#CC2927", DisplayOrder = 5, IsFeatured = true, YearsOfExperience = 5 },
                new() { Name = "Azure", Category = SkillCategory.Cloud, Percentage = 80, IconClass = "devicon-azure-plain", Color = "#0078D4", DisplayOrder = 6, IsFeatured = true, YearsOfExperience = 3 },
                new() { Name = "Docker", Category = SkillCategory.DevOps, Percentage = 75, IconClass = "devicon-docker-plain", Color = "#2496ED", DisplayOrder = 7, YearsOfExperience = 2 },
                new() { Name = "Git", Category = SkillCategory.Tools, Percentage = 92, IconClass = "devicon-git-plain", Color = "#F05032", DisplayOrder = 8, YearsOfExperience = 5 }
            };
            context.Skills.AddRange(skills);
        }

        if (!context.SocialLinks.IgnoreQueryFilters().Any())
        {
            var links = new List<SocialLink>
            {
                new() { Platform = "GitHub", Url = "https://github.com/johndeveloper", IconClass = "fab fa-github", Color = "#333", Username = "johndeveloper", DisplayOrder = 1 },
                new() { Platform = "LinkedIn", Url = "https://linkedin.com/in/johndeveloper", IconClass = "fab fa-linkedin", Color = "#0A66C2", Username = "johndeveloper", DisplayOrder = 2 },
                new() { Platform = "Twitter", Url = "https://twitter.com/johndeveloper", IconClass = "fab fa-twitter", Color = "#1DA1F2", Username = "johndeveloper", DisplayOrder = 3 },
                new() { Platform = "LeetCode", Url = "https://leetcode.com/johndeveloper", IconClass = "fas fa-code", Color = "#FFA116", Username = "johndeveloper", DisplayOrder = 4 }
            };
            context.SocialLinks.AddRange(links);
        }

        await context.SaveChangesAsync();
    }
}
