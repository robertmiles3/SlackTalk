namespace Devalp.SlackTalk.Models
{
    /// <summary>
    /// Response model when POSTing to chat.postMessage
    /// </summary>
    public class SlackChatPostMessageResponse : SlackAPIResponse
    {
        /// <summary>
        /// The channel-like thing where the message was posted. 
        /// </summary>
        public string channel { get; set; }
        
        /// <summary>
        /// Timestamp Id. 
        /// </summary>
        public string ts { get; set; }
        
        /// <summary>
        /// Complete message object, as parsed by Slack servers. This may differ from the provided arguments as our servers sanitize links, attachments, and other properties. Your message may mutate. 
        /// </summary>
        public SlackMessage message { get; set; }
    }
}