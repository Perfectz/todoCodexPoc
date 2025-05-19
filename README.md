# todoCodexPoc

This repo demonstrates a simple .NET MAUI application with an Entity Framework Core SQLite backend.

## EF Core migrations

The `dotnet` CLI is required to run EF Core commands. If `dotnet` is installed, you can create the initial database with:

```bash
cd TodoMauiApp
 dotnet ef migrations add Init
 dotnet ef database update
```

In the Codex test environment `dotnet` is not available, so these commands fail. They will work on a local machine that has the .NET SDK installed.
