using System.Collections.Generic;
using System.Threading.Tasks;
using Volley.Models;

namespace Volley.Resources
{
    /// <summary>
    /// Projects API resource.
    /// </summary>
    public class ProjectsResource
    {
        private readonly VolleyClient _client;

        internal ProjectsResource(VolleyClient client)
        {
            _client = client;
        }

        /// <summary>
        /// List all projects in the current organization.
        /// </summary>
        /// <returns>List of projects</returns>
        public async Task<List<Project>> ListAsync()
        {
            var response = await _client.RequestAsync<Dictionary<string, object>>("GET", "/api/projects");
            var projects = new List<Project>();
            
            if (response.ContainsKey("projects") && response["projects"] != null)
            {
                var projectsData = response["projects"];
                var projectsJson = Newtonsoft.Json.JsonConvert.SerializeObject(projectsData);
                var projectsList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Project>>(projectsJson);
                if (projectsList != null)
                {
                    projects.AddRange(projectsList);
                }
            }
            
            return projects;
        }

        /// <summary>
        /// Create a new project.
        /// </summary>
        /// <param name="name">Project name</param>
        /// <returns>Created project</returns>
        public async Task<Project> CreateAsync(string name)
        {
            var data = new { name };
            var response = await _client.RequestAsync<Project>("POST", "/api/projects", data);
            return response;
        }

        /// <summary>
        /// Update a project.
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <param name="name">Updated project name</param>
        /// <returns>Updated project</returns>
        public async Task<Project> UpdateAsync(long projectId, string name)
        {
            var data = new { name };
            var response = await _client.RequestAsync<Project>("PUT", $"/api/projects/{projectId}", data);
            return response;
        }

        /// <summary>
        /// Delete a project.
        /// </summary>
        /// <param name="projectId">Project ID to delete</param>
        public async Task DeleteAsync(long projectId)
        {
            await _client.RequestAsync("DELETE", $"/api/projects/{projectId}");
        }
    }
}

