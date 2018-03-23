using System.Collections.Generic;
using System.Threading.Tasks;
using Devalp.SlackTalk.Models;

namespace Devalp.SlackTalk.Tests
{
    public class NullMessageProcessor : ISlackTalkProcessor, IHandlesCommands, IHandlesActions
    {
        public HashSet<string> Commands { get; } = new HashSet<string> { "/foo" };
        public HashSet<string> CallbackIds { get; } = new HashSet<string> { "foo_callback" };
        
        public Task<AppResponse> ProcessAsync(SlackCommand command) => Task.FromResult(new AppResponse());
        
        public Task<AppResponse> ProcessAsync(SlackCallback callback) => Task.FromResult(new AppResponse());
    }
}