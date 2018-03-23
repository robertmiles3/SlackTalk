using System.Collections.Generic;
using System.Threading.Tasks;
using Devalp.SlackTalk.Models;

namespace Devalp.SlackTalk
{
    /// <summary>
    /// A processor that contains the ability to handle/process an event notification
    /// </summary>
    public interface IHandlesEvents
    {
        /// <summary>
        /// A hash set of event types that this processor can process.
        /// </summary>
        HashSet<string> EventTypes { get; }
        
        /// <summary>
        /// Process the given Slack event <paramref name="eventOuter"/>.
        /// </summary>
        /// <param name="eventOuter">The event to process.</param>
        /// <returns>A boolean to let the router know whether the processing succeeded or not.</returns>
        Task<bool> ProcessAsync(SlackEventOuter eventOuter);
    }
}