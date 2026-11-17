using Microsoft.EntityFrameworkCore;
using ScamCheck.Models;

namespace ScamCheck.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<MaliciousDomain> MaliciousDomains => Set<MaliciousDomain>();
    public DbSet<ScamCase> ScamCases => Set<ScamCase>();
    public DbSet<ChecklistQuestion> ChecklistQuestions => Set<ChecklistQuestion>();
}
