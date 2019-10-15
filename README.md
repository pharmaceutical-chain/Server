# Server


## Prerequisites

- (Recommended) Visual Studio 2017+ with .NET Core workflow.
- ASP.NET Core 2.2+

## Getting Started 

Restore and install packages.
```
dotnet restore
```

Migrate the database.
```
dotnet ef database update
```

Run the server.
```
dotnet run
```

Providing the server is up and running at `https://localhost:5001/`. All available APIs should now be available at `https://localhost:5001/swagger.`
