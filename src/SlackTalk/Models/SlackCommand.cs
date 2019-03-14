namespace SlackTalk.Models
{
    /// <summary>
    /// The slash command that is posted from Slack. They typically trigger an action (like posting a gif, or starting a video conference, or adding something new to your todo list).
    /// </summary>
    public class SlackCommand
    {
        /// <summary>
        /// The token is an additional identifier that's sent with the slash command that you could use to verify that what's calling your script is actually your slash command.
        /// </summary>
        public string token { get; set; }
        
        /// <summary>
        /// The Slack team ID of the executing workspace
        /// </summary>
        public string team_id { get; set; }
        
        /// <summary>
        /// The Slack team domain of the executing workspace
        /// </summary>
        public string team_domain { get; set; }
        
        /// <summary>
        /// The enterprise ID, when the executing workspace is part of an Enterprise Grid.
        /// </summary>
        public string enterprise_id { get; set; }
        
        /// <summary>
        /// The enterprise name, when the executing workspace is part of an Enterprise Grid.
        /// </summary>
        public string enterprise_name { get; set; }
        
        /// <summary>
        /// The channel ID is the channel where the slash command was issued.
        /// </summary>
        public string channel_id { get; set; }
        
        /// <summary>
        /// The channel name is the channel name where the slash command was issued.
        /// </summary>
        public string channel_name { get; set; }
        
        /// <summary>
        /// The user ID is the user's ID who entered the command
        /// </summary>
        public string user_id { get; set; }
        
        /// <summary>
        /// The user name is the user's name who entered the command
        /// </summary>
        public string user_name { get; set; }
        
        /// <summary>
        /// The slash command that was entered
        /// </summary>
        public string command { get; set; }
        
        /// <summary>
        /// The text that was entered with the command
        /// </summary>
        public string text { get; set; }
        
        /// <summary>
        /// </summary>
        public string response_url { get; set; }
        
        /// <summary>
        /// A trigger_id is an artifact of interaction between the user and your app that can be used to request upgraded permissions. Use it soon after receiving to draw a theoretical dotted line between the interaction and your requesting permissions.
        /// </summary>
        public string trigger_id { get; set; }
    }
}