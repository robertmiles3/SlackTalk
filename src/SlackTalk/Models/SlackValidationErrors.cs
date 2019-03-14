using System.Collections.Generic;

namespace SlackTalk.Models
{
    /// <summary>
    /// If your app finds any errors with the submission, respond with a SlackValidationErrors describing the elements and error messages. The API returns these errors to the user in-app, allowing the user to make corrections and submit again.
    /// </summary>
    public class SlackValidationErrors : ISlackSendable
    {
        /// <summary>
        /// A list of validation errors
        /// </summary>
        public List<SlackValidationErrors> errors { get; set; }
    }
}
