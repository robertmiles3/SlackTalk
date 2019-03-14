using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using SlackTalk.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Moq;
using Xunit;

namespace SlackTalk.Tests
{
    public partial class Tests
    {
        [Fact(DisplayName = "Can_Convert_Form_To_SlackCommand")]
        public void Can_Convert_Form_To_SlackCommand()
        {
            var formDictionary = new Dictionary<string, StringValues>
            {
                { "token", "some_token" },
                { "team_id", "some_team_id" },
                { "team_domain", "some_team_domain" },
                { "enterprise_id", "some_enterprise_id" },
                { "enterprise_name", "some_enterprise_name" },
                { "channel_id", "some_channel_id" },
                { "channel_name", "some_channel_name" },
                { "user_id", "some_user_id" },
                { "user_name", "some_user_name" },
                { "command", "some_command" },
                { "text", "some_text" },
                { "response_url", "some_response_url" },
                { "trigger_id", "some_trigger_id" },
            };
            var formCollection = new FormCollection(formDictionary);

            var command = formCollection.AsSlackCommand();
            
            Assert.Equal(formDictionary[nameof(command.token)], command.token);
            Assert.Equal(formDictionary[nameof(command.team_id)], command.team_id);
            Assert.Equal(formDictionary[nameof(command.team_domain)], command.team_domain);
            Assert.Equal(formDictionary[nameof(command.enterprise_id)], command.enterprise_id);
            Assert.Equal(formDictionary[nameof(command.enterprise_name)], command.enterprise_name);
            Assert.Equal(formDictionary[nameof(command.channel_id)], command.channel_id);
            Assert.Equal(formDictionary[nameof(command.channel_name)], command.channel_name);
            Assert.Equal(formDictionary[nameof(command.user_id)], command.user_id);
            Assert.Equal(formDictionary[nameof(command.user_name)], command.user_name);
            Assert.Equal(formDictionary[nameof(command.command)], command.command);
            Assert.Equal(formDictionary[nameof(command.text)], command.text);
            Assert.Equal(formDictionary[nameof(command.response_url)], command.response_url);
            Assert.Equal(formDictionary[nameof(command.trigger_id)], command.trigger_id);
        }
        
        [Fact(DisplayName = "Can_Convert_Empty_Form_To_SlackCommand")]
        public void Can_Convert_Empty_Form_To_SlackCommand()
        {
            var formDictionary = new Dictionary<string, StringValues>();
            var formCollection = new FormCollection(formDictionary);

            var command = formCollection.AsSlackCommand();
            
            Assert.Equal(string.Empty, command.token);
            Assert.Equal(string.Empty, command.team_id);
            Assert.Equal(string.Empty, command.team_domain);
            Assert.Equal(string.Empty, command.enterprise_id);
            Assert.Equal(string.Empty, command.enterprise_name);
            Assert.Equal(string.Empty, command.channel_id);
            Assert.Equal(string.Empty, command.channel_name);
            Assert.Equal(string.Empty, command.user_id);
            Assert.Equal(string.Empty, command.user_name);
            Assert.Equal(string.Empty, command.command);
            Assert.Equal(string.Empty, command.text);
            Assert.Equal(string.Empty, command.response_url);
            Assert.Equal(string.Empty, command.trigger_id);
        }
        
        [Fact(DisplayName = "Command_Handles_Invalid_Token")]
        public async Task Command_Handles_Invalid_Token()
        {
            var router = new SlackTalkRouter(
                scopeFactory: new Mock<IServiceScopeFactory>().Object, 
                options: TestHelper.GetOptions(),
                logger: new Mock<ILogger<SlackTalkRouter>>().Object);

            var context = TestHelper.GetIncomingCommandContext(new SlackCommand { command = "/some_command", token = "some_token" });

            await router.ProcessAsync(IncomingMessageType.Command, context, TestHelper.GetHttpClient(string.Empty));
            var error = context.Response.Body.ReadAsString().FromJson<SlackMessage>();

            Assert.Contains("Invalid token", error.text, StringComparison.OrdinalIgnoreCase);
        }
        
        [Fact(DisplayName = "Command_Handles_Missing_Processor")]
        public async Task Command_Handles_Missing_Processor()
        {
            var router = new SlackTalkRouter(
                scopeFactory: TestHelper.GetServiceScopeFactory(new List<ISlackTalkProcessor>(0)), 
                options: TestHelper.GetOptions(),
                logger: new Mock<ILogger<SlackTalkRouter>>().Object);
            
            var context = TestHelper.GetIncomingCommandContext(new SlackCommand { command = "/some_command", token = "bar" });

            await router.ProcessAsync(IncomingMessageType.Command, context, TestHelper.GetHttpClient(string.Empty));
            var error = context.Response.Body.ReadAsString().FromJson<SlackMessage>();

            Assert.Contains("No registered processor found to handle this command", error.text, StringComparison.OrdinalIgnoreCase);
        }
        
        [Fact(DisplayName = "Command_Handles_Invalid_Command")]
        public async Task Command_Handles_Invalid_Command()
        {
            var router = new SlackTalkRouter(
                scopeFactory: TestHelper.GetServiceScopeFactory(new List<ISlackTalkProcessor> { new DummyProcessor() }), 
                options: TestHelper.GetOptions(),
                logger: new Mock<ILogger<SlackTalkRouter>>().Object);

            var context = TestHelper.GetIncomingCommandContext(new SlackCommand { command = "/some_command", token = "bar" });

            await router.ProcessAsync(IncomingMessageType.Command, context, TestHelper.GetHttpClient(string.Empty));
            var error = context.Response.Body.ReadAsString().FromJson<SlackMessage>();

            Assert.Contains("No registered processor found to handle this command", error.text, StringComparison.OrdinalIgnoreCase);
        }

        [Fact(DisplayName = "Command_Handles_Failing_Processor")]
        public async Task Command_Handles_Failing_Processor()
        {
            var router = new SlackTalkRouter(
                scopeFactory: TestHelper.GetServiceScopeFactory(new List<ISlackTalkProcessor> { new FailingProcessor() }), 
                options: TestHelper.GetOptions(),
                logger: new Mock<ILogger<SlackTalkRouter>>().Object);

            var context = TestHelper.GetIncomingCommandContext(new SlackCommand { command = "/foo", token = "bar" });

            await router.ProcessAsync(IncomingMessageType.Command, context, TestHelper.GetHttpClient(string.Empty));
            var error = context.Response.Body.ReadAsString().FromJson<SlackMessage>();

            Assert.Contains("Processor failed", error.text, StringComparison.OrdinalIgnoreCase);
        }
        
        [Fact(DisplayName = "Command_Handles_Immediate_Response")]
        public async Task Command_Handles_Immediate_Response()
        {
            var router = new SlackTalkRouter(
                scopeFactory: TestHelper.GetServiceScopeFactory(new List<ISlackTalkProcessor> { new DummyProcessor() }), 
                options: TestHelper.GetOptions(),
                logger: new Mock<ILogger<SlackTalkRouter>>().Object);

            var context = TestHelper.GetIncomingCommandContext(new SlackCommand { command = "/foo", token = "bar" });

            await router.ProcessAsync(IncomingMessageType.Command, context, TestHelper.GetHttpClient(string.Empty));
            var error = context.Response.Body.ReadAsString().FromJson<SlackMessage>();

            Assert.Contains("DUMMY", error.text, StringComparison.OrdinalIgnoreCase);
        }
        
        [Fact(DisplayName = "Command_Handles_Null_Message")]
        public async Task Command_Handles_Null_Message()
        {
            var router = new SlackTalkRouter(
                scopeFactory: TestHelper.GetServiceScopeFactory(new List<ISlackTalkProcessor> { new NullMessageProcessor() }), 
                options: TestHelper.GetOptions(),
                logger: new Mock<ILogger<SlackTalkRouter>>().Object);

            var context = TestHelper.GetIncomingCommandContext(new SlackCommand { command = "/foo", token = "bar" });

            await router.ProcessAsync(IncomingMessageType.Command, context, TestHelper.GetHttpClient(string.Empty));
            var body = context.Response.Body.ReadAsString();

            Assert.Equal(200, context.Response.StatusCode);
            Assert.Equal(string.Empty, body);
        }
        
        [Fact(DisplayName = "Command_Handles_Delayed_Response_Success")]
        public async Task Command_Handles_Delayed_Response_Success()
        {
            var router = new SlackTalkRouter(
                scopeFactory: TestHelper.GetServiceScopeFactory(new List<ISlackTalkProcessor> { new DelayedResponseProcessor() }), 
                options: TestHelper.GetOptions(),
                logger: new Mock<ILogger<SlackTalkRouter>>().Object);

            var context = TestHelper.GetIncomingCommandContext(new SlackCommand { command = "/foo", token = "bar", response_url = "https://www.example.com/" });

            await router.ProcessAsync(IncomingMessageType.Command, context, TestHelper.GetHttpClient(string.Empty));
            var body = context.Response.Body.ReadAsString();

            Assert.Equal(200, context.Response.StatusCode);
            Assert.Equal(string.Empty, body);
        }

        [Fact(DisplayName = "Command_Handles_Delayed_Response_Failure")]
        public async Task Command_Handles_Delayed_Response_Failure()
        {
            var router = new SlackTalkRouter(
                scopeFactory: TestHelper.GetServiceScopeFactory(new List<ISlackTalkProcessor> { new DelayedResponseProcessor() }), 
                options: TestHelper.GetOptions(),
                logger: new Mock<ILogger<SlackTalkRouter>>().Object);

            var context = TestHelper.GetIncomingCommandContext(new SlackCommand { command = "/foo", token = "bar", response_url = "https://www.example.com/" });

            await router.ProcessAsync(IncomingMessageType.Command, context, TestHelper.GetHttpClient(string.Empty, HttpStatusCode.InternalServerError));

            Assert.Equal(200, context.Response.StatusCode);
        }
        
        [Fact(DisplayName = "Action_Handles_Invalid_Token")]
        public async Task Action_Handles_Invalid_Token()
        {
            var router = new SlackTalkRouter(
                scopeFactory: new Mock<IServiceScopeFactory>().Object, 
                options: TestHelper.GetOptions(),
                logger: new Mock<ILogger<SlackTalkRouter>>().Object);

            var context = TestHelper.GetIncomingActionContext(new SlackCallback { callback_id = "foo_callback" });

            await router.ProcessAsync(IncomingMessageType.Action, context, TestHelper.GetHttpClient(string.Empty));
            var error = context.Response.Body.ReadAsString().FromJson<SlackMessage>();

            Assert.Contains("Invalid token", error.text, StringComparison.OrdinalIgnoreCase);
        }
        
        [Fact(DisplayName = "Action_Handles_Missing_Processor")]
        public async Task Action_Handles_Missing_Processor()
        {
            var router = new SlackTalkRouter(
                scopeFactory: TestHelper.GetServiceScopeFactory(new List<ISlackTalkProcessor>(0)), 
                options: TestHelper.GetOptions(),
                logger: new Mock<ILogger<SlackTalkRouter>>().Object);
            
            var context = TestHelper.GetIncomingActionContext(new SlackCallback { callback_id = "bar_callback", token = "bar" });

            await router.ProcessAsync(IncomingMessageType.Action, context, TestHelper.GetHttpClient(string.Empty));
            var error = context.Response.Body.ReadAsString().FromJson<SlackMessage>();

            Assert.Contains("No registered processor found to handle", error.text, StringComparison.OrdinalIgnoreCase);
        }
        
        [Fact(DisplayName = "Action_Handles_Invalid_Action")]
        public async Task Action_Handles_Invalid_Action()
        {
            var router = new SlackTalkRouter(
                scopeFactory: TestHelper.GetServiceScopeFactory(new List<ISlackTalkProcessor> { new DummyProcessor() }), 
                options: TestHelper.GetOptions(),
                logger: new Mock<ILogger<SlackTalkRouter>>().Object);

            var context = TestHelper.GetIncomingActionContext(new SlackCallback { callback_id = "bar_callback", token = "bar" });

            await router.ProcessAsync(IncomingMessageType.Action, context, TestHelper.GetHttpClient(string.Empty));
            var error = context.Response.Body.ReadAsString().FromJson<SlackMessage>();

            Assert.Contains("No registered processor found to handle", error.text, StringComparison.OrdinalIgnoreCase);
        }

        [Fact(DisplayName = "Action_Handles_Failing_Processor")]
        public async Task Action_Handles_Failing_Processor()
        {
            var router = new SlackTalkRouter(
                scopeFactory: TestHelper.GetServiceScopeFactory(new List<ISlackTalkProcessor> { new FailingProcessor() }), 
                options: TestHelper.GetOptions(),
                logger: new Mock<ILogger<SlackTalkRouter>>().Object);

            var context = TestHelper.GetIncomingActionContext(new SlackCallback { callback_id = "foo_callback", token = "bar" });

            await router.ProcessAsync(IncomingMessageType.Action, context, TestHelper.GetHttpClient(string.Empty));
            var error = context.Response.Body.ReadAsString().FromJson<SlackMessage>();

            Assert.Contains("Processor failed", error.text, StringComparison.OrdinalIgnoreCase);
        }
        
        [Fact(DisplayName = "Action_Handles_Immediate_Response")]
        public async Task Action_Handles_Immediate_Response()
        {
            var router = new SlackTalkRouter(
                scopeFactory: TestHelper.GetServiceScopeFactory(new List<ISlackTalkProcessor> { new DummyProcessor() }), 
                options: TestHelper.GetOptions(),
                logger: new Mock<ILogger<SlackTalkRouter>>().Object);

            var context = TestHelper.GetIncomingActionContext(new SlackCallback { callback_id = "foo_callback", token = "bar" });

            await router.ProcessAsync(IncomingMessageType.Action, context, TestHelper.GetHttpClient(string.Empty));
            var error = context.Response.Body.ReadAsString().FromJson<SlackMessage>();

            Assert.Contains("DUMMY", error.text, StringComparison.OrdinalIgnoreCase);
        }
        
        [Fact(DisplayName = "Action_Handles_Null_Message")]
        public async Task Action_Handles_Null_Message()
        {
            var router = new SlackTalkRouter(
                scopeFactory: TestHelper.GetServiceScopeFactory(new List<ISlackTalkProcessor> { new NullMessageProcessor() }), 
                options: TestHelper.GetOptions(),
                logger: new Mock<ILogger<SlackTalkRouter>>().Object);

            var context = TestHelper.GetIncomingActionContext(new SlackCallback { callback_id = "foo_callback", token = "bar" });

            await router.ProcessAsync(IncomingMessageType.Action, context, TestHelper.GetHttpClient(string.Empty));
            var body = context.Response.Body.ReadAsString();

            Assert.Equal(200, context.Response.StatusCode);
            Assert.Equal(string.Empty, body);
        }
        
        [Fact(DisplayName = "Action_Handles_Delayed_Response_Success")]
        public async Task Action_Handles_Delayed_Response_Success()
        {
            var router = new SlackTalkRouter(
                scopeFactory: TestHelper.GetServiceScopeFactory(new List<ISlackTalkProcessor> { new DelayedResponseProcessor() }), 
                options: TestHelper.GetOptions(),
                logger: new Mock<ILogger<SlackTalkRouter>>().Object);

            var context = TestHelper.GetIncomingActionContext(new SlackCallback { callback_id = "foo_callback", token = "bar", response_url = "https://www.example.com/" });

            await router.ProcessAsync(IncomingMessageType.Action, context, TestHelper.GetHttpClient(string.Empty));
            var body = context.Response.Body.ReadAsString();

            Assert.Equal(200, context.Response.StatusCode);
            Assert.Equal(string.Empty, body);
        }

        [Fact(DisplayName = "Action_Handles_Delayed_Response_Failure")]
        public async Task Action_Handles_Delayed_Response_Failure()
        {
            var router = new SlackTalkRouter(
                scopeFactory: TestHelper.GetServiceScopeFactory(new List<ISlackTalkProcessor> { new DelayedResponseProcessor() }), 
                options: TestHelper.GetOptions(),
                logger: new Mock<ILogger<SlackTalkRouter>>().Object);

            var context = TestHelper.GetIncomingActionContext(new SlackCallback { callback_id = "foo_callback", token = "bar", response_url = "https://www.example.com/" });

            await router.ProcessAsync(IncomingMessageType.Action, context, TestHelper.GetHttpClient(string.Empty, HttpStatusCode.InternalServerError));

            Assert.Equal(200, context.Response.StatusCode);
        }
    }
}
