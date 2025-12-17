using System;
using System.Threading.Tasks;
using Xunit;
using Volley;
using Volley.Models;

namespace Volley.Tests
{
    /// <summary>
    /// Integration tests that make real API calls.
    /// These tests are skipped unless VOLLEY_API_TOKEN is set.
    /// </summary>
    public class IntegrationTests
    {
        private static string? GetApiToken()
        {
            return Environment.GetEnvironmentVariable("VOLLEY_API_TOKEN");
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task TestListOrganizations()
        {
            var apiToken = GetApiToken();
            if (string.IsNullOrWhiteSpace(apiToken))
            {
                return; // Skip if no token
            }

            var client = new VolleyClient(apiToken);
            var orgs = await client.Organizations.ListAsync();
            Assert.NotNull(orgs);
            // May be empty for new accounts, which is OK
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task TestGetOrganization()
        {
            var apiToken = GetApiToken();
            if (string.IsNullOrWhiteSpace(apiToken))
            {
                return; // Skip if no token
            }

            var client = new VolleyClient(apiToken);
            var orgs = await client.Organizations.ListAsync();
            if (orgs == null || orgs.Count == 0)
            {
                return; // Skip if no organizations
            }

            var org = await client.Organizations.GetAsync(orgs[0].Id);
            Assert.NotNull(org);
            Assert.Equal(orgs[0].Id, org.Id);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task TestListProjects()
        {
            var apiToken = GetApiToken();
            if (string.IsNullOrWhiteSpace(apiToken))
            {
                return; // Skip if no token
            }

            var client = new VolleyClient(apiToken);
            var orgs = await client.Organizations.ListAsync();
            if (orgs == null || orgs.Count == 0)
            {
                return; // Skip if no organizations
            }

            client.SetOrganizationId(orgs[0].Id);
            var projects = await client.Projects.ListAsync();
            Assert.NotNull(projects);
            // May be empty, which is OK
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task TestListSources()
        {
            var apiToken = GetApiToken();
            if (string.IsNullOrWhiteSpace(apiToken))
            {
                return; // Skip if no token
            }

            var client = new VolleyClient(apiToken);
            var orgs = await client.Organizations.ListAsync();
            if (orgs == null || orgs.Count == 0)
            {
                return; // Skip if no organizations
            }

            client.SetOrganizationId(orgs[0].Id);
            var projects = await client.Projects.ListAsync();
            if (projects == null || projects.Count == 0)
            {
                return; // Skip if no projects
            }

            var sources = await client.Sources.ListAsync(projects[0].Id);
            Assert.NotNull(sources);
            // May be empty, which is OK
        }
    }
}

