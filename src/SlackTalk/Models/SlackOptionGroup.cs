using System.Collections.Generic;

namespace Devalp.SlackTalk.Models
{
    /// <summary>
    /// Options groups are a set of 100 options divided into groups. They can be used with static or external message menu data types.
    /// </summary>
    public class SlackOptionGroup
    {
        /// <summary>
        ///     A short, user-facing string to label this option to users.
        /// </summary>
        public string text { get; set; }
        
        /// <summary>
        /// The individual options to appear in this message menu, provided as an array of option fields. Required when data_source is default or otherwise unspecified.
        /// </summary>
        public List<SlackOption> options { get; set; }
    }
}