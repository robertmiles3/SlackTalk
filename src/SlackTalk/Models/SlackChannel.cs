namespace Devalp.SlackTalk.Models
{
    /// <summary>
    /// Where it all happened — the user inciting this action clicked a button on a message contained within a channel, and this hash presents attributed about that channel.
    /// </summary>
    public class SlackChannel
    {
        /// <summary>
        /// A string identifier for the channel housing the originating message. Channel IDs are unique to the workspace they appear within.
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// The name of the channel the message appeared in, without the leading # character.
        /// </summary>
        public string name { get; set; }
    }
}
