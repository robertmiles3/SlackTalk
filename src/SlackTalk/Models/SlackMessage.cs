using System.Collections.Generic;

namespace Devalp.SlackTalk.Models
{
    /// <summary>
    /// SlackMessage is the top-level of messages. Messages act as delivery vehicle for all interactive message experiences. Use them not only when initiating messages, but also when updating or creating evolving workflows.
    /// </summary>
    public class SlackMessage : ISlackSendable
    {     
        /// <summary>
        /// The basic text of the message. Only required if the message contains zero attachments.
        /// </summary>
        public string text { get; set; }
        
        /// <summary>
        /// Provide a JSON array of attachment objects. Adds additional components to the message. Messages should contain no more than 20 attachments.
        /// </summary>
        public List<SlackAttachment> attachments { get; set; }

        /// <summary>
        /// When replying to a parent message, this value is the ts value of the parent message to the thread. See message threading for more context.
        /// </summary>
        public string thread_ts { get; set; }

        /// <summary>
        /// Expects one of two values:
        ///     in_channel — display the message to all users in the channel where a message button was clicked. Messages sent in response to invoked button actions are set to in_channel by default.
        ///     ephemeral — display the message only to the user who clicked a message button. Messages sent in response to Slash commands are set to ephemeral by default.
        /// This field cannot be specified for a brand new message and must be used only in response to the execution of message button action or a slash command response. Once a response_type is set, it cannot be changed when updating the message.
        /// </summary>
        public string response_type { get; set; }

        /// <summary>
        /// Used only when creating messages in response to a button action invocation. When set to true, the inciting message will be replaced by this message you're providing. When false, the message you're providing is considered a brand new message.
        /// </summary>
        public bool? replace_original { get; set; }

        /// <summary>
        /// Used only when creating messages in response to a button action invocation. When set to true, the inciting message will be deleted and if a message is provided, it will be posted as a brand new message.
        /// </summary>
        public bool? delete_original { get; set; }
        
        /// <summary>
        /// Helper to create a SlackMessage with response_type set to "in_channel"
        /// </summary>
        public static SlackMessage CreateInChannelMessage() => new SlackMessage { response_type = "in_channel" };
        
        /// <summary>
        /// Helper to create a SlackMessage with response_type set to "ephemeral"
        /// </summary>
        public static SlackMessage CreateEphemeralMessage(string message = null) => new SlackMessage { response_type = "ephemeral", text = message };
        
        /// <summary>
        /// Helper to create a SlackMessage with response_type set to "ephemeral" and basic error text
        /// </summary>
        public static SlackMessage CreateErrorMessage(string error) => new SlackMessage { response_type = "ephemeral", text = error };
    }
}