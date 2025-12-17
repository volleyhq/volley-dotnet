using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using RichardSzalay.MockHttp;
using Xunit;
using Volley;
using Volley.Models;

namespace Volley.Tests
{
    public class OrganizationsResourceTests
    {
        [Fact]
        public async Task ListAsync_WithValidResponse_ReturnsOrganizations()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://api.volleyhooks.com/api/org/list")
                .Respond("application/json", @"{
                    ""organizations"": [
                        { ""id"": 1, ""name"": ""Org 1"", ""slug"": ""org-1"", ""role"": ""owner"", ""account_id"": 10, ""created_at"": ""2024-01-01T00:00:00Z"" },
                        { ""id"": 2, ""name"": ""Org 2"", ""slug"": ""org-2"", ""role"": ""admin"", ""account_id"": 10, ""created_at"": ""2024-01-02T00:00:00Z"" }
                    ]
                }");

            var httpClient = mockHttp.ToHttpClient();
            var client = new VolleyClient("test-token", httpClient: httpClient);

            var orgs = await client.Organizations.ListAsync();
            Assert.NotNull(orgs);
            Assert.Equal(2, orgs.Count);
            Assert.Equal("Org 1", orgs[0].Name);
            Assert.Equal("Org 2", orgs[1].Name);
        }

        [Fact]
        public async Task ListAsync_WithEmptyResponse_ReturnsEmptyList()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://api.volleyhooks.com/api/org/list")
                .Respond("application/json", "{ \"organizations\": [] }");

            var httpClient = mockHttp.ToHttpClient();
            var client = new VolleyClient("test-token", httpClient: httpClient);

            var orgs = await client.Organizations.ListAsync();
            Assert.NotNull(orgs);
            Assert.Empty(orgs);
        }

        [Fact]
        public async Task GetAsync_WithValidResponse_ReturnsOrganization()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://api.volleyhooks.com/api/org")
                .Respond("application/json", "{ \"id\": 1, \"name\": \"Test Org\", \"slug\": \"test-org\", \"role\": \"owner\" }");

            var httpClient = mockHttp.ToHttpClient();
            var client = new VolleyClient("test-token", httpClient: httpClient);

            var org = await client.Organizations.GetAsync();
            Assert.NotNull(org);
            Assert.Equal(1, org.Id);
            Assert.Equal("Test Org", org.Name);
            Assert.Equal("test-org", org.Slug);
            Assert.Equal("owner", org.Role);
        }

        [Fact]
        public async Task GetAsync_WithOrganizationId_SetsAndRestoresOrganizationId()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://api.volleyhooks.com/api/org")
                .WithHeaders("X-Organization-ID", "123")
                .Respond("application/json", "{ \"id\": 123, \"name\": \"Test\", \"slug\": \"test\", \"role\": \"owner\" }");

            var httpClient = mockHttp.ToHttpClient();
            var client = new VolleyClient("test-token", httpClient: httpClient);
            client.SetOrganizationId(456); // Original org ID

            var org = await client.Organizations.GetAsync(123);
            
            Assert.NotNull(org);
            Assert.Equal(123, org.Id);
            Assert.Equal(456, client.GetOrganizationId()); // Should restore original
        }

        [Fact]
        public async Task CreateAsync_WithValidRequest_CreatesOrganization()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://api.volleyhooks.com/api/org")
                .WithPartialContent("{\"name\":\"New Org\"}")
                .Respond("application/json", "{ \"id\": 1, \"name\": \"New Org\", \"slug\": \"new-org\", \"role\": \"owner\", \"account_id\": 10, \"created_at\": \"2024-01-01T00:00:00Z\" }");

            var httpClient = mockHttp.ToHttpClient();
            var client = new VolleyClient("test-token", httpClient: httpClient);

            var org = await client.Organizations.CreateAsync("New Org");
            Assert.NotNull(org);
            Assert.Equal("New Org", org.Name);
            Assert.Equal("new-org", org.Slug);
        }

        [Fact]
        public async Task GetAsync_WithErrorResponse_ThrowsVolleyException()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://api.volleyhooks.com/api/org")
                .Respond(HttpStatusCode.NotFound, "application/json", "{ \"error\": \"Organization not found\" }");

            var httpClient = mockHttp.ToHttpClient();
            var client = new VolleyClient("test-token", httpClient: httpClient);

            var exception = await Assert.ThrowsAsync<VolleyException>(
                () => client.Organizations.GetAsync());
            
            Assert.Equal(404, exception.StatusCode);
        }
    }
}

