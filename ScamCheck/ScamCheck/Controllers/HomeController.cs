using Microsoft.AspNetCore.Mvc;
using ScamCheck.Models;
using System.Diagnostics;
using ScamCheck.Data;
using Microsoft.EntityFrameworkCore;

namespace ScamCheck.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Cases(string? q, string? brand)
        {
            using var db = HttpContext.RequestServices.GetRequiredService<AppDbContext>();
            ViewBag.Brands = await db.ScamCases
                .Where(x => !string.IsNullOrWhiteSpace(x.BrandImpersonated) && x.BrandImpersonated != "None")
                .Select(x => x.BrandImpersonated!)
                .Distinct()
                .OrderBy(x => x)
                .ToListAsync();
            var cases = db.ScamCases.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(q)) cases = cases.Where(x => x.Title.Contains(q) || x.Description.Contains(q) || x.RedFlags.Contains(q));
            if (!string.IsNullOrWhiteSpace(brand) && brand != "All brands") cases = cases.Where(x => x.BrandImpersonated == brand);
            return View(await cases.OrderByDescending(x => x.AddedUtc).ToListAsync());
        }

        public IActionResult Backup() => View();

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
