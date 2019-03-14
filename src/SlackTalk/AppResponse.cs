using System.Collections.Generic;
using SlackTalk.Models;

namespace SlackTalk
{
    /// <summary>
    /// A response coming back from a processor that informs the router how and what to send back
    /// </summary>
    public class AppResponse
    {
        /// <summary>
        /// The message to responding directly to the incoming invocation request. Your provided message will replace the existing message.
        /// </summary>
        public ISlackSendable ImmediateMessage { get; set; }
        
        /// <summary>
        /// A list of messages that should be sent back to the response_url of the incoming invocation request. These messages will NOT replace the existing message.
        /// </summary>
        public List<ISlackSendable> ResponseUrlMessages { get; set; } = new List<ISlackSendable>();

        internal string response_url { get; set; } 
    }
}