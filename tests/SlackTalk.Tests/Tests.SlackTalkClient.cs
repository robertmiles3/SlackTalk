using System;
using System.Threading.Tasks;
using Devalp.SlackTalk.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Devalp.SlackTalk.Tests
{
    public partial class Tests
    {
        // Read from environment variable so that unit testing token doesn't creep into git
        private static readonly string _accessToken = Environment.GetEnvironmentVariable("slacktalk_tests_access_token");
            
        [Fact(DisplayName = "API_chat_postMessage")]
        public async Task API_chat_postMessage()
        {
            var options = TestHelper.GetOptions();
            options.Value.AccessToken = _accessToken;
            var client = new SlackTalkClient(options, new Mock<ILogger<SlackTalkClient>>().Object);

            var message = new SlackMessage
            {
                text = "some message",
                channel = "#testing"
            };
            var result = await client.ChatPostMessageAsync(message);

            Assert.True(result.ok);
            Assert.Equal(message.text, result.message.text);
        }
    }
}
