using System.Collections.Generic;
using System.Threading.Tasks;
using SlackTalk.Models;

namespace SlackTalk.Tests
{
    public class FailingProcessor : ISlackTalkProcessor, IHandlesCommands, IHandlesActions
    {
        public HashSet<string> Commands { get; } = new HashSet<string> { "/foo" };
        public HashSet<string> CallbackIds { get; } = new HashSet<string> { "foo_callback" };
        
        public Task<AppResponse> ProcessAsync(SlackCommand command) => Task.FromResult((AppResponse)null);
        
        public Task<AppResponse> ProcessAsync(SlackCallback callback) => Task.FromResult((AppResponse)null);
    }
}