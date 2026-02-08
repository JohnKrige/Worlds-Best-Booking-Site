# Winterflood

## Project Structure

This repository contains two projects:

- **Winterflood.Server** — ASP.NET Core Web API targeting .NET 10
- **winterflood.client** — Angular 18 client application

Running the ASP.NET Server project will automatically start the Angular client application via SPA middleware integration.

**Prerequisites**

- **.NET 10 SDK** — install from https://dotnet.microsoft.com/en-us/download/dotnet/10.0
  - Verify: `dotnet --version` (should report a `10.x` version)
- **Node.js (LTS)** — Angular 18 is compatible with Node 18 or newer (Node 18/20 recommended)
  - Install from https://nodejs.org/
  - Verify: `node --version` and `npm --version`
- **(Optional)** Install Angular CLI globally for convenience: `npm install -g @angular/cli` (not required — project uses local CLI)

**Development HTTPS certificate (recommended)**

- To run the client with the development ASP.NET Core HTTPS certificate, trust the dev cert on your machine:

  ```powershell
  dotnet dev-certs https --trust
  ```

  On Windows this may prompt for elevation.

  ## 🔧 Initial Setup (First Time Only)

After cloning the repository, install the Angular client dependencies:

```bash
cd winterflood.client
npm install

# 🚀 Running the Project

This solution is configured so that **running the ASP.NET Core API automatically starts the Angular frontend**.

The Angular application is hosted using the ASP.NET Core SPA middleware. When the API starts, it:

1. Launches the Angular development server using `npm start`
2. Waits for the Angular build to complete
3. Proxies frontend requests through the API server

This means you only need to start **one project**.

---

## ▶️ How to Run the Application

### Option 1 — Using Visual Studio

1. Set the **API project** as the startup project.
2. Press **F5** or click **Run**.
3. The browser will open at the API URL (e.g. `https://localhost:7030`).
4. The Angular app will automatically compile and load.

No separate Angular startup is required.
