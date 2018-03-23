namespace Devalp.SlackTalk.Models
{
    /// <summary>
    /// A collection of option fields. Used in static and external message menu data types. The value is especially important when used in selected_options and must match one of the previously provided options.
    /// </summary>
    public class SlackOption
    {
        /// <summary>
        /// A short, user-facing string to label this option to users. Use a maximum of 30 characters or so for best results across, you guessed it, form factors.
        /// </summary>
        public string text { get; set; }
        
        /// <summary>
        /// A short string that identifies this particular option to your application. It will be sent to your Action URL when this option is selected. While there's no limit to the value of your Slack app, this value may contain up to only 2000 characters.
        /// </summary>
        public string value { get; set; }
        
        /// <summary>
        /// A user-facing string that provides more details about this option. Also should contain up to 30 characters.
        /// </summary>
        public string description { get; set; }
    }
}