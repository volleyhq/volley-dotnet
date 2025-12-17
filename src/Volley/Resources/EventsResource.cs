using System.Collections.Generic;
using System.Threading.Tasks;
using Volley.Models;

namespace Volley.Resources
{
    /// <summary>
    /// Events API resource.
    /// </summary>
    public class EventsResource
    {
        private readonly VolleyClient _client;

        internal EventsResource(VolleyClient client)
        {
            _client = client;
        }

        /// <summary>
        /// List events for a project.
        /// </summary>
        public async Task<(List<Event> Events, int Total, int Limit, int Offset)> ListAsync(
            long projectId, int? limit = null, int? offset = null, string? sourceId = null, 
            string? status = null, string? startDate = null, string? endDate = null)
        {
            var queryParams = new Dictionary<string, string>();
            if (limit.HasValue) queryParams["limit"] = limit.Value.ToString();
            if (offset.HasValue) queryParams["offset"] = offset.Value.ToString();
            if (sourceId != null) queryParams["source_id"] = sourceId;
            if (status != null) queryParams["status"] = status;
            if (startDate != null) queryParams["start_date"] = startDate;
            if (endDate != null) queryParams["end_date"] = endDate;

            var response = await _client.RequestAsync<Dictionary<string, object>>("GET", $"/api/projects/{projectId}/events", queryParams: queryParams);
            
            var events = new List<Event>();
            if (response.ContainsKey("requests") && response["requests"] != null)
            {
                var eventsData = response["requests"];
                var eventsJson = Newtonsoft.Json.JsonConvert.SerializeObject(eventsData);
                var eventsList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Event>>(eventsJson);
                if (eventsList != null)
                {
                    events.AddRange(eventsList);
                }
            }

            var total = response.ContainsKey("total") ? System.Convert.ToInt32(response["total"]) : 0;
            var limitVal = response.ContainsKey("limit") ? System.Convert.ToInt32(response["limit"]) : 0;
            var offsetVal = response.ContainsKey("offset") ? System.Convert.ToInt32(response["offset"]) : 0;

            return (events, total, limitVal, offsetVal);
        }

        /// <summary>
        /// Get an event by ID.
        /// </summary>
        public async Task<Event> GetAsync(long projectId, string eventId)
        {
            var response = await _client.RequestAsync<Dictionary<string, object>>("GET", $"/api/projects/{projectId}/events/{eventId}");
            var evtJson = Newtonsoft.Json.JsonConvert.SerializeObject(response["request"]);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Event>(evtJson)!;
        }

        /// <summary>
        /// Replay an event.
        /// </summary>
        public async Task<string> ReplayAsync(string eventId, long? destinationId = null, long? connectionId = null)
        {
            var data = new Dictionary<string, object>
            {
                ["event_id"] = eventId
            };
            if (destinationId.HasValue) data["destination_id"] = destinationId.Value;
            if (connectionId.HasValue) data["connection_id"] = connectionId.Value;

            var response = await _client.RequestAsync<Dictionary<string, object>>("POST", "/api/replay-event", data);
            return response.ContainsKey("message") ? response["message"].ToString()! : "Event replayed";
        }
    }
}

