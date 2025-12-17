# Releasing Volley .NET SDK

This guide explains how to release a new version of the Volley .NET SDK.

## Prerequisites

1. Ensure all tests pass:
   ```bash
   dotnet test
   ```

2. Update the version in `src/Volley/Volley.csproj`:
   ```xml
   <Version>1.0.0</Version>
   <PackageVersion>1.0.0</PackageVersion>
   ```

3. Update the version in `src/Volley/Version.cs`:
   ```csharp
   public const string SDKVersion = "1.0.0";
   ```

4. Ensure you have NuGet credentials configured:
   ```bash
   # Configure NuGet API key
   dotnet nuget add source https://api.nuget.org/v3/index.json -n nuget.org
   ```

## Release Steps

### 1. Update Version

Update the version in both `Volley.csproj` and `Version.cs` following [Semantic Versioning](https://semver.org/):
- **MAJOR** version for incompatible API changes
- **MINOR** version for backwards-compatible functionality additions
- **PATCH** version for backwards-compatible bug fixes

### 2. Build the Package

```bash
cd src/Volley
dotnet pack --configuration Release
```

This creates a `.nupkg` file in `bin/Release/`.

### 3. Test the Package Locally

```bash
# Create a test project
mkdir test-package
cd test-package
dotnet new console

# Install the local package
dotnet add package Volley --source ../bin/Release
```

### 4. Create Git Tag

```bash
git tag -a v1.0.0 -m "Release v1.0.0 - Initial release of Volley .NET SDK"
git push origin v1.0.0
```

### 5. Publish to NuGet

```bash
cd src/Volley
dotnet nuget push bin/Release/Volley.1.0.0.nupkg --api-key YOUR_NUGET_API_KEY --source https://api.nuget.org/v3/index.json
```

Or use the NuGet CLI:

```bash
nuget push bin/Release/Volley.1.0.0.nupkg -ApiKey YOUR_NUGET_API_KEY -Source https://api.nuget.org/v3/index.json
```

### 6. Create GitHub Release

1. Go to [GitHub Releases](https://github.com/volleyhq/volley-dotnet/releases)
2. Click "Draft a new release"
3. Select the tag you just created
4. Add release notes
5. Publish the release

## NuGet API Key

Get your NuGet API key from:
- https://www.nuget.org/account/apikeys

## Verification

After publishing, verify the package is available:

```bash
dotnet add package Volley --version 1.0.0
```

Or check on NuGet:
- https://www.nuget.org/packages/Volley

