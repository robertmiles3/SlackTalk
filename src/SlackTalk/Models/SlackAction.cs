using System.Collections.Generic;

namespace SlackTalk.Models
{
    /// <summary>
    /// A SlackAction is either a button or select that can be attached to a SlackMessage's attachments
    /// </summary>
    public class SlackAction
    {
        /// <summary>
        /// Provide a string to give this specific action a name. The name will be returned to your Action URL along with the message's callback_id when this action is invoked. Use it to identify this particular response path. If multiple actions share the same name, only one of them can be in a triggered state.
        /// </summary>
        public string name { get; set; }
        
        /// <summary>
        /// The user-facing label for the message button or menu representing this action. Cannot contain markup. Best to keep these short and decisive. Use a maximum of 30 characters or so for best results across form factors.
        /// </summary>
        public string text { get; set; }
        
        /// <summary>
        /// Provide "button" when this action is a message button or provide "select" when the action is a message menu.
        /// </summary>
        public string type { get; set; }
        
        /// <summary>
        /// Provide a string identifying this specific action. It will be sent to your Action URL along with the name and attachment's callback_id. If providing multiple actions with the same name, value can be strategically used to differentiate intent. Your value may contain up to 2000 characters.
        /// </summary>
        public string value { get; set; }

        /// <summary>
        /// If you provide a JSON hash of confirmation fields, your button or menu will pop up a dialog with your indicated text and choices, giving them one last chance to avoid a destructive action or other undesired outcome.
        /// </summary>
        public SlackConfirm confirm { get; set; }
        
        /// <summary>
        /// The url destination for the button action
        /// </summary>
        public string url { get; set; }
        
        /// <summary>
        /// Used only with message buttons, this decorates buttons with extra visual importance, which is especially useful when providing logical default action or highlighting a destructive activity.
        ///     default — Yes, it's the default. Buttons will look simple.
        ///     primary — Use this sparingly, when the button represents a key action to accomplish. You should probably only ever have one primary button within a set.
        ///     danger — Use this when the consequence of the button click will result in the destruction of something, like a piece of data stored on your servers. Use even more sparingly than primary.
        /// </summary>
        public string style { get; set; } // primary, danger, blank for default

        /// <summary>
        /// Used only with message menus. The individual options to appear in this menu, provided as an array of option fields. Required when data_source is static or otherwise unspecified. A maximum of 100 options can be provided in each menu.
        /// </summary>
        public List<SlackOption> options { get; set; }

        /// <summary>
        /// Used only with message menus. An alternate, semi-hierarchal way to list available options. Provide an array of option group definitions. This replaces and supersedes the options array.
        /// </summary>
        public List<SlackOptionGroup> option_groups { get; set; }

        /// <summary>
        /// Accepts static, users, channels, conversations, or external. Our clever default behavior is default, which means the menu's options are provided directly in the posted message under options. Defaults to static.
        /// </summary>
        public string data_source { get; set; }

        /// <summary>
        /// If provided, the first element of this array will be set as the pre-selected option for this menu. Any additional elements will be ignored. 
        /// The selected option's value field is contextual based on menu type and is always required:
        ///    - For menus of type static (the default) this should be in the list of options included in the action.
        ///    - For menus of type users, channels, or conversations, this should be a valid ID of the corresponding type.
        ///    - For menus of type external this can be any value, up to a couple thousand characters.
        /// </summary>
        public List<SlackOption> selected_options { get; set; }

        /// <summary>
        /// Only applies when data_source is set to external. If present, Slack will wait till the specified number of characters are entered before sending a request to your app's external suggestions API endpoint. Defaults to 1.
        /// </summary>
        public int? min_query_length { get; set; }
    }
}