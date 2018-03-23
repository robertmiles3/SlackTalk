namespace Devalp.SlackTalk.Models
{
    /// <summary>
    /// A SlackConfirm is a confirmation warning to protect users from destructive actions or particularly distinguished decisions by asking them to confirm their button click one more time. Use confirmation dialogs with care.
    /// </summary>
    public class SlackConfirm
    {
        /// <summary>
        /// Title the pop up window. Please be brief.
        /// </summary>
        public string title { get; set; }
        
        /// <summary>
        /// Describe in detail the consequences of the action and contextualize your button text choices. Use a maximum of 30 characters or so for best results across form factors.
        /// </summary>
        public string text { get; set; }
        
        /// <summary>
        /// The text label for the button to continue with an action. Keep it short. Defaults to "Okay".
        /// </summary>
        public string ok_text { get; set; }
        
        /// <summary>
        /// The text label for the button to cancel the action. Keep it short. Defaults to "Cancel".
        /// </summary>
        public string dismiss_text { get; set; }
    }
}