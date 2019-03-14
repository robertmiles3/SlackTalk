using System.Collections.Generic;

namespace SlackTalk.Models
{
    /// <summary>
    /// SlackMessage is the top-level of messages. Messages act as delivery vehicle for all interactive message experiences. Use them not only when initiating messages, but also when updating or creating evolving workflows.
    /// </summary>
    public class SlackMessage : ISlackSendable
    {     
        /// <summary>
        /// The basic text of the message. Only required if the message contains zero attachments. Provide no more than 40,000 characters or risk truncation.
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
        /// Authentication token bearing required scopes. Used only with <see cref="SlackTalkClient"/>.
        /// </summary>
        public string token { get; set; }
        
        /// <summary>
        /// Channel, private group, or IM channel to send message to. Can be an encoded ID, or a name.
        /// </summary>
        public string channel { get; set; }
        
        /// <summary>
        /// Pass true to post the message as the authed user, instead of as a bot. Defaults to false.
        /// </summary>
        public bool? as_user { get; set; }
        
        /// <summary>
        /// Emoji to use as the icon for this message. Overrides icon_url. Must be used in conjunction with as_user set to false, otherwise ignored.
        /// </summary>
        public string icon_emoji { get; set; }
        
        /// <summary>
        /// URL to an image to use as the icon for this message. Must be used in conjunction with <see cref="as_user"/> set to false, otherwise ignored. 
        /// </summary>
        public string icon_url { get; set; }
        
        /// <summary>
        /// Disable Slack markup parsing by setting to false. Enabled by default.
        /// </summary>
        public bool? mrkdwn { get; set; }
        
        /// <summary>
        /// Change how messages are treated. Defaults to none. 
        /// </summary>
        public string parse { get; set; }
        
        /// <summary>
        /// Used in conjunction with thread_ts and indicates whether reply should be made visible to everyone in the channel or conversation. Defaults to false.
        /// </summary>
        public bool? reply_broadcast { get; set; }
        
        /// <summary>
        /// Pass true to enable unfurling of primarily text-based content.
        /// </summary>
        public bool? unfurl_links { get; set; }
        
        /// <summary>
        /// Pass false to disable unfurling of media content.
        /// </summary>
        public bool? unfurl_media { get; set; }
        
        /// <summary>
        /// Set your bot's user name. Must be used in conjunction with <see cref="as_user"/> set to false, otherwise ignored.  
        /// </summary>
        public string username { get; set; }
        
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