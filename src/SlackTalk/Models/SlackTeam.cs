namespace SlackTalk.Models
{
    /// <summary>
    /// A small set of string attributes about the workspace/team where this action occurred.
    /// </summary>
    public class SlackTeam
    {
        /// <summary>
        /// A unique identifier for the Slack workspace where the originating message appeared.
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// The slack.com subdomain of that same Slack workspace, like watermelonsugar.
        /// </summary>
        public string domain { get; set; }
    }
}
