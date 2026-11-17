using Microsoft.EntityFrameworkCore;

namespace ScamCheck.Api.Data;

public static class SeedData
{
    public static async Task InitializeAsync(AppDbContext db)
    {
        if (!await db.MaliciousDomains.AnyAsync())
        {
            var domains = new[] {
                ("takealot-sa.xyz","phishing","Takealot"),("takealot-delivery.top","fake-store","Takealot"),("takealot-voucher.click","fake-store","Takealot"),("takealot-order-confirm.lol","phishing","Takealot"),("takealot-specials.shop","fake-store","Takealot"),("takealot-track-za.mom","fake-courier","Takealot"),("dstv-renewal.top","phishing","DStv"),("dstv-account-verify.xyz","phishing","DStv"),("dstv-premium-deal.click","fake-store","DStv"),("dstv-urgent-payment.lol","phishing","DStv"),("dstv-subscription.shop","phishing","DStv"),("sars-refund-check.lol","phishing","SARS"),("sars-tax-refund.xyz","phishing","SARS"),("sars-e-filing-verify.click","phishing","SARS"),("sars-refund-claim.top","phishing","SARS"),("sars-notice-mom.shop","phishing","SARS"),("saa-ticket-confirm.click","phishing","SAA"),("saa-flight-refund.top","phishing","SAA"),("flysaa-booking.xyz","phishing","SAA"),("saa-checkin-verify.lol","phishing","SAA"),("saa-special-fare.shop","fake-store","SAA"),("parcel-held-za.mom","fake-courier","Courier"),("parcel-fee-required.top","fake-courier","Courier"),("courier-delivery-sms.xyz","fake-courier","Courier"),("post-office-parcel.click","fake-courier","Courier"),("fastway-track-lol.shop","fake-courier","Fastway"),("dhl-customs-za.top","fake-courier","DHL"),("ram-delivery-confirm.xyz","fake-courier","RAM"),("courier-redelivery.mom","fake-courier","Courier"),("parcel-address-update.click","fake-courier","Courier"),("standardbank-secure.xyz","banking-trojan","Standard Bank"),("fnb-verification.top","banking-trojan","FNB"),("absa-security-check.lol","banking-trojan","Absa"),("nedbank-login.click","banking-trojan","Nedbank"),("capitec-alert.mom","banking-trojan","Capitec"),("mtn-banking-verify.shop","banking-trojan","MTN"),("vodacom-wallet.xyz","banking-trojan","Vodacom"),("telkom-payment.top","banking-trojan","Telkom"),("bank-update-cyou.cyou","banking-trojan","South African banks"),("secure-bank-login.shop","banking-trojan","South African banks")
            }.Select(x => new MaliciousDomain { Domain=x.Item1, Category=x.Item2, BrandImpersonated=x.Item3, AddedUtc=DateTime.UtcNow, SourceNote="ScamCheck research seed" });
            db.MaliciousDomains.AddRange(domains);
        }
        if (!await db.ScamCases.AnyAsync())
        {
            var cases = new[] {
                ("Your Takealot parcel is waiting","SMS","Takealot","A message says a small delivery fee is required before a parcel can be released.","Unexpected link\nUrgent payment request\nSender is not Takealot"),
                ("Takealot delivery address update","WhatsApp","Takealot","A fake agent asks for card details to correct a delivery address.","Unsolicited contact\nCard details requested\nShortened URL"),
                ("Fake DStv renewal discount","SMS","DStv","A discount offer leads to a lookalike payment page.","Too-good-to-be-true offer\nPressure to pay today\nLookalike domain"),
                ("DStv account suspension warning","Email","DStv","The recipient is told their subscription will stop unless they verify immediately.","Threat of suspension\nGeneric greeting\nLink does not use dstv.com"),
                ("SARS refund notification","SMS","SARS","A message promises a tax refund after a form is completed online.","SARS does not request card PINs\nUnexpected refund\nSuspicious domain"),
                ("Outstanding SARS eFiling notice","Email","SARS","A PDF and link claim an urgent tax balance is due.","Urgency\nUnexpected attachment\nRequests login details"),
                ("SAA ticket confirmation","WhatsApp","SAA","A fake booking confirmation asks the passenger to pay a verification fee.","No booking made\nFee for confirmation\nUntrusted number"),
                ("Fake SAA flight refund","Email","SAA","A refund form collects identity and banking information.","Requests banking details\nGeneric sender\nUnusual link"),
                ("Parcel held at customs","SMS","Courier","A parcel is allegedly held and requires a small customs payment.","Tiny fee request\nNo tracking context\nLink to unknown host"),
                ("Redelivery charge","SMS","Courier","The recipient must pay to schedule another delivery attempt.","Unexpected delivery\nCountdown language\nPayment link"),
                ("Fastway tracking update","Email","Fastway","A tracking message directs the user to install an app.","Executable download\nUnusual sender\nNo expected parcel"),
                ("Post Office parcel fee","WhatsApp","Courier","A contact forwards a parcel payment link.","Forwarded message\nNo tracking number\nUrgent fee"),
                ("Hi Mom, I lost my phone","WhatsApp","None","An attacker impersonates a family member and asks for an immediate transfer.","New number\nEmotional pressure\nRequests instant payment"),
                ("Hi Dad, please pay this bill","WhatsApp","None","A supposed child asks a parent to pay a bill before calling.","Refuses a voice call\nUrgency\nNew bank account"),
                ("Friend stranded overseas","WhatsApp","None","An account takeover is used to request emergency funds.","Odd wording\nNo verification\nInternational transfer"),
                ("Bank verification call","Call","Standard Bank","A caller says fraud was detected and asks for an OTP.","Requests OTP\nCaller ID spoofing\nCreates panic"),
                ("FNB security desk","Call","FNB","A fake agent asks the customer to move funds to a safe account.","Safe account story\nRequests PIN\nUnsolicited call"),
                ("Absa account unlock","SMS","Absa","A link claims the account will be frozen unless verified.","Threat\nLogin link\nNo official app notification"),
                ("Nedbank reward claim","Email","Nedbank","A reward requires a deposit and card verification.","Prize bait\nCard details\nUnexpected email"),
                ("Capitec app update","SMS","Capitec","A fake update page distributes a malicious Android package.","APK download\nUnknown domain\nUrgent update"),
                ("MTN points expiry","SMS","MTN","Points allegedly expire unless the recipient logs in.","Lure of reward\nLogin page\nShort deadline"),
                ("Vodacom data prize","WhatsApp","Vodacom","A competition message collects identity information.","Forwarded competition\nPersonal data request\nNo terms"),
                ("Telkom account credit","Email","Telkom","A credit notice links to a fake customer portal.","Unexpected credit\nLookalike portal\nPassword request"),
                ("Invoice malware attachment","Email","None","A fake invoice contains a link to a malicious download.","Unexpected invoice\nMacro or executable\nSender mismatch"),
                ("Banking trojan installation","SMS","South African banks","A security update link installs malware that targets banking sessions.","APK file\nUnknown source\nSecurity scare")
            }.Select(x => new ScamCase { Title=x.Item1, Channel=x.Item2, BrandImpersonated=x.Item3, Description=x.Item4, RedFlags=x.Item5, AddedUtc=DateTime.UtcNow });
            db.ScamCases.AddRange(cases);
        }
        if (!await db.ChecklistQuestions.AnyAsync())
        {
            var questions = new[] {
                ("Are your backups connected to the same network as your production machines?",3,"Segment backup networks and restrict access with firewall rules."),
                ("Can backups be deleted or encrypted using the same admin credentials as production?",3,"Use separate privileged accounts, MFA, and immutable retention policies."),
                ("Have you tested a full restore in the last 90 days?",2,"Schedule a documented restore drill at least quarterly."),
                ("Do you have at least one offline or immutable copy of your backups?",3,"Keep an offline, air-gapped, or object-locked copy with tested retention."),
                ("Would you know within 24 hours if a backup failed silently?",2,"Configure independent monitoring and alerts for backup jobs and storage."),
                ("Are backup logs reviewed by someone other than the person who set them up?",1,"Assign a second reviewer and retain tamper-resistant audit logs."),
                ("Is your backup software patched and up to date?",1,"Patch backup servers promptly and track them in vulnerability management."),
                ("Do you have a written, tested disaster recovery plan?",2,"Document recovery priorities, owners, contacts, and run a tabletop exercise.")
            }.Select((x,i) => new ChecklistQuestion { Text=x.Item1, Weight=x.Item2, FixAdvice=x.Item3, Order=i+1 });
            db.ChecklistQuestions.AddRange(questions);
        }
        await db.SaveChangesAsync();
    }
}
