using System.Collections.Generic;
using System.Threading.Tasks;
using SlackTalk.Models;

namespace SlackTalk
{
    /// <summary>
    /// A processor that contains the ability to handle/process a callback action
    /// </summary>
    public interface IHandlesActions
    {
        /// <summary>
        /// A hash set of callback_id's that this processor can process.
        /// </summary>
        HashSet<string> CallbackIds { get; }
        
        /// <summary>
        /// Process the given Slack action <paramref name="callback"/>.
        /// </summary>
        /// <param name="callback">The callback to process.</param>
        /// <returns>A response model containing the message along with flags to help the router process.</returns>
        Task<AppResponse> ProcessAsync(SlackCallback callback);
    }
}