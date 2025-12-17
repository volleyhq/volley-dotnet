using System.Collections.Generic;
using System.Threading.Tasks;
using Volley.Models;

namespace Volley.Resources
{
    /// <summary>
    /// Delivery Attempts API resource.
    /// </summary>
    public class DeliveryAttemptsResource
    {
        private readonly VolleyClient _client;

        internal DeliveryAttemptsResource(VolleyClient client)
        {
            _client = client;
        }

        /// <summary>
        /// List delivery attempts.
        /// </summary>
        public async Task<(List<DeliveryAttempt> Attempts, int Total, int Limit, int Offset)> ListAsync(
            long? projectId = null, string? eventId = null, long? connectionId = null, 
            string? status = null, int? limit = null, int? offset = null)
        {
            var queryParams = new Dictionary<string, string>();
            if (projectId.HasValue) queryParams["project_id"] = projectId.Value.ToString();
            if (eventId != null) queryParams["event_id"] = eventId;
            if (connectionId.HasValue) queryParams["connection_id"] = connectionId.Value.ToString();
            if (status != null) queryParams["status"] = status;
            if (limit.HasValue) queryParams["limit"] = limit.Value.ToString();
            if (offset.HasValue) queryParams["offset"] = offset.Value.ToString();

            var response = await _client.RequestAsync<Dictionary<string, object>>("GET", "/api/delivery-attempts", queryParams: queryParams);
            
            var attempts = new List<DeliveryAttempt>();
            if (response.ContainsKey("attempts") && response["attempts"] != null)
            {
                var attemptsData = response["attempts"];
                var attemptsJson = Newtonsoft.Json.JsonConvert.SerializeObject(attemptsData);
                var attemptsList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DeliveryAttempt>>(attemptsJson);
                if (attemptsList != null)
                {
                    attempts.AddRange(attemptsList);
                }
            }

            var total = response.ContainsKey("total") ? System.Convert.ToInt32(response["total"]) : 0;
            var limitVal = response.ContainsKey("limit") ? System.Convert.ToInt32(response["limit"]) : 0;
            var offsetVal = response.ContainsKey("offset") ? System.Convert.ToInt32(response["offset"]) : 0;

            return (attempts, total, limitVal, offsetVal);
        }
    }
}

