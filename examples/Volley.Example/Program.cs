using System;
using System.Threading.Tasks;
using Volley;
using Volley.Models;

namespace Volley.Example
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Get API token from environment variable
            var apiToken = Environment.GetEnvironmentVariable("VOLLEY_API_TOKEN");
            if (string.IsNullOrWhiteSpace(apiToken))
            {
                Console.Error.WriteLine("VOLLEY_API_TOKEN environment variable is required");
                Environment.Exit(1);
            }

            // Create a client
            var client = new VolleyClient(apiToken);

            try
            {
                // Example 1: List organizations
                Console.WriteLine("=== Listing Organizations ===");
                var orgs = await client.Organizations.ListAsync();
                if (orgs == null || orgs.Count == 0)
                {
                    Console.WriteLine("  No organizations found");
                    return;
                }

                foreach (var org in orgs)
                {
                    Console.WriteLine($"  - {org.Name} (ID: {org.Id}, Role: {org.Role})");
                }

                // Example 2: Set organization context
                var orgId = orgs[0].Id;
                client.SetOrganizationId(orgId);
                Console.WriteLine($"\n=== Using Organization: {orgs[0].Name} (ID: {orgId}) ===");

                // Example 3: List projects
                Console.WriteLine("\n=== Listing Projects ===");
                var projects = await client.Projects.ListAsync();
                if (projects == null || projects.Count == 0)
                {
                    Console.WriteLine("  No projects found");
                    return;
                }

                foreach (var project in projects)
                {
                    Console.Write($"  - {project.Name} (ID: {project.Id}");
                    if (project.IsDefault)
                    {
                        Console.Write(", Default");
                    }
                    Console.WriteLine(")");
                }

                // Example 4: List sources for first project
                var projectId = projects[0].Id;
                Console.WriteLine($"\n=== Listing Sources for Project: {projects[0].Name} (ID: {projectId}) ===");
                var sources = await client.Sources.ListAsync(projectId);
                if (sources == null || sources.Count == 0)
                {
                    Console.WriteLine("  No sources found");
                }
                else
                {
                    foreach (var source in sources)
                    {
                        Console.WriteLine($"  - {source.Slug} (ID: {source.Id}, Ingestion ID: {source.IngestionId})");
                    }
                }

                // Example 5: List events (if any)
                Console.WriteLine($"\n=== Listing Recent Events for Project: {projects[0].Name} ===");
                var (events, total, limit, offset) = await client.Events.ListAsync(projectId, limit: 10);
                if (events != null && events.Count > 0)
                {
                    Console.WriteLine($"Total events: {total}");
                    int count = 0;
                    foreach (var evt in events)
                    {
                        if (count >= 5) break; // Show only first 5
                        Console.WriteLine($"  - Event ID: {evt.EventId}, Status: {evt.Status}");
                        count++;
                    }
                }
            }
            catch (VolleyException ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                if (ex.StatusCode > 0)
                {
                    Console.Error.WriteLine($"Status Code: {ex.StatusCode}");
                }
                Environment.Exit(1);
            }
        }
    }
}

