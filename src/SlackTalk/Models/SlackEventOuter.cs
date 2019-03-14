using System.Collections.Generic;
using Newtonsoft.Json;

namespace SlackTalk.Models
{
    /// <summary>
    /// A SlackEventOuter is also referred to as the "outer event", or the object containing the event that happened itself.
    /// </summary>
    public class SlackEventOuter
    {
        /// <summary>
        /// The shared-private callback token that authenticates this callback to the application as having come from Slack. Match this against what you were given when the subscription was created. If it does not match, do not process the event and discard it.
        /// </summary>
        public string token { get; set; }
        
        /// <summary>
        /// The unique identifier for the workspace/team where this event occurred.
        /// </summary>
        public string team_id { get; set; }
        
        /// <summary>
        /// The unique identifier for the application this event is intended for. Your application's ID can be found in the URL of the your application console. If your Request URL manages multiple applications, use this field along with the token field to validate and route incoming requests.
        /// </summary>
        public string api_app_id { get; set; }
        
        /// <summary>
        /// Contains the inner set of fields representing the event that's happening.
        /// </summary>
        [JsonProperty("event")] 
        public SlackEvent Event { get; set; }
        
        /// <summary>
        /// This reflects the type of callback you're receiving. Typically, that is event_callback. You may encounter url_verification during the configuration process. The `event` fields "inner event" will also contain a type field indicating which event type lurks within.
        /// </summary>
        public string type { get; set; }
        
        /// <summary>
        /// An array of string-based User IDs. Each member of the collection represents a user that has installed your application/bot and indicates the described event would be visible to those users. You'll receive a single event for a piece of data intended for multiple users in a workspace, rather than a message per user.
        /// </summary>
        public List<string> authed_users { get; set; }
        
        /// <summary>
        /// A unique identifier for this specific event, globally unique across all workspaces. Events will include this field beginning March 9, 2017.
        /// </summary>
        public string event_id { get; set; }
        
        /// <summary>
        /// The epoch timestamp in seconds indicating when this event was dispatched. Events will include this field beginning March 9, 2017.
        /// </summary>
        public int event_time { get; set; }
        
        /// <summary>
        /// The challenge value to respond with on a url_verification event.
        /// </summary>
        public string challenge { get; set; }
        
        /// <summary>
        /// A rounded epoch time value indicating the minute your application became rate limited for this workspace. 1518467820 is at 2018-02-12 20:37:00 UTC. Only used when type is "app_rate_limited".
        /// </summary>
        public int minute_rate_limited { get; set; }
    }
}