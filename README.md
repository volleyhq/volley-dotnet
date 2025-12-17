# Volley .NET SDK

Official .NET SDK for the Volley API. This SDK provides a convenient way to interact with the Volley webhook infrastructure API.

[Volley](https://volleyhooks.com) is a webhook infrastructure platform that provides reliable webhook delivery, rate limiting, retries, monitoring, and more.

## Resources

- **Documentation**: [https://docs.volleyhooks.com](https://docs.volleyhooks.com)
- **Getting Started Guide**: [https://docs.volleyhooks.com/getting-started](https://docs.volleyhooks.com/getting-started)
- **API Reference**: [https://docs.volleyhooks.com/api](https://docs.volleyhooks.com/api)
- **Authentication Guide**: [https://docs.volleyhooks.com/authentication](https://docs.volleyhooks.com/authentication)
- **Security Guide**: [https://docs.volleyhooks.com/security](https://docs.volleyhooks.com/security)
- **Console**: [https://app.volleyhooks.com](https://app.volleyhooks.com)
- **Website**: [https://volleyhooks.com](https://volleyhooks.com)

## Installation

### Package Manager

```powershell
Install-Package Volley
```

### .NET CLI

```bash
dotnet add package Volley
```

### PackageReference

```xml
<PackageReference Include="Volley" Version="1.0.0" />
```

## Quick Start

```csharp
using Volley;
using Volley.Models;

// Create a client with your API token
var client = new VolleyClient("your-api-token");

// Optionally set organization context
client.SetOrganizationId(123);

// List organizations
var orgs = await client.Organizations.ListAsync();
foreach (var org in orgs)
{
    Console.WriteLine($"Organization: {org.Name} (ID: {org.Id})");
}
```

## Authentication

Volley uses API tokens for authentication. These are long-lived tokens designed for programmatic access.

### Getting Your API Token

1. Log in to the [Volley Console](https://app.volleyhooks.com)
2. Navigate to **Settings → Account → API Token**
3. Click **View Token** (you may need to verify your password)
4. Copy the token and store it securely

**Important**: API tokens are non-expiring and provide full access to your account. Keep them secure and rotate them if compromised. See the [Security Guide](https://docs.volleyhooks.com/security) for best practices.

```csharp
var client = new VolleyClient("your-api-token");
```

For more details on authentication, API tokens, and security, see the [Authentication Guide](https://docs.volleyhooks.com/authentication) and [Security Guide](https://docs.volleyhooks.com/security).

## Organization Context

When you have multiple organizations, you need to specify which organization context to use for API requests. The API verifies that resources (like projects) belong to the specified organization.

You can set the organization context in two ways:

```csharp
// Method 1: Set organization ID for all subsequent requests
client.SetOrganizationId(123);

// Method 2: Create client with organization ID
var client = new VolleyClient("your-api-token", organizationId: 123);

// Clear organization context (uses first accessible organization)
client.ClearOrganizationId();
```

**Note**: If you don't set an organization ID, the API uses your first accessible organization by default. For more details, see the [API Reference - Organization Context](https://docs.volleyhooks.com/api#organization-context).

## Examples

### Organizations

```csharp
// List all organizations
var orgs = await client.Organizations.ListAsync();

// Get current organization
var org = await client.Organizations.GetAsync(); // null = use default

// Create organization
var newOrg = await client.Organizations.CreateAsync("My Organization");
```

### Projects

```csharp
// List projects in current organization
var projects = await client.Projects.ListAsync();

// Create project
var project = await client.Projects.CreateAsync("My Project");

// Update project
var updated = await client.Projects.UpdateAsync(project.Id, "Updated Name");

// Delete project
await client.Projects.DeleteAsync(project.Id);
```

### Sources

```csharp
// List sources for a project
var sources = await client.Sources.ListAsync(projectId);

// Create source
var source = await client.Sources.CreateAsync(
    projectId: projectId,
    slug: "stripe",
    type: "webhook",
    eps: 10
);

// Get source
var sourceDetails = await client.Sources.GetAsync(projectId, source.Id);

// Update source
var updated = await client.Sources.UpdateAsync(projectId, source.Id, eps: 20);

// Delete source
await client.Sources.DeleteAsync(projectId, source.Id);
```

### Destinations

```csharp
// List destinations for a project
var destinations = await client.Destinations.ListAsync(projectId);

// Create destination
var destination = await client.Destinations.CreateAsync(
    projectId: projectId,
    name: "My API",
    url: "https://api.example.com/webhook",
    eps: 5
);

// Get destination
var destDetails = await client.Destinations.GetAsync(projectId, destination.Id);

// Update destination
var updated = await client.Destinations.UpdateAsync(projectId, destination.Id, url: "https://new-url.com/webhook");

// Delete destination
await client.Destinations.DeleteAsync(projectId, destination.Id);
```

### Connections

```csharp
// Create connection
var connection = await client.Connections.CreateAsync(
    projectId: projectId,
    sourceId: source.Id,
    destinationId: destination.Id,
    status: "enabled"
);

// Get connection
var connDetails = await client.Connections.GetAsync(connection.Id);

// Update connection
var updated = await client.Connections.UpdateAsync(connection.Id, status: "paused");

// Delete connection
await client.Connections.DeleteAsync(connection.Id);
```

### Events

```csharp
// List events
var (events, total, limit, offset) = await client.Events.ListAsync(
    projectId: projectId,
    limit: 10,
    offset: 0
);

// Get event by ID
var eventDetails = await client.Events.GetAsync(projectId, "evt_abc123xyz");

// Replay a failed event
await client.Events.ReplayAsync("evt_abc123xyz");
```

### Delivery Attempts

```csharp
// List delivery attempts
var (attempts, total, limit, offset) = await client.DeliveryAttempts.ListAsync(
    projectId: projectId,
    eventId: "evt_abc123xyz",
    status: "failed",
    limit: 20
);
```

### Webhooks

```csharp
// Send a webhook
var body = new Dictionary<string, object>
{
    ["event"] = "payment.succeeded",
    ["amount"] = 1000
};

var headers = new Dictionary<string, string>
{
    ["X-Custom-Header"] = "value"
};

await client.Webhooks.SendAsync(
    sourceId: source.Id,
    destinationId: destination.Id,
    body: body,
    headers: headers
);
```

## Error Handling

The SDK throws `VolleyException` for API errors:

```csharp
try
{
    var org = await client.Organizations.GetAsync(999);
}
catch (VolleyException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
    Console.WriteLine($"Status Code: {ex.StatusCode}");
    
    if (ex.IsNotFound())
    {
        Console.WriteLine("Organization not found");
    }
    else if (ex.IsUnauthorized())
    {
        Console.WriteLine("Authentication failed");
    }
}
```

## Client Options

```csharp
// Custom base URL (for testing)
var client = new VolleyClient(
    apiToken: "your-api-token",
    baseUrl: "https://api-staging.volleyhooks.com"
);

// Custom HttpClient
var httpClient = new HttpClient();
httpClient.Timeout = TimeSpan.FromSeconds(60);
var client = new VolleyClient(
    apiToken: "your-api-token",
    httpClient: httpClient
);
```

## Requirements

- .NET Standard 2.1 or later
- .NET Core 3.0 or later
- .NET Framework 4.7.2 or later

## License

MIT License - see [LICENSE](LICENSE) file for details.

## Support

- **Documentation**: [https://docs.volleyhooks.com](https://docs.volleyhooks.com)
- **Issues**: [https://github.com/volleyhq/volley-dotnet/issues](https://github.com/volleyhq/volley-dotnet/issues)
- **Email**: support@volleyhooks.com

