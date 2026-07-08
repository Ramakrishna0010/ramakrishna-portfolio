namespace PortfolioApp.Domain.Enums;

public enum SkillCategory
{
    Frontend = 1,
    Backend = 2,
    Database = 3,
    DevOps = 4,
    Cloud = 5,
    Mobile = 6,
    Tools = 7,
    SoftSkills = 8,
    Other = 9
}

public enum ProjectStatus
{
    InProgress = 1,
    Completed = 2,
    OnHold = 3,
    Archived = 4
}

public enum BlogStatus
{
    Draft = 1,
    Published = 2,
    Archived = 3
}

public enum ContactStatus
{
    New = 1,
    Read = 2,
    Replied = 3,
    Archived = 4
}

public enum UserRole
{
    Admin = 1,
    Visitor = 2
}

public enum ServiceType
{
    WebDevelopment = 1,
    MobileDevelopment = 2,
    CloudSolutions = 3,
    Consulting = 4,
    UIUXDesign = 5,
    Other = 6
}
