# Solution Guide

This repository contains a small .NET application along with unit tests.

## Running Tests Locally

The tests require the .NET SDK. Execute the following command from the repository root:

```bash
dotnet restore
dotnet test
```

This will build the projects and run all xUnit tests. Note that the Codex test environment does not have the `dotnet` CLI, so the command must be run on a local machine with the .NET SDK installed.
