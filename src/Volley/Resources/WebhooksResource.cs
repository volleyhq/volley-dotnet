using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volley.Resources
{
    /// <summary>
    /// Webhooks API resource.
    /// </summary>
    public class WebhooksResource
    {
        private readonly VolleyClient _client;

        internal WebhooksResource(VolleyClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Send a webhook.
        /// </summary>
        public async Task<string> SendAsync(long sourceId, long destinationId, 
            Dictionary<string, object> body, Dictionary<string, string>? headers = null)
        {
            var data = new Dictionary<string, object>
            {
                ["source_id"] = sourceId,
                ["destination_id"] = destinationId,
                ["body"] = body
            };
            if (headers != null) data["headers"] = headers;

            var response = await _client.RequestAsync<Dictionary<string, object>>("POST", "/api/webhooks", data);
            return response.ContainsKey("message") ? response["message"].ToString()! : "Webhook sent";
        }
    }
}

