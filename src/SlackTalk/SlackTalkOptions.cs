namespace SlackTalk
{
    /// <summary>
    /// Options model for settings related to SlackTalk
    /// </summary>
    public class SlackTalkOptions
    {
        /// <summary>
        /// The route that SlackTalk listens on for incoming slash commands
        /// </summary>
        public string CommandRoute { get; set; } = "/slack-talk/command";
        
        /// <summary>
        /// The route that SlackTalk listens on for incoming interactive message actions
        /// </summary>
        public string ActionRoute { get; set; } = "/slack-talk/action";
        
        /// <summary>
        /// The route that SlackTalk listens on for incoming event notifications
        /// </summary>
        public string EventRoute { get; set; } = "/slack-talk/event";

        /// <summary>
        /// Slack App verification token. Use this token to verify that requests are actually coming from Slack.
        /// </summary>
        public string VerificationToken { get; set; }
        
        /// <summary>
        /// Access token utilized anytime you use the <see cref="SlackTalkClient"/>.
        /// </summary>
        public string AccessToken { get; set; }
    }
}