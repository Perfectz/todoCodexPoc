# todoCodexPoc

This repo demonstrates a simple .NET console and MAUI application with an Entity Framework Core SQLite backend.

## Local prerequisites

- .NET SDK 8.0 installed locally. The container used in Codex testing does not include the SDK and has no internet access.

## Building and running

Build and run the console app locally with:

```bash
dotnet restore
dotnet run --project src/TaskTracker.Cli/TaskTracker.csproj [verb] [arguments]
```

Running with no verb lists all tasks.

### CLI verbs

- `add "task description"` &ndash; add a new task. Natural language due dates are parsed automatically.
- `voice` &ndash; transcribe speech using `IVoiceToText` and create a task from the result.

## Running tests

The tests live under `tests/TaskTracker.Tests`. Execute them locally with:

```bash
dotnet restore
dotnet test
```

## EF Core migrations

The MAUI app relies on Entity Framework Core. After installing the SDK you can create the initial database and schema with:

```bash
cd src/TaskTracker.Mobile
dotnet ef migrations add Init
dotnet ef database update
```

## Why no `dotnet` commands in container prompts?

The container environment lacks the .NET SDK and has no network access. Any `dotnet` commands will fail here, so migrations and tests must be executed on a machine where the SDK is available.

## Building the mobile app

The MAUI project lives in `src/TaskTracker.Mobile`.

## Project layout

```
src/
  TaskTracker.Core/   # shared domain models and services
  TaskTracker.Cli/    # console interface
  TaskTracker.Mobile/ # MAUI mobile app
tests/
  TaskTracker.Tests/  # xUnit tests
```

Open `TodoCodexPoc.sln` in your IDE to work with all projects at once.

### Local prerequisites

- Install the .NET 8 SDK and the MAUI workloads.

### Android

Build the Android project with:

```bash
dotnet restore src/TaskTracker.Mobile/TaskTracker.Mobile.csproj
dotnet build src/TaskTracker.Mobile/TaskTracker.Mobile.csproj -f net8.0-android
```

Install the resulting APK on a device or emulator using `adb install`.

### iOS

Build for iOS with:

```bash
dotnet restore src/TaskTracker.Mobile/TaskTracker.Mobile.csproj
dotnet build src/TaskTracker.Mobile/TaskTracker.Mobile.csproj -f net8.0-ios
```

Run the app through Xcode on a simulator or connected device.
