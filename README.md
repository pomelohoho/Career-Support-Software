# Career Support Software

**A job support system for international students navigating H-1B sponsorship.**

---

## Table of Contents

1. [Overview](#overview)  
2. [Features](#features)  
3. [Architecture & Tech Stack](#architecture--tech-stack)  
4. [Data Flow](#data-flow)  
5. [Security & Privacy](#security--privacy)  
6. [Getting Started](#getting-started)  
   1. [Prerequisites](#prerequisites)  
   2. [Backend Setup (ASP.NET Core)](#backend-setup-aspnet-core)  
   3. [Frontend Setup (React + Vite)](#frontend-setup-react--vite)  
7. [Usage](#usage)  
8. [Future Enhancements](#future-enhancements)  
9. [Work Visa System Analysis](#work-visa-system-analysis)  
10. [Ethical & Sociotechnical Considerations](#ethical--sociotechnical-considerations)  
11. [References](#references)  

---

## Overview

International students on F-1 visas often struggle to find H-1B–sponsoring employers. This project aggregates job postings from approved sources, lets users save filter preferences, and delivers a real-time, paginated, searchable feed of roles that match their H-1B sponsorship needs—while strongly protecting sensitive user data.

---

## Features

- **User Authentication & Authorization**  
  - ASP.NET Identity (JWT + refresh tokens)  
  - Role-based access (Student, Admin)

- **Preference Dashboard**  
  - Save filters (title, location, salary floor, sponsorship status)  
  - Encrypted at-rest & end-to-end for sensitive fields

- **Job Feed**  
  - Hourly ingestion of postings (past 7 days) from RapidAPI  
  - Deduplication, normalization, persistence in SQL Server  
  - React-driven UI with pagination, filtering, “Sponsored” highlight  

- **Admin Tools**  
  - Manual ingestion endpoint  
  - Database health & sample data endpoints  

---

## Architecture & Tech Stack

| Layer              | Technology                              |
|--------------------|-----------------------------------------|
| **Frontend**       | React 19 + Vite, Material-UI v5, Recharts |
| **Backend**        | .NET 9 Web API (C#), ASP.NET Identity   |
| **Database**       | SQL Server Express 2022 (EF Core)       |
| **Ingestion**      | `JobService` hourly hosted worker       |
| **Hosting (dev)**  | Local IIS/Kestrel + SQL Server Express  |

---

## Data Flow

1. **Ingestion**  
   - Every hour, calls RapidAPI:  
     `GET /fantastic-jobs-fantastic-jobs-default/api/linkedin-job-search-api/active-jb-7d?...`
   - Deserialize → normalize → dedupe → save to `JobPostings` table.

2. **User Interaction**  
   - Student logs in → preferences loaded (client-encrypted)  
   - UI fetches `GET /api/jobs?limit=100&includeNonSponsors=true`  
   - Server applies filters → returns JSON page of matching jobs

3. **Preference Storage**  
   - Preferences fields encrypted client-side with per-user key  
   - Stored ciphertext in `UserPreferences` table

---

## Security & Privacy

- **Transport**: HTTPS 1.3 enforced (`UseHttpsRedirection`).  
- **Auth**: JWT (15 min) + 7-day refresh tokens; BCrypt-hashed passwords.  
- **Data Minimization**: Only email + hashed password + encrypted preferences saved.  
- **E2EE**: Sensitive filter settings encrypted in browser; server stores only ciphertext.  
- **At-Rest**: Transparent Data Encryption (TDE) & SQL Always-Encrypted for critical columns.  
- **Least Privilege**: Role policies prevent cross-user access.  
- **Secrets Management**: Environment variables (dev) / Azure Key Vault (prod).

---

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)  
- [Node.js 18+ & npm](https://nodejs.org/)  
- SQL Server Express 2022 (or equivalent)  
- A RapidAPI key for LinkedIn Job Search  

### Backend Setup (ASP.NET Core)

1. **Clone & configure**  
   ```bash
   git clone https://github.com/pomelohoho/Career-Support-Software.git
   cd Career-Support-Software/Server
  ``

2. **appsettings.json**

   ```jsonc
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=.;Database=JobSupportSystem;Trusted_Connection=True;"
     },
     "Jwt": {
       "Key": "<your-jwt-secret>",
       "Issuer": "CareerSupportAPI",
       "Audience": "CareerSupportClient",
       "ExpirationMinutes": 15,
       "RefreshExpirationDays": 7
     },
     "RapidAPI": {
       "Key": "<your-rapidapi-key>"
     }
   }
   ```
3. **Database Migration & Seeding**

   ```bash
   cd CareerSupportSoftware.Server
   dotnet ef database update
   dotnet run
   ```

   * This will apply migrations, seed roles & admin user.

### Frontend Setup (React + Vite)

1. **Install & start**

   ```bash
   cd CareerSupportSoftware.Client
   npm install
   npm run dev
   ```
2. **Env** (optional)

   ```env
   VITE_API_BASE_URL=https://localhost:5001/api
   ```
3. **Open**

   * Frontend: `http://localhost:5173`
   * Backend: `https://localhost:5001`

---

## Usage

1. **Register** as a new student or log in with the seeded admin:

   ```
   Email: admin@careersupport.com
   Pass:  AdminPassword123!
   ```
2. **Set your preferences** (title filters, salary floor, sponsorship).
3. **Browse** paginated job listings—sponsored roles are highlighted.
4. **Log out** and log back in; your filters persist (and remain encrypted).

---

## Future Enhancements

* **H-1B Sponsorship Guide**: Plain-language overview + filing checklist for employers.
* **Activity Analytics**: Recharts visualizations of which filters yield the most matches.
* **OAuth 2.0** social login (Google, Microsoft).
* **Azure Deployment**: Docker + GitHub Actions with blue/green, vulnerability scanning.
* **Row-Level Security** in Azure SQL for per-user data isolation.

---

## Work Visa System Analysis

For detailed background on H-1B rules, processes, and stakeholder perspectives, see the [analysis section in our paper](https://github.com/pomelohoho/Career-Support-Software/blob/05a3000ad1e5f3415b1033507148ba4c4483f94c/Individual%20Project%20Final%20Paper.pdf) (or Section 9 below).

---

## Ethical & Sociotechnical Considerations

* **End-User Safety**: E2EE for preferences to protect vulnerable students.
* **Discrimination Risk**: Neutral job presentation; salary-range validation for pay-transparency compliance.
* **Bias Auditing**: Regular, open audits of ranking logic to prevent prestige or location bias.
* **Transparency**: Open code, clear methodology, public privacy notices.

---

## References

1. Akin Gump Strauss Hauer & Feld LLP. *California Expands Definition of Sensitive Personal Information Covered Under CCPA*. AG Data Dive (2023).
2. American Immigration Council. *The H-1B Visa Program: A Fact Sheet* (2023).
3. Beaudry, D. *Employment Visas: Why Won’t Companies Sponsor an H1B?* PowerTies (2025).
4. Franssen, M. & Kroes, P. “Sociotechnical Systems.” *A Companion to the Philosophy of Technology* (2009).
5. Malki, L. M. et al. “Privacy Practices of Female mHealth Apps in a Post-Roe World.” CHI ’24 (2024).
6. Pinch, T. J. & Bijker, W. E. “The Social Construction of Facts and Artefacts.” *Social Studies of Science* (1984).
7. Song, Q. et al. “Collective Privacy Sensemaking…Post Roe v. Wade.” CHI ’24 Proceedings (2024).
8. Tenner, E. *Why Things Bite Back: Technology and the Revenge of Unintended Consequences* (1996).

---

