using System.Collections.Generic;
using System.Threading.Tasks;
using Devalp.SlackTalk.Models;

namespace Devalp.SlackTalk.Tests
{
    public class DelayedResponseProcessor : ISlackTalkProcessor, IHandlesCommands, IHandlesActions
    {
        public HashSet<string> Commands { get; } = new HashSet<string> { "/foo" };
        public HashSet<string> CallbackIds { get; } = new HashSet<string> { "foo_callback" };
        
        public Task<AppResponse> ProcessAsync(SlackCommand command)
        {
            var response = new AppResponse();
            response.ResponseUrlMessages.Add(new SlackMessage { text = "DUMMY" });
            return Task.FromResult(response);
        }
        
        public Task<AppResponse> ProcessAsync(SlackCallback callback)
        {
            var response = new AppResponse();
            response.ResponseUrlMessages.Add(new SlackMessage { text = "DUMMY" });
            return Task.FromResult(response);
        }
    }
}