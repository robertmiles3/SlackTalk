using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Devalp.SlackTalk.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Devalp.SlackTalk
{
    /// <summary>
    /// A client for interacting with the Slack API
    /// </summary>
    public class SlackTalkClient : ISlackTalkClient
    {
        private readonly HttpClient _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://slack.com/api/")
        };
        private readonly ILogger _logger;
        private readonly string _bearerAuthenticationToken;

        /// <summary>
        /// A client for interacting with the Slack API
        /// </summary>
        /// <param name="options">The options for the client.</param>
        /// <param name="logger">The logger.</param>
        public SlackTalkClient(IOptions<SlackTalkOptions> options, ILogger<SlackTalkClient> logger)
        {
            _bearerAuthenticationToken = $"Bearer {options.Value.AccessToken}";
            _logger = logger;
        }

        /// <summary>
        /// Sends a message to a channel.
        /// </summary>
        /// <param name="message">The message to send.</param>
        public async Task<SlackChatPostMessageResponse> ChatPostMessage(SlackMessage message)
        {
            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, "chat.postMessage");
                var stringContent = new StringContent(message.ToJson(), Encoding.UTF8, "application/json");
                requestMessage.Headers.Add("Authorization", _bearerAuthenticationToken);
                requestMessage.Content = stringContent;
                var response = await _httpClient.SendAsync(requestMessage);
                var rawContent = await response.Content.ReadAsStringAsync();
                
                if (!response.IsSuccessStatusCode)
                    throw new Exception($"StatusCode {(int)response.StatusCode}. Raw content = {rawContent}");

                return rawContent.FromJson<SlackChatPostMessageResponse>();
            }
            catch (Exception e)
            {
                _logger.LogError(e, null);
                throw;
            }
        }
    }

    /// <summary>
    /// A client for interacting with the Slack API
    /// </summary>
    public interface ISlackTalkClient
    {
        /// <summary>
        /// Sends a message to a channel.
        /// </summary>
        /// <param name="message">The message to send.</param>
        Task<SlackChatPostMessageResponse> ChatPostMessage(SlackMessage message);
    }
}