using System.Collections.Generic;
using System.Threading.Tasks;
using SlackTalk.Models;

namespace SlackTalk.Tests
{
    public class DummyProcessor : ISlackTalkProcessor, IHandlesCommands, IHandlesActions
    {
        public HashSet<string> Commands { get; } = new HashSet<string> { "/foo" };
        public HashSet<string> CallbackIds { get; } = new HashSet<string> { "foo_callback" };
        
        public Task<AppResponse> ProcessAsync(SlackCommand command)
        {
            return Task.FromResult(new AppResponse { ImmediateMessage = new SlackMessage { text = "DUMMY" } });
        }
        
        public Task<AppResponse> ProcessAsync(SlackCallback callback)
        {
            return Task.FromResult(new AppResponse { ImmediateMessage = new SlackMessage { text = "DUMMY" } });
        }
    }
}