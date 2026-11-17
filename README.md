# 🛡️ ScamCheck

**A mobile-first, privacy-focused web application designed to protect South African citizens from phishing and help SMMEs secure their data.**

🔗 **[View the Live Application Here](https://scamcheck-jq2w.onrender.com/)**

---

## 📖 Overview

South Africa has one of the highest rates of cybercrime on the continent. Criminal syndicates frequently impersonate trusted local brands (such as Takealot, SARS, DStv, and local banks) via SMS and WhatsApp. Simultaneously, Small, Medium, and Micro Enterprises (SMMEs) often assume they are protected by backups, unaware that modern ransomware specifically targets connected backup drives.

**ScamCheck** solves these problems with a zero-login, privacy-first approach. It offers:
1. **Instant Link Verification:** Check suspicious URLs against a blacklist of known local phishing domains and heuristic risk factors.
2. **SMME Backup Assessment:** An interactive 8-question risk calculator that scores a business's backup security and provides actionable remediation steps.
3. **Scam Case Library:** A searchable database of real South African scam campaigns with details on red flags. 

---

## 🛠️ Technology Stack

*   **Backend Framework:** C# .NET 8 (ASP.NET Core MVC)
*   **Database:** SQLite (Serverless, file-based)
*   **ORM:** Entity Framework Core (EF Core)
*   **Frontend:** HTML5, CSS3, Vanilla JavaScript, Razor Views
*   **PWA:** Service Workers & Web App Manifest (Installable on mobile devices, offline capable)
*   **DevOps & Hosting:** Git, GitHub, Docker, Render

---

## 🏗️ Architecture

The application follows a standard **Model-View-Controller (MVC)** architecture:
*   **Controllers:** `HomeController` manages page routing, while `ScamApiController` serves as a RESTful API for asynchronous JavaScript `fetch` requests (calculating scores, checking links, querying the database).
*   **Views:** Razor (`.cshtml`) pages styled with a custom, mobile-first CSS design.
*   **Models:** Entity Framework Core entities (`MaliciousDomain`, `ScamCase`, `ChecklistQuestion`).
*   **Deployment:** The application is containerized using a `Dockerfile` (based on the .NET 8 Linux images) and deployed to the Render cloud platform.

---

## 🚀 Key Features

*   **"Never Say Safe" Policy:** Automated checks cannot guarantee a link is safe. The app returns "Malicious," "Suspicious," or "Unknown" (with a legal warning), avoiding false confidence and legal liability.
*   **POPIA Compliant:** No user accounts, no tracking cookies, and no collection of Personal Identifiable Information (PII). 
*   **Offline Capable:** Using PWA Service Workers, the app shell and the scam case library are cached, allowing users to search for known scams even when they lose network connection.
*   **Data-Driven:** The database initializes automatically on startup (`EnsureCreated`) and is seeded with South African-specific scam data via an idempotent `SeedData` class.

---

## 💻 Running Locally

To run this project locally, ensure you have the **.NET 8 SDK** installed.

1. Clone the repository:
git clone https://github.com/preciousmchunu423/ScamCheck.git

2. Navigate to the project directory:
cd ScamCheck/ScamCheck

3. Run the application:
dotnet run

Open your browser and navigate to https://localhost:5001 (or the port specified in the terminal).

## 📦 Deployment (Docker)
The project includes a Dockerfile for easy cloud deployment. The production container uses the .NET 8 ASP.NET Core runtime image.

Note: When deploying to a platform with an ephemeral file system (like Render's free tier), the SQLite database will reset to the seed data upon server restart.

## 📄 Documentation
For a full breakdown of the project architecture, UML diagrams, and testing results, please refer to the ScamCheck_Project_Report.pdf located in this repository.

