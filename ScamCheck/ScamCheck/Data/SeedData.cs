using Microsoft.EntityFrameworkCore;
using ScamCheck.Models;

namespace ScamCheck.Data;

public static class SeedData
{
    public static async Task InitializeAsync(AppDbContext db)
    {
        var cases = new[]
        {
            new ScamCase { Title = "Fake Takealot delivery SMS", Channel = "SMS", BrandImpersonated = "Takealot", Description = "A message says a small delivery fee is required before a parcel can be released.", RedFlags = "Link does not end in takealot.com|Asks for payment on a non Takealot site|Uses urgency" },
            new ScamCase { Title = "Fake DStv renewal discount", Channel = "SMS", BrandImpersonated = "DStv", Description = "A discount offer leads to a lookalike payment page.", RedFlags = "Too good to be true offer|Pressure to pay today|Lookalike domain" },
            new ScamCase { Title = "SASSA grant on hold", Channel = "SMS", BrandImpersonated = "SASSA", Description = "A message claims your grant is on hold until you confirm your personal details.", RedFlags = "SASSA never asks for your PIN|Requests banking details through a link|Urgent deadline" },
            new ScamCase { Title = "Fake SRD R370 payment link", Channel = "SMS", BrandImpersonated = "SASSA", Description = "A fake SRD payment link promises a R370 grant after you complete a form.", RedFlags = "Unknown payment link|Asks for ID and banking details|SASSA applications use official channels" },
            new ScamCase { Title = "Wrong deposit reversal request", Channel = "WhatsApp", BrandImpersonated = "Capitec", Description = "Someone claims they paid money into your account by mistake and asks you to send it back.", RedFlags = "Pressure to refund immediately|Does not use an official bank process|Never spend or return funds before the bank confirms them" },
            new ScamCase { Title = "Wrong deposit reversal request", Channel = "WhatsApp", BrandImpersonated = "FNB", Description = "Someone claims they paid money into your account by mistake and asks you to send it back.", RedFlags = "Pressure to refund immediately|Does not use an official bank process|Never spend or return funds before the bank confirms them" },
            new ScamCase { Title = "Wrong deposit reversal request", Channel = "WhatsApp", BrandImpersonated = "Absa", Description = "Someone claims they paid money into your account by mistake and asks you to send it back.", RedFlags = "Pressure to refund immediately|Does not use an official bank process|Never spend or return funds before the bank confirms them" },
            new ScamCase { Title = "Wrong deposit reversal request", Channel = "WhatsApp", BrandImpersonated = "Nedbank", Description = "Someone claims they paid money into your account by mistake and asks you to send it back.", RedFlags = "Pressure to refund immediately|Does not use an official bank process|Never spend or return funds before the bank confirms them" },
            new ScamCase { Title = "Wrong deposit reversal request", Channel = "WhatsApp", BrandImpersonated = "Standard Bank", Description = "Someone claims they paid money into your account by mistake and asks you to send it back.", RedFlags = "Pressure to refund immediately|Does not use an official bank process|Never spend or return funds before the bank confirms them" },
            new ScamCase { Title = "Prepaid electricity token scam", Channel = "SMS", BrandImpersonated = "Eskom", Description = "A message warns that your meter will be blocked unless you buy a token from a linked vendor.", RedFlags = "Threatens immediate disconnection|Token seller is not an official channel|Requests payment to a personal account" },
            new ScamCase { Title = "Outstanding traffic fine payment", Channel = "SMS", BrandImpersonated = "Traffic fines", Description = "A message claims you have an outstanding fine and links to an urgent payment page.", RedFlags = "Official fines use AARTO or municipal channels|Unknown payment link|Threatens arrest or extra fees" },
            new ScamCase { Title = "Fake Sixty60 voucher", Channel = "WhatsApp", BrandImpersonated = "Checkers Sixty60", Description = "A survey or giveaway promises a R1000 grocery voucher after you share a link.", RedFlags = "Too-good-to-be-true voucher|Requests contact sharing|Lookalike website" },
            new ScamCase { Title = "Fake Shoprite gift card", Channel = "SMS", BrandImpersonated = "Shoprite", Description = "A message says you won a Shoprite gift card and must complete a survey to claim it.", RedFlags = "Unexpected prize|Survey collects personal information|Claim link is not shoprite.co.za" },
            new ScamCase { Title = "Fake Pick n Pay voucher", Channel = "WhatsApp", BrandImpersonated = "Pick n Pay", Description = "A forwarded promotion offers a gift card after you answer questions and share the message.", RedFlags = "Requires forwarding to contacts|Unknown link|No official competition terms" },
            new ScamCase { Title = "Fake Woolworths survey prize", Channel = "SMS", BrandImpersonated = "Woolworths", Description = "A survey link promises a shopping voucher but leads to a data collection page.", RedFlags = "Prize was not entered for|Requests card or login details|Lookalike domain" },
            new ScamCase { Title = "WhatsApp verification code scam", Channel = "WhatsApp", BrandImpersonated = "WhatsApp", Description = "A contact asks you to send the six-digit WhatsApp code that just arrived on your phone.", RedFlags = "The code is for your account only|A real contact will not need your code|Enable two-step verification" }
        };

        foreach (var scamCase in cases)
            if (!await db.ScamCases.AnyAsync(x => x.Title == scamCase.Title && x.BrandImpersonated == scamCase.BrandImpersonated))
                db.ScamCases.Add(scamCase);

        var domains = new[]
        {
            new MaliciousDomain { Domain = "takealot-sa.xyz", Category = "fake store", BrandImpersonated = "Takealot", SourceNote = "Demo domain from the design mock-up" }
        };
        foreach (var domain in domains)
            if (!await db.MaliciousDomains.AnyAsync(x => x.Domain == domain.Domain)) db.MaliciousDomains.Add(domain);

        var questions = new[]
        {
            new ChecklistQuestion { Order = 1, Question = "Are your backups connected to the same network as your work computers?", Weight = 3, FixAdvice = "Disconnect one backup copy from the network today." },
            new ChecklistQuestion { Order = 2, Question = "Can backups be deleted with the same admin password as your work computers?", Weight = 3, FixAdvice = "Use different credentials for backups." },
            new ChecklistQuestion { Order = 3, Question = "Have you tested a full restore in the last 90 days?", Weight = 2, FixAdvice = "Test a full restore this week." },
            new ChecklistQuestion { Order = 4, Question = "Do you have at least one offline or immutable copy of your backups?", Weight = 3, FixAdvice = "Create an offline or immutable backup copy." },
            new ChecklistQuestion { Order = 5, Question = "Would you know within 24 hours if a backup failed silently?", Weight = 2, FixAdvice = "Set up automated backup alerts." },
            new ChecklistQuestion { Order = 6, Question = "Are backup logs reviewed by someone other than the person who set them up?", Weight = 1, FixAdvice = "Have a second person review backup logs." },
            new ChecklistQuestion { Order = 7, Question = "Is your backup software patched and up to date?", Weight = 1, FixAdvice = "Update your backup software." },
            new ChecklistQuestion { Order = 8, Question = "Do you have a written, tested disaster recovery plan?", Weight = 2, FixAdvice = "Write and test a disaster recovery plan." }
        };
        foreach (var question in questions)
        {
            var existing = await db.ChecklistQuestions.FirstOrDefaultAsync(x => x.Order == question.Order);
            if (existing is null) db.ChecklistQuestions.Add(question);
            else
            {
                existing.Question = question.Question;
                existing.Weight = question.Weight;
                existing.FixAdvice = question.FixAdvice;
            }
        }

        await db.SaveChangesAsync();
    }
}
