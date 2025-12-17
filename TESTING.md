# Testing the Volley .NET SDK

This guide explains how to run tests for the Volley .NET SDK.

## Prerequisites

- .NET SDK 6.0 or later
- A Volley API token (for integration tests)

## Running Tests

### Unit Tests

Unit tests mock HTTP requests and don't require a real API token:

```bash
dotnet test
```

### Integration Tests

Integration tests make real API calls and require a valid API token:

```bash
export VOLLEY_API_TOKEN=your-api-token-here
dotnet test --filter "Category=Integration"
```

Or on Windows:

```powershell
$env:VOLLEY_API_TOKEN="your-api-token-here"
dotnet test --filter "Category=Integration"
```

## Test Structure

- **Unit Tests**: Test individual components with mocked HTTP responses
- **Integration Tests**: Test against the real Volley API (requires API token)

## Coverage

To generate code coverage reports:

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

## Writing Tests

### Unit Test Example

```csharp
[Fact]
public async Task TestListOrganizations()
{
    // Mock HTTP client
    var mockHttp = new MockHttpMessageHandler();
    mockHttp.When("https://api.volleyhooks.com/api/org/list")
        .Respond("application/json", "{ \"organizations\": [] }");
    
    var httpClient = new HttpClient(mockHttp);
    var client = new VolleyClient("test-token", httpClient: httpClient);
    
    var orgs = await client.Organizations.ListAsync();
    Assert.NotNull(orgs);
}
```

### Integration Test Example

```csharp
[Fact]
[Trait("Category", "Integration")]
public async Task TestListOrganizationsIntegration()
{
    var apiToken = Environment.GetEnvironmentVariable("VOLLEY_API_TOKEN");
    if (string.IsNullOrWhiteSpace(apiToken))
    {
        return; // Skip if no token
    }
    
    var client = new VolleyClient(apiToken);
    var orgs = await client.Organizations.ListAsync();
    Assert.NotNull(orgs);
}
```

