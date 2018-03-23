using System.Collections.Generic;

namespace Devalp.SlackTalk.Models
{
    /// <summary>
    /// SlackCallback contains the fields delivered to your action URL whenever a button is pressed or a menu option is selected.
    /// </summary>
    public class SlackCallback
    {
        /// <summary>
        /// Use this string to determine where the invoked action originates from, like an interactive_message or a dialog_submission.
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// A dictionary of key/value pairs representing the user's submission. Each key is a name field your app provided when composing the form. Each value is the user's submitted value, or in the case of a select menu, the value you assigned to a specific response.
        /// </summary>
        public Dictionary<string, string> submission { get; set; }

        /// <summary>
        /// An array of actions that were clicked, including the name and value of the actions, as you prepared when creating your message buttons. Though presented as an array, at this time you'll only receive a single action per incoming invocation.
        /// </summary>
        public List<SlackAction> actions { get; set; }

        /// <summary>
        /// The string you provided in the original message attachment as the callback_id. Use this to identify the specific set of actions/buttons originally posed. If the value of an action is the answer, callback_id is the specific question that was asked. No more than 200 or so characters please.
        /// </summary>
        public string callback_id { get; set; }

        /// <summary>
        /// A small set of string attributes about the workspace/team where this action occurred.
        /// </summary>
        public SlackTeam team { get; set; }

        /// <summary>
        /// Where it all happened — the user inciting this action clicked a button on a message contained within a channel, and this hash presents attributed about that channel.
        /// </summary>
        public SlackChannel channel { get; set; }

        /// <summary>
        /// The clicker! The action-invoker! The button-presser! These attributes tell you all about the user who decided to interact your message.
        /// </summary>
        public SlackUser user { get; set; }

        /// <summary>
        /// The time when the action occurred, expressed in decimal epoch time, wrapped in a string. Like "1458170917.164398".
        /// </summary>
        public string action_ts { get; set; }

        /// <summary>
        /// The time when the message containing the action was posted, expressed in decimal epoch time, wrapped in a string. Like "1458170917.164398".
        /// </summary>
        public string message_ts { get; set; }

        /// <summary>
        /// A 1-indexed identifier for the specific attachment within a message that contained this action. In case you were curious or building messages containing buttons within many attachments.
        /// </summary>
        public string attachment_id { get; set; }

        /// <summary>
        /// This is the same string you received when configuring your application for interactive message support, presented to you on an app details page. Validate this to ensure the request is coming to you from Slack.
        /// </summary>
        public string token { get; set; }

        /// <summary>
        /// The original message that triggered this action. This is especially useful if you don't retain state or need to know the message's message_ts for use with chat.update This value is not provided for ephemeral messages.
        /// </summary>
        public SlackMessage original_message { get; set; }

        /// <summary>
        /// A string containing a URL, used to respond to this invocation independently from the triggering of your action URL.
        /// </summary>
        public string response_url { get; set; }
    }
}