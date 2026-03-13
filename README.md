<!-- Improved compatibility of back to top link: See: https://github.com/othneildrew/Best-README-Template/pull/73 -->

<a name="readme-top"></a>

<!-- PROJECT SHIELDS -->
[![MIT License][license-shield]][license-url]
[![LinkedIn][linkedin-shield]][linkedin-url]

<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="https://github.com/gustmrg/bitfinance">
    <img src="/assets/logo.png" alt="" height="100" >
  </a>

<h3 align="center">BitFinance</h3>
  <p align="center">
    Simple and effective financial budget management
    <br />
</div>

<!-- TABLE OF CONTENTS -->
<details>
  <summary>Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About the Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#license">License</a></li>
  </ol>
</details>

<!-- ABOUT THE PROJECT -->

## About the Project

BitFinance is a multi-tenant financial management application designed to simplify tracking and managing budgets for families and organizations. It provides a clean API that allows users to record bills, track expenses, manage members within an organization, and control access through a subscription-based plan system.

Key capabilities include:
- **Organizations** — create and manage shared financial workspaces for families or teams
- **Bills** — track recurring payments and one-off bills with due dates, categories, and document attachments
- **Expenses** — record and categorize spending with full audit history
- **File Attachments** — attach invoices or receipts to bills and expenses, stored locally (MinIO) or in S3-compatible storage
- **Subscription Plans** — Free, Basic, and Premium tiers with plan-based entitlements (limits on bills, expenses, uploads, and invitations)
- **Secure Authentication** — JWT access tokens with refresh token rotation, HTTP-only cookies, and multi-device session management

<p align="right">(<a href="#readme-top">back to top</a>)</p>

### Built With

[![.NET][dotnet]][dotnet-url] [![PostgreSQL][Postgresql.org]][Postgresql-url] [![Redis][Redis]][Redis-url] [![Docker][Docker]][Docker-url]

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- GETTING STARTED -->

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/) and Docker Compose

### Installation

1. Clone the repository
   ```sh
   git clone https://github.com/gustmrg/bitfinance-backend.git
   cd bitfinance-backend
   ```

2. Copy the environment file and fill in your values
   ```sh
   cp .env.example .env
   ```

3. Start the infrastructure (PostgreSQL, Redis, MinIO)
   ```sh
   docker compose up -d
   ```

4. Apply database migrations
   ```sh
   dotnet ef database update --project src/BitFinance.Data --startup-project src/BitFinance.API
   ```

5. Run the API
   ```sh
   dotnet run --project src/BitFinance.API
   ```

The API will be available at `https://localhost:8080` (see console output for the exact port).

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- ROADMAP -->

## Roadmap

- [x] Authentication & authorization (JWT + refresh token rotation)
- [x] Bills management (create, update, delete)
- [x] Expenses management (create, update, delete)
- [x] Organization/family management (create, update, delete)
- [x] Member invitations via invite link
- [x] Docker containers (development & production)
- [x] File attachments (invoices, receipts) for bills and expenses
- [x] Local file storage with MinIO
- [x] Subscription plans (Free, Basic, Premium)
- [x] Plan-based entitlements (feature limits per tier)
- [x] Cloud file storage on AWS S3
- [ ] Due date notifications
- [ ] Dashboard analytics

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- LICENSE -->

## License

Distributed under the MIT License. See `LICENSE.md` for more information.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- MARKDOWN LINKS & IMAGES -->

[license-shield]: https://img.shields.io/github/license/gustmrg/bitfinance.svg?style=for-the-badge
[license-url]: https://github.com/gustmrg/bitfinance/blob/main/LICENSE.md
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/gustmrg
[dotnet]: https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white
[dotnet-url]: https://dotnet.microsoft.com/
[Postgresql.org]: https://img.shields.io/badge/postgresql-4169E1?style=for-the-badge&logo=postgresql&logoColor=white
[Postgresql-url]: https://www.postgresql.org
[Redis]: https://img.shields.io/badge/Redis-DC382D?style=for-the-badge&logo=redis&logoColor=white
[Redis-url]: https://redis.io
[Docker]: https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white
[Docker-url]: https://www.docker.com
