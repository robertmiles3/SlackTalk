namespace Devalp.SlackTalk.Models
{
    /// <summary>
    /// The clicker! The action-invoker! The button-presser! These attributes tell you all about the user who decided to interact with your message.
    /// </summary>
    public class SlackUser
    {
        /// <summary>
        /// A string identifier for the user invoking the action. Users IDs are unique to the workspace they appear within.
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// The name of that very same user.
        /// </summary>
        public string name { get; set; }
    }
}
