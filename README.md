# Winterflood

This repository contains two projects:

- `Winterflood.Server` — ASP.NET Core Web API targeting .NET 10
- `winterflood.client` — Angular 18 client application

Running the API can also launch the Angular frontend via the ASP.NET Core SPA middleware.

## Prerequisites

- **.NET 10 SDK** — install from https://dotnet.microsoft.com/en-us/download/dotnet/10.0
  - Verify: `dotnet --version` (should report a `10.x` version)
- **Node.js (LTS)** — Angular 18 works well with Node 18 or newer (Node 18/20 recommended)
  - Install from https://nodejs.org/
  - Verify: `node --version` and `npm --version`
- **(Optional)**: `npm install -g @angular/cli` for convenience (not required — project uses local CLI)

## Development HTTPS certificate

To run the client behind the ASP.NET Core dev certificate, trust the dev cert on your machine:

```powershell
dotnet dev-certs https --trust
```

On Windows this may prompt for elevation.

## Initial setup (first time)

After cloning the repo, install the client dependencies:

```bash
cd winterflood.client
npm install
```

## How to run (development)

You have two common options to run the projects during development.

Option A — Run from Visual Studio

 - Set the `Winterflood.Server` project as the startup project and run (F5).
 - The server will start and the SPA middleware will launch the Angular dev server automatically. The browser will open to the API URL (for example `https://localhost:7030`) and serve the frontend.

Option B — Run from terminals (manual)

1. Start the API:

```powershell
cd Winterflood.Server
dotnet restore
dotnet run
```

2. Start the client (in a separate terminal):

```powershell
cd winterflood.client
npm run start:windows   # on Windows
# or
npm run start:default   # on macOS/Linux
```

Notes:

- The `prestart` script in `winterflood.client/package.json` runs `aspnetcore-https` to wire up the dev certificate used by the Angular dev server.
- `npm start` uses `run-script-os` to select the platform-specific script; you can run `npm run start:windows` or `npm run start:default` directly.

