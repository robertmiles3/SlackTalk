using System.Collections.Generic;
using System.Threading.Tasks;
using Devalp.SlackTalk.Models;

namespace Devalp.SlackTalk
{
    /// <summary>
    /// A processor that contains the ability to handle/process a Slack slash command
    /// </summary>
    public interface IHandlesCommands
    {
        /// <summary>
        /// A hash set of commands that this processor can process.
        /// </summary>
        HashSet<string> Commands { get; }
        
        /// <summary>
        /// Process the given Slack slash <paramref name="command"/>.
        /// </summary>
        /// <param name="command">The command to process.</param>
        /// <returns>A response model containing the message along with flags to help the router process.</returns>
        Task<AppResponse> ProcessAsync(SlackCommand command);
    }
}