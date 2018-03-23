using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Devalp.SlackTalk.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Devalp.SlackTalk
{
    /// <summary>
    /// A router interface for routing event notifications to an appropriate processor
    /// </summary>
    internal interface ISlackTalkRouter
    {
        /// <summary>
        /// Executes the router to find an appropriate processor for the incoming message
        /// </summary>
        /// <param name="type">The type of incoming message from Slack.</param>
        /// <param name="context">The <see cref="HttpContext"/> for the current request.</param>
        /// <param name="httpClient">The <see cref="HttpClient"/> instance to use for any network calls (to promote reuse)</param>
        /// <returns>A task that represents the execution of the router.</returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="context"/> or <paramref name="httpClient"/> is <c>null</c>.</exception>
        Task ProcessAsync(IncomingMessageType type, HttpContext context, HttpClient httpClient);
    }

    /// <summary>
    /// A router for routing incoming Slack notifications to an appropriate processor
    /// </summary>
    public class SlackTalkRouter : ISlackTalkRouter
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger _logger;
        private readonly string _verificationToken;

        /// <summary>
        /// Creates a new instance of <see cref="SlackTalkRouter"/>
        /// </summary>
        /// <param name="provider">The services provider to find the appropriate processor to handle the incoming notification.</param>
        /// <param name="options">The options for the router.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="provider"/> or <paramref name="options"/> is <c>null</c>.</exception>
        public SlackTalkRouter(IServiceProvider provider, IOptions<SlackTalkOptions> options, ILogger<SlackTalkRouter> logger)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _logger = logger;
            _ = options ?? throw new ArgumentNullException(nameof(options));
            _verificationToken = options.Value.VerificationToken;
        }

        /// <summary>
        /// Executes the router to find an appropriate processor for the incoming message
        /// </summary>
        /// <param name="type">The type of incoming message from Slack.</param>
        /// <param name="context">The <see cref="HttpContext"/> for the current request.</param>
        /// <param name="httpClient">The <see cref="HttpClient"/> instance to use for any network calls (to promote reuse)</param>
        /// <returns>A task that represents the execution of the router.</returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="context"/> or <paramref name="httpClient"/> is <c>null</c>.</exception>
        public async Task ProcessAsync(IncomingMessageType type, HttpContext context, HttpClient httpClient)
        {
            _ = context ?? throw new ArgumentNullException(nameof(context));
            _ = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            
            // Ensure it's a POST coming from Slack
            if (context.Request.Method != "POST")
            {
                context.Response.StatusCode = 404;
                return;
            }
            
            AppResponse response = null;
            var isRespondable = false;
            
            try
            {
                switch (type)
                {
                    case IncomingMessageType.Command:
                        isRespondable = true;
                        response = await ProcessCommandAsync(context);
                        break;
                    case IncomingMessageType.Action:
                        isRespondable = true;
                        response = await ProcessActionAsync(context);
                        break;
                    case IncomingMessageType.Event:
                        await ProcessEventAsync(context);
                        break;
                }
            }
            catch (Exception e)
            {
                var message = $"An exception occurred while processing this {type}";
                _logger.LogError(e, message, null);

                if (isRespondable)
                {
                    if (response == null)
                        response = new AppResponse();
                    response.ImmediateMessage = SlackMessage.CreateErrorMessage($"message: {e.Message}");
                    response.ResponseUrlMessages.Clear();
                    
                    context.Response.StatusCode = 200;
                }
                else
                {
                    context.Response.StatusCode = 500;
                }
            }

            if (!isRespondable)
                return;
            
            if (response.ImmediateMessage != null)
            {
                // Respond immediately with a message
                // Per Slack docs, this keeps the original message in the channel
                // See https://api.slack.com/slash-commands#delayed_responses_and_multiple_responses
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(response.ImmediateMessage.ToJson());
            }
            if (response.ResponseUrlMessages.Count > 0 && !string.IsNullOrEmpty(response.response_url))
            {
                foreach (var message in response.ResponseUrlMessages)
                {
                    // Send the message back to the response_url
                    // Per Slack docs, this replaces the original message in the channel
                    // See https://api.slack.com/slash-commands#delayed_responses_and_multiple_responses
                    var requestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri(response.response_url))
                    {
                        Content = new StringContent(message.ToJson(), Encoding.UTF8, "application/json")
                    };

                    try
                    {
                        var result = await httpClient.SendAsync(requestMessage);
                        if (!result.IsSuccessStatusCode)
                            throw new Exception($"Failed to post to Slack: Status Code \"{result.StatusCode}\" ({(int)result.StatusCode}).");
                    }
                    catch (Exception e)
                    {
                        // Something happened posting back to the response_url, so log the error
                        _logger.LogError(e, "An exception occurred while sending a message to the response_url", null);
                    }
                }
            }
        }
        
        private async Task<AppResponse> ProcessCommandAsync(HttpContext context)
        {
            // Get the request form body as a SlackCommand
            var command = context.Request.Form.AsSlackCommand();
            if (command == null)
                throw new Exception("Invalid form content");

            // Verify the incoming token
            if (command.token != _verificationToken)
                throw new Exception("Invalid token.");

            // Attempt to find the processor in the DI container that matches this type
            var services = _provider.GetServices<ISlackTalkProcessor>();
            if (!(services.FirstOrDefault(p => p is IHandlesCommands commandProcessor && commandProcessor.Commands.Contains(command.command)) is IHandlesCommands processor))
                throw new Exception("No registered processor found to handle this command");
                    
            // Process the command
            var response = await processor.ProcessAsync(command);
            
            if (response == null)
                throw new Exception("Processor failed to process this command");
            
            response.response_url = command.response_url;
            return response;
        }

        private async Task<AppResponse> ProcessActionAsync(HttpContext context)
        {
            // Get the request form body as a SlackCallback
            var callback = context.Request.Form.AsSlackCallback();
            if (callback == null)
                throw new Exception("Invalid form content");
                    
            // Verify the incoming token
            if (callback.token != _verificationToken)
                throw new Exception("Invalid token.");
                    
            // Attempt to find the processor in the DI container that matches this type
            var services = _provider.GetServices<ISlackTalkProcessor>();
            if (!(services.FirstOrDefault(p => p is IHandlesActions actionProcessor && actionProcessor.CallbackIds.Contains(callback.callback_id)) is IHandlesActions processor))
                throw new Exception("No registered processor found to handle this action");
                    
            // Process the callback
            var response = await processor.ProcessAsync(callback);
            
            if (response == null)
                throw new Exception("Processor failed to process this action");
            
            response.response_url = callback.response_url;
            return response;
        }

        private async Task<bool> ProcessEventAsync(HttpContext context)
        {
            try
            {
                // Read the POST body
                string body;
                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    body = reader.ReadToEnd();
                }
                if (string.IsNullOrWhiteSpace(body))
                    throw new Exception("Invalid body content");

                // Deserialize the body into a SlackEventOuter
                var outerEvent = body.FromJson<SlackEventOuter>();
                if (outerEvent == null)
                    throw new Exception("Failed to deserialize body json");

                // Verify the incoming token
                if (outerEvent.token != _verificationToken)
                    throw new Exception("Invalid token.");

                // If a url_verification event, respond appropriately with challenge
                if (outerEvent.type == "url_verification")
                {
                    context.Response.StatusCode = 200;
                    context.Response.ContentType = "text/plain";
                    await context.Response.WriteAsync(outerEvent.challenge);
                    return true;
                }
                
                // Attempt to find the processor in the DI container that matches this type
                var services = _provider.GetServices<ISlackTalkProcessor>();
                if (!(services.FirstOrDefault(p => p is IHandlesEvents eventProcessor && eventProcessor.EventTypes.Contains(outerEvent.Event.type)) is IHandlesEvents processor))
                    throw new Exception("No registered processor found to handle this command");
                
                // Process the command
                await processor.ProcessAsync(outerEvent);

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}