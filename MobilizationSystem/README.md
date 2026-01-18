
Install .NET CLI:
https://learn.microsoft.com/en-us/ef/core/cli/dotnet#installing-the-tools

Migration & Database:
https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli#create-your-first-migration

.NET CLI:
dotnet ef migrations add InitialCreate
dotnet ef database update

Visual Studio:
Add-Migration InitialCreate
Update-Database
