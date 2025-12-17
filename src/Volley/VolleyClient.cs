using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Volley.Models;
using Volley.Resources;

namespace Volley
{
    /// <summary>
    /// Main client for interacting with the Volley API.
    /// </summary>
    public class VolleyClient
    {
        private const string DefaultBaseUrl = "https://api.volleyhooks.com";
        private const int DefaultTimeoutSeconds = 30;

        private readonly HttpClient _httpClient;
        private readonly string _apiToken;
        private readonly string _baseUrl;
        private long? _organizationId;

        /// <summary>
        /// Organizations API resource.
        /// </summary>
        public OrganizationsResource Organizations { get; }

        /// <summary>
        /// Projects API resource.
        /// </summary>
        public ProjectsResource Projects { get; }

        /// <summary>
        /// Sources API resource.
        /// </summary>
        public SourcesResource Sources { get; }

        /// <summary>
        /// Destinations API resource.
        /// </summary>
        public DestinationsResource Destinations { get; }

        /// <summary>
        /// Connections API resource.
        /// </summary>
        public ConnectionsResource Connections { get; }

        /// <summary>
        /// Events API resource.
        /// </summary>
        public EventsResource Events { get; }

        /// <summary>
        /// Delivery Attempts API resource.
        /// </summary>
        public DeliveryAttemptsResource DeliveryAttempts { get; }

        /// <summary>
        /// Webhooks API resource.
        /// </summary>
        public WebhooksResource Webhooks { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VolleyClient"/> class.
        /// </summary>
        /// <param name="apiToken">Your Volley API token</param>
        /// <param name="baseUrl">Custom base URL (defaults to https://api.volleyhooks.com)</param>
        /// <param name="organizationId">Organization ID for all requests</param>
        /// <param name="httpClient">Custom HttpClient (optional)</param>
        public VolleyClient(
            string apiToken,
            string? baseUrl = null,
            long? organizationId = null,
            HttpClient? httpClient = null)
        {
            if (string.IsNullOrWhiteSpace(apiToken))
            {
                throw new ArgumentException("API token is required", nameof(apiToken));
            }

            _apiToken = apiToken;
            _baseUrl = baseUrl ?? DefaultBaseUrl;
            _organizationId = organizationId;

            _httpClient = httpClient ?? new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(DefaultTimeoutSeconds)
            };

            // Set default headers
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiToken}");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            // Initialize resource clients
            Organizations = new OrganizationsResource(this);
            Projects = new ProjectsResource(this);
            Sources = new SourcesResource(this);
            Destinations = new DestinationsResource(this);
            Connections = new ConnectionsResource(this);
            Events = new EventsResource(this);
            DeliveryAttempts = new DeliveryAttemptsResource(this);
            Webhooks = new WebhooksResource(this);
        }

        /// <summary>
        /// Sets the organization ID for subsequent requests.
        /// </summary>
        /// <param name="organizationId">The organization ID to use</param>
        public void SetOrganizationId(long organizationId)
        {
            _organizationId = organizationId;
        }

        /// <summary>
        /// Clears the organization ID (uses default organization).
        /// </summary>
        public void ClearOrganizationId()
        {
            _organizationId = null;
        }

        /// <summary>
        /// Gets the current organization ID.
        /// </summary>
        /// <returns>The organization ID, or null if not set</returns>
        public long? GetOrganizationId()
        {
            return _organizationId;
        }

        /// <summary>
        /// Performs an HTTP request with authentication.
        /// </summary>
        /// <typeparam name="T">Response type</typeparam>
        /// <param name="method">HTTP method</param>
        /// <param name="path">API path</param>
        /// <param name="data">Request body data (for POST/PUT)</param>
        /// <param name="queryParams">Query parameters</param>
        /// <returns>Response data</returns>
        internal async Task<T> RequestAsync<T>(
            string method,
            string path,
            object? data = null,
            System.Collections.Generic.Dictionary<string, string>? queryParams = null)
        {
            var url = _baseUrl + path;

            // Add query parameters
            if (queryParams != null && queryParams.Count > 0)
            {
                var queryString = new System.Text.StringBuilder();
                foreach (var param in queryParams)
                {
                    if (queryString.Length > 0)
                        queryString.Append("&");
                    queryString.Append($"{Uri.EscapeDataString(param.Key)}={Uri.EscapeDataString(param.Value)}");
                }
                url += "?" + queryString.ToString();
            }

            var request = new HttpRequestMessage(new HttpMethod(method), url);

            // Add organization ID header if set
            if (_organizationId.HasValue)
            {
                request.Headers.Add("X-Organization-ID", _organizationId.Value.ToString());
            }

            // Add request body for POST/PUT
            if (data != null && (method == "POST" || method == "PUT"))
            {
                var json = JsonConvert.SerializeObject(data);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            try
            {
                var response = await _httpClient.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = "Request failed";
                    try
                    {
                        var errorData = JsonConvert.DeserializeObject<System.Collections.Generic.Dictionary<string, object>>(responseContent);
                        if (errorData != null)
                        {
                            if (errorData.ContainsKey("error") && errorData["error"] != null)
                                errorMessage = errorData["error"].ToString() ?? errorMessage;
                            else if (errorData.ContainsKey("message") && errorData["message"] != null)
                                errorMessage = errorData["message"].ToString() ?? errorMessage;
                        }
                    }
                    catch
                    {
                        if (!string.IsNullOrWhiteSpace(responseContent))
                            errorMessage = responseContent;
                    }

                    throw new VolleyException(errorMessage, (int)response.StatusCode);
                }

                if (string.IsNullOrWhiteSpace(responseContent))
                {
                    return default(T)!;
                }

                return JsonConvert.DeserializeObject<T>(responseContent) ?? throw new VolleyException("Failed to deserialize response", 0);
            }
            catch (VolleyException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new VolleyException($"Request failed: {ex.Message}", 0, ex);
            }
        }

        /// <summary>
        /// Performs an HTTP request that returns void.
        /// </summary>
        /// <param name="method">HTTP method</param>
        /// <param name="path">API path</param>
        /// <param name="data">Request body data (for POST/PUT)</param>
        /// <param name="queryParams">Query parameters</param>
        internal async Task RequestAsync(
            string method,
            string path,
            object? data = null,
            System.Collections.Generic.Dictionary<string, string>? queryParams = null)
        {
            await RequestAsync<object>(method, path, data, queryParams);
        }
    }
}

