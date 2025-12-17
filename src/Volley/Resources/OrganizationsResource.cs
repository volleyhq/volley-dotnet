using System.Collections.Generic;
using System.Threading.Tasks;
using Volley.Models;

namespace Volley.Resources
{
    /// <summary>
    /// Organizations API resource.
    /// </summary>
    public class OrganizationsResource
    {
        private readonly VolleyClient _client;

        internal OrganizationsResource(VolleyClient client)
        {
            _client = client;
        }

        /// <summary>
        /// List all organizations the user has access to.
        /// </summary>
        /// <returns>List of organizations</returns>
        public async Task<List<Organization>> ListAsync()
        {
            var response = await _client.RequestAsync<Dictionary<string, object>>("GET", "/api/org/list");
            var organizations = new List<Organization>();
            
            if (response.ContainsKey("organizations") && response["organizations"] != null)
            {
                var orgsData = response["organizations"];
                var orgsJson = Newtonsoft.Json.JsonConvert.SerializeObject(orgsData);
                var orgsList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Organization>>(orgsJson);
                if (orgsList != null)
                {
                    organizations.AddRange(orgsList);
                }
            }
            
            return organizations;
        }

        /// <summary>
        /// Get the current organization.
        /// </summary>
        /// <param name="organizationId">Optional organization ID. If null, uses default organization.</param>
        /// <returns>Organization details</returns>
        public async Task<Organization> GetAsync(long? organizationId = null)
        {
            var originalOrgId = _client.GetOrganizationId();
            if (organizationId.HasValue)
            {
                _client.SetOrganizationId(organizationId.Value);
            }

            try
            {
                var response = await _client.RequestAsync<Dictionary<string, object>>("GET", "/api/org");
                var orgJson = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<Organization>(orgJson)!;
            }
            finally
            {
                if (originalOrgId.HasValue)
                {
                    _client.SetOrganizationId(originalOrgId.Value);
                }
                else
                {
                    _client.ClearOrganizationId();
                }
            }
        }

        /// <summary>
        /// Create a new organization.
        /// </summary>
        /// <param name="name">Organization name</param>
        /// <returns>Created organization</returns>
        public async Task<Organization> CreateAsync(string name)
        {
            var data = new { name };
            var response = await _client.RequestAsync<Organization>("POST", "/api/org", data);
            return response;
        }
    }
}

