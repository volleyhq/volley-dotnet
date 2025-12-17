using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using RichardSzalay.MockHttp;
using Xunit;
using Volley;
using Volley.Models;

namespace Volley.Tests
{
    public class EventsResourceTests
    {
        [Fact]
        public async Task ListAsync_WithValidResponse_ReturnsEvents()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://api.volleyhooks.com/api/projects/1/events*")
                .Respond("application/json", @"{
                    ""requests"": [
                        { ""id"": 1, ""event_id"": ""evt_123"", ""source_id"": 10, ""project_id"": 1, ""raw_body"": ""{}"", ""headers"": {}, ""status"": ""success"", ""created_at"": ""2024-01-01T00:00:00Z"" }
                    ],
                    ""total"": 1,
                    ""limit"": 10,
                    ""offset"": 0
                }");

            var httpClient = mockHttp.ToHttpClient();
            var client = new VolleyClient("test-token", httpClient: httpClient);

            var (events, total, limit, offset) = await client.Events.ListAsync(1);
            Assert.NotNull(events);
            Assert.Single(events);
            Assert.Equal(1, total);
            Assert.Equal(10, limit);
            Assert.Equal(0, offset);
            Assert.Equal("evt_123", events[0].EventId);
        }

        [Fact]
        public async Task ListAsync_WithQueryParameters_AddsQueryString()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://api.volleyhooks.com/api/projects/1/events?limit=5&offset=10")
                .Respond("application/json", "{ \"requests\": [], \"total\": 0, \"limit\": 5, \"offset\": 10 }");

            var httpClient = mockHttp.ToHttpClient();
            var client = new VolleyClient("test-token", httpClient: httpClient);

            var (events, total, limit, offset) = await client.Events.ListAsync(1, limit: 5, offset: 10);
            Assert.NotNull(events);
            Assert.Equal(5, limit);
            Assert.Equal(10, offset);
        }

        [Fact]
        public async Task GetAsync_WithValidEventId_ReturnsEvent()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://api.volleyhooks.com/api/projects/1/events/evt_123")
                .Respond("application/json", @"{
                    ""request"": {
                        ""id"": 1,
                        ""event_id"": ""evt_123"",
                        ""source_id"": 10,
                        ""project_id"": 1,
                        ""raw_body"": ""{\""test\"": \""value\""}"",
                        ""headers"": { ""Content-Type"": ""application/json"" },
                        ""status"": ""success"",
                        ""created_at"": ""2024-01-01T00:00:00Z""
                    }
                }");

            var httpClient = mockHttp.ToHttpClient();
            var client = new VolleyClient("test-token", httpClient: httpClient);

            var evt = await client.Events.GetAsync(1, "evt_123");
            Assert.NotNull(evt);
            Assert.Equal("evt_123", evt.EventId);
            Assert.Equal(10, evt.SourceId);
        }

        [Fact]
        public async Task ReplayAsync_WithValidEventId_ReplaysEvent()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://api.volleyhooks.com/api/replay-event")
                .WithPartialContent("\"event_id\":\"evt_123\"")
                .Respond("application/json", "{ \"message\": \"Event replayed successfully\", \"delivery_attempt_id\": 100 }");

            var httpClient = mockHttp.ToHttpClient();
            var client = new VolleyClient("test-token", httpClient: httpClient);

            var message = await client.Events.ReplayAsync("evt_123");
            Assert.Contains("replayed", message, System.StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task ReplayAsync_WithDestinationId_IncludesDestinationId()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://api.volleyhooks.com/api/replay-event")
                .WithPartialContent("\"destination_id\":50")
                .Respond("application/json", "{ \"message\": \"Event replayed\" }");

            var httpClient = mockHttp.ToHttpClient();
            var client = new VolleyClient("test-token", httpClient: httpClient);

            await client.Events.ReplayAsync("evt_123", destinationId: 50);
            // Should not throw
        }

        [Fact]
        public async Task GetAsync_WithErrorResponse_ThrowsVolleyException()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://api.volleyhooks.com/api/projects/1/events/invalid")
                .Respond(HttpStatusCode.NotFound, "application/json", "{ \"error\": \"Event not found\" }");

            var httpClient = mockHttp.ToHttpClient();
            var client = new VolleyClient("test-token", httpClient: httpClient);

            var exception = await Assert.ThrowsAsync<VolleyException>(
                () => client.Events.GetAsync(1, "invalid"));
            
            Assert.Equal(404, exception.StatusCode);
        }
    }
}

