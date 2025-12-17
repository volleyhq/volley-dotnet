using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using RichardSzalay.MockHttp;
using Xunit;
using Volley;
using Volley.Models;

namespace Volley.Tests
{
    public class ProjectsResourceTests
    {
        [Fact]
        public async Task ListAsync_WithValidResponse_ReturnsProjects()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://api.volleyhooks.com/api/projects")
                .Respond("application/json", @"{
                    ""projects"": [
                        { ""id"": 1, ""name"": ""Project 1"", ""organization_id"": 10, ""is_default"": true, ""created_at"": ""2024-01-01T00:00:00Z"", ""updated_at"": ""2024-01-01T00:00:00Z"" },
                        { ""id"": 2, ""name"": ""Project 2"", ""organization_id"": 10, ""is_default"": false, ""created_at"": ""2024-01-02T00:00:00Z"", ""updated_at"": ""2024-01-02T00:00:00Z"" }
                    ]
                }");

            var httpClient = mockHttp.ToHttpClient();
            var client = new VolleyClient("test-token", httpClient: httpClient);

            var projects = await client.Projects.ListAsync();
            Assert.NotNull(projects);
            Assert.Equal(2, projects.Count);
            Assert.Equal("Project 1", projects[0].Name);
            Assert.True(projects[0].IsDefault);
            Assert.Equal("Project 2", projects[1].Name);
            Assert.False(projects[1].IsDefault);
        }

        [Fact]
        public async Task CreateAsync_WithValidRequest_CreatesProject()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://api.volleyhooks.com/api/projects")
                .WithPartialContent("{\"name\":\"New Project\"}")
                .Respond("application/json", "{ \"id\": 1, \"name\": \"New Project\", \"organization_id\": 10, \"is_default\": false, \"created_at\": \"2024-01-01T00:00:00Z\", \"updated_at\": \"2024-01-01T00:00:00Z\" }");

            var httpClient = mockHttp.ToHttpClient();
            var client = new VolleyClient("test-token", httpClient: httpClient);

            var project = await client.Projects.CreateAsync("New Project");
            Assert.NotNull(project);
            Assert.Equal("New Project", project.Name);
        }

        [Fact]
        public async Task UpdateAsync_WithValidRequest_UpdatesProject()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://api.volleyhooks.com/api/projects/1")
                .WithPartialContent("{\"name\":\"Updated Project\"}")
                .Respond("application/json", "{ \"id\": 1, \"name\": \"Updated Project\", \"organization_id\": 10, \"is_default\": false, \"created_at\": \"2024-01-01T00:00:00Z\", \"updated_at\": \"2024-01-02T00:00:00Z\" }");

            var httpClient = mockHttp.ToHttpClient();
            var client = new VolleyClient("test-token", httpClient: httpClient);

            var project = await client.Projects.UpdateAsync(1, "Updated Project");
            Assert.NotNull(project);
            Assert.Equal("Updated Project", project.Name);
        }

        [Fact]
        public async Task DeleteAsync_WithValidId_DeletesProject()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://api.volleyhooks.com/api/projects/1")
                .Respond(HttpStatusCode.NoContent);

            var httpClient = mockHttp.ToHttpClient();
            var client = new VolleyClient("test-token", httpClient: httpClient);

            await client.Projects.DeleteAsync(1);
            // Should not throw
        }

        [Fact]
        public async Task DeleteAsync_WithErrorResponse_ThrowsVolleyException()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://api.volleyhooks.com/api/projects/999")
                .Respond(HttpStatusCode.NotFound, "application/json", "{ \"error\": \"Project not found\" }");

            var httpClient = mockHttp.ToHttpClient();
            var client = new VolleyClient("test-token", httpClient: httpClient);

            var exception = await Assert.ThrowsAsync<VolleyException>(
                () => client.Projects.DeleteAsync(999));
            
            Assert.Equal(404, exception.StatusCode);
        }
    }
}

