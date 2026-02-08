# Winterflood

This repository contains two projects:

- `Winterflood.Server` — ASP.NET server targeting .NET 10
- `winterflood.client` — Angular 18 client app

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

**Run the server**

1. Open a terminal in the solution root and change to the server folder:

   ```powershell
   cd Winterflood.Server
   ```

2. Restore/build and run:

   ```powershell
   dotnet restore
   dotnet run
   ```

3. (Optional) For live-reload during development use:

   ```powershell
   dotnet watch run
   ```

**Run the client (Angular)**

1. Open a terminal and change to the client folder:

   ```powershell
   cd winterflood.client
   ```

2. Install dependencies and start the dev server (on Windows):

   ```powershell
   npm install
   npm run start:windows
   ```

   On macOS/Linux use:

   ```bash
   npm install
   npm run start:default
   ```

Notes:

- The `prestart` script runs `aspnetcore-https` to wire up the dev certificate used by the Angular dev server. If you see certificate errors, ensure the dev cert is trusted (see above).
- `npm start` in this project uses `run-script-os` to choose the correct platform script; you can call `npm run start:windows` or `npm run start:default` directly.

**Build/Production**

- Server: `dotnet publish -c Release`
- Client: `npm run build` (produces dist in the client project)

**Troubleshooting**

- If ports conflict, check startup output for the port numbers and adjust or stop conflicting services.
- If `dotnet` or `node` commands are not found, confirm the SDKs are installed and the PATH is refreshed.

If you want, I can add small scripts or VS Code launch tasks to automate these commands.
