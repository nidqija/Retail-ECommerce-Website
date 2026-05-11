# 🛒 RetailECommerce Storefront
 
A robust C# ASP.NET Core MVC application demonstrating Attribute Routing, Factory Method patterns, and modern storefront architecture.
 
## 🛠 Getting Started
 
If you are importing this project for the first time, follow these steps to set up your local development environment.
 
### 1. Clone & Fetch
 
To get the code and ensure you have the latest updates from the main branch:
 
```bash
# Clone the repository
git clone https://github.com/YourUsername/RetailECommerce.git
 
# Navigate to the project root
cd RetailECommerce
 
# Fetch the latest metadata and pull changes
git fetch origin
git pull origin main
```
 
### 2. Install Dependencies (NuGet Restore)
 
In .NET, dependencies are tracked in the .csproj file. You do not need a requirements.txt. Run the following to download all required NuGet packages:
 
```bash
dotnet restore
```
 
### 3. Setup Development Certificates
 
To run the project over HTTPS without browser security warnings, trust the .NET development certificates:
 
```bash
dotnet dev-certs https --trust
```
 
### 4. Setup Database Migrations
 
If the project uses Entity Framework Core, apply pending database migrations:
 
```bash
# Apply migrations to create/update the database
dotnet ef database update
```
 
**Optional:** If you need to create a new migration after model changes:
 
```bash
# Create a new migration
dotnet ef migrations add MigrationName
 
# Remove the last migration (if needed)
dotnet ef migrations remove
 
# List all migrations
dotnet ef migrations list
```
 
### 5. Run the Project
 
Launch the application using the .NET CLI:
 
```bash
dotnet run
```
OR 
```bash
dotnet watch run
```
**Note:** The application will typically listen on `http://localhost:5001`. Check the console output for the specific port.
 
## 📂 Project Overview
 
| Directory | Responsibility |
|-----------|---|
| **Controllers** | Handles incoming requests using Attribute Routing. |
| **Models** | Contains data schemas and ViewModels. |
| **Views** | Razor (.cshtml) files for UI rendering. |
| **Services** | Business logic and Factory Method implementations. |
| **wwwroot** | Static files (Bootstrap, CSS, JS, and Images). |
 
## 📝 Developer Notes
 
- **Routing:** This project uses `[Route("path")]` attributes on controllers for SEO-friendly URLs.
- **Factory Method:** Used in page rendering to dynamically select view templates based on product categories.
- **Git Ignore:** The `bin/`, `obj/`, and `appsettings.Development.json` files are ignored to keep the repository clean and secure.

## 🆘 Troubleshooting
 
If the project does not compile or packages seem broken, try a hard clean:
 
```bash
# Remove temporary build artifacts
dotnet clean
 
# Force a rebuild
dotnet build
```