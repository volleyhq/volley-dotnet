using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using RichardSzalay.MockHttp;
using Xunit;
using Volley;
using Volley.Models;

namespace Volley.Tests
{
    public class VolleyClientTests
    {
        [Fact]
        public void Constructor_WithValidToken_CreatesClient()
        {
            var client = new VolleyClient("test-token");
            Assert.NotNull(client);
            Assert.NotNull(client.Organizations);
            Assert.NotNull(client.Projects);
            Assert.NotNull(client.Sources);
            Assert.NotNull(client.Destinations);
            Assert.NotNull(client.Connections);
            Assert.NotNull(client.Events);
            Assert.NotNull(client.DeliveryAttempts);
            Assert.NotNull(client.Webhooks);
        }

        [Fact]
        public void Constructor_WithEmptyToken_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => new VolleyClient(""));
            Assert.Throws<ArgumentException>(() => new VolleyClient(null!));
        }

        [Fact]
        public void Constructor_WithCustomBaseUrl_UsesCustomUrl()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://custom-api.com/api/org/list")
                .Respond("application/json", "{ \"organizations\": [] }");

            var httpClient = mockHttp.ToHttpClient();
            var client = new VolleyClient("test-token", baseUrl: "https://custom-api.com", httpClient: httpClient);

            Assert.NotNull(client);
        }

        [Fact]
        public void SetOrganizationId_SetsOrganizationId()
        {
            var client = new VolleyClient("test-token");
            client.SetOrganizationId(123);
            Assert.Equal(123, client.GetOrganizationId());
        }

        [Fact]
        public void ClearOrganizationId_ClearsOrganizationId()
        {
            var client = new VolleyClient("test-token");
            client.SetOrganizationId(123);
            client.ClearOrganizationId();
            Assert.Null(client.GetOrganizationId());
        }

        [Fact]
        public void Constructor_WithOrganizationId_SetsOrganizationId()
        {
            var client = new VolleyClient("test-token", organizationId: 456);
            Assert.Equal(456, client.GetOrganizationId());
        }

        [Fact]
        public async Task Organizations_GetAsync_WithSuccessResponse_ReturnsData()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://api.volleyhooks.com/api/org")
                .Respond("application/json", "{ \"id\": 1, \"name\": \"Test Org\", \"slug\": \"test-org\", \"role\": \"owner\" }");

            var httpClient = mockHttp.ToHttpClient();
            var client = new VolleyClient("test-token", httpClient: httpClient);

            var response = await client.Organizations.GetAsync();
            Assert.NotNull(response);
            Assert.Equal(1, response.Id);
            Assert.Equal("Test Org", response.Name);
        }

        [Fact]
        public async Task Organizations_GetAsync_WithErrorResponse_ThrowsVolleyException()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://api.volleyhooks.com/api/org")
                .Respond(HttpStatusCode.NotFound, "application/json", "{ \"error\": \"Organization not found\" }");

            var httpClient = mockHttp.ToHttpClient();
            var client = new VolleyClient("test-token", httpClient: httpClient);

            var exception = await Assert.ThrowsAsync<VolleyException>(
                () => client.Organizations.GetAsync());
            
            Assert.Equal(404, exception.StatusCode);
            Assert.Contains("not found", exception.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task Organizations_GetAsync_WithOrganizationHeader_AddsHeader()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://api.volleyhooks.com/api/org")
                .WithHeaders("X-Organization-ID", "123")
                .Respond("application/json", "{ \"id\": 1, \"name\": \"Test\", \"slug\": \"test\", \"role\": \"owner\" }");

            var httpClient = mockHttp.ToHttpClient();
            var client = new VolleyClient("test-token", httpClient: httpClient);
            client.SetOrganizationId(123);

            var response = await client.Organizations.GetAsync();
            Assert.NotNull(response);
        }

        [Fact]
        public async Task Events_ListAsync_WithQueryParameters_AddsQueryString()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://api.volleyhooks.com/api/projects/1/events?limit=10&offset=0")
                .Respond("application/json", "{ \"requests\": [], \"total\": 0, \"limit\": 10, \"offset\": 0 }");

            var httpClient = mockHttp.ToHttpClient();
            var client = new VolleyClient("test-token", httpClient: httpClient);

            var (events, total, limit, offset) = await client.Events.ListAsync(1, limit: 10, offset: 0);
            Assert.NotNull(events);
            Assert.Equal(10, limit);
            Assert.Equal(0, offset);
        }
    }
}

