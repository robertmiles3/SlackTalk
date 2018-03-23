using System.Collections.Generic;

namespace Devalp.SlackTalk.Models
{
    /// <summary>
    /// SlackAttachments are used to add additional components to the message/SlackMessage 
    /// </summary>
    public class SlackAttachment
    {
        /// <summary>
        /// Provide this attachment with a visual header by providing a short string here.
        /// </summary>
        public string title { get; set; }
        
        /// <summary>
        /// A plaintext message displayed to users using an interface that does not support attachments or interactive messages. Consider leaving a URL pointing to your service if the potential message actions are representable outside of Slack. Otherwise, let folks know what they are missing.
        /// </summary>
        public string fallback { get; set; }

        /// <summary>
        /// The provided string will act as a unique identifier for the collection of buttons within the attachment. It will be sent back to your message button action URL with each invoked action. This field is required when the attachment contains message buttons. It is key to identifying the interaction you're working with.
        /// </summary>
        public string callback_id { get; set; }
        
        /// <summary>
        /// Used to visually distinguish an attachment from other messages. Accepts hex values and a few named colors as documented in attaching content to messages. Use sparingly and according to best practices.
        /// </summary>
        public string color { get; set; }
        
        /// <summary>
        /// Optional text that appears above the attachment block
        /// </summary>
        public string pretext { get; set; }
        
        /// <summary>
        /// By passing a valid URL in the title_link parameter (optional), the title text will be hyperlinked.
        /// </summary>
        public string title_link { get; set; }
        
        /// <summary>
        /// This is the main text in a message attachment, and can contain standard message markup. The content will automatically collapse if it contains 700+ characters or 5+ linebreaks, and will display a "Show more..." link to expand the content. Links posted in the text field will not unfurl.
        /// </summary>
        public string text { get; set; }
        
        /// <summary>
        /// Some brief text to help contextualize and identify an attachment. Limited to 300 characters, and may be truncated further when displayed to users in environments with limited screen real estate.
        /// </summary>
        public string footer { get; set; }
        
        /// <summary>
        /// By providing the ts field with an integer value in "epoch time", the attachment will display an additional timestamp value as part of the attachment's footer.
        /// </summary>
        public long? ts { get; set; }
        
        /// <summary>
        /// A collection of actions (buttons or menus) to include in the attachment. Required when using message buttons or message menus. A maximum of 5 actions per attachment may be provided.
        /// </summary>
        public List<SlackAction> actions { get; set; }
    }
}