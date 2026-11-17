using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using ScamCheck.Data;
using ScamCheck.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Data Source=scamcheck.db";
var sqlitePath = new SqliteConnectionStringBuilder(connectionString).DataSource;
if (!string.IsNullOrWhiteSpace(sqlitePath) && sqlitePath != ":memory:")
{
    var sqliteDirectory = Path.GetDirectoryName(Path.GetFullPath(sqlitePath));
    if (!string.IsNullOrWhiteSpace(sqliteDirectory)) Directory.CreateDirectory(sqliteDirectory);
}
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.EnsureCreatedAsync();
    var sourceNoteColumn = await db.Database
        .SqlQueryRaw<long>("SELECT COUNT(*) AS \"Value\" FROM pragma_table_info('MaliciousDomains') WHERE name = 'SourceNote'")
        .SingleAsync();
    if (sourceNoteColumn == 0)
        await db.Database.ExecuteSqlRawAsync("ALTER TABLE MaliciousDomains ADD COLUMN SourceNote TEXT NULL");
    await SeedData.InitializeAsync(db);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
