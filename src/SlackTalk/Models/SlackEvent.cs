namespace SlackTalk.Models
{
    /// <summary>
    /// A SlackEvent is also referred to as the "inner event", or the event that happened itself.
    /// </summary>
    public class SlackEvent
    {
        /// <summary>
        /// The specific name of the event described by its adjacent fields. This field is included with every inner event type. Examples: reaction_added, messages.channel, team_join
        /// </summary>
        public string type { get; set; }
        
        /// <summary>
        /// The timestamp of the event. The combination of event_ts, team_id, user_id, or channel_id is intended to be unique. This field is included with every inner event type.
        /// </summary>
        public string event_ts { get; set; }
        
        /// <summary>
        /// The user ID belonging to the user that incited this action. Not included in all events as not all events are controlled by users. See the top-level callback object's authed_users if you need to calculate event visibility by user.
        /// </summary>
        public string user { get; set; }
        
        /// <summary>
        /// The timestamp of what the event describes, which may occur slightly prior to the event being dispatched as described by event_ts. The combination of ts, team_id, user_id, or channel_id is intended to be unique.
        /// </summary>
        public string ts { get; set; }
        
        /// <summary>
        /// Data specific to the underlying object type being described. Often you'll encounter abbreviated versions of full objects. For instance, when file objects are referenced, only the file's ID is presented. See each individual event type for more detail.
        /// </summary>
        public object item { get; set; }
    }
}