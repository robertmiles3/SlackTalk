using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using SlackTalk.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Moq;
using Moq.Protected;

namespace SlackTalk.Tests
{
    internal static class TestHelper
    {
        internal static IOptions<SlackTalkOptions> GetOptions() => Options.Create(new SlackTalkOptions { VerificationToken = "bar" });
        
        internal static HttpContext GetIncomingCommandContext(SlackCommand command = null)
        {
            var context = new DefaultHttpContext();
            context.Request.Method = "POST";
            context.Request.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            
            // https://stackoverflow.com/questions/45959605/inspect-defaulthttpcontext-body-in-unit-test-situation
            // Must set the Response.Body to a new MemoryStream because DefaultHttpContext's default is Stream.Null
            // that ignores all reads and writes.
            context.Response.Body = new MemoryStream();

            if (command != null)
            {
                context.Request.Form = command.AsForm();
            }

            return context;
        }
        
        internal static HttpContext GetIncomingActionContext(SlackCallback callback = null)
        {
            var context = new DefaultHttpContext();
            context.Request.Method = "POST";
            context.Request.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            
            // https://stackoverflow.com/questions/45959605/inspect-defaulthttpcontext-body-in-unit-test-situation
            // Must set the Response.Body to a new MemoryStream because DefaultHttpContext's default is Stream.Null
            // that ignores all reads and writes.
            context.Response.Body = new MemoryStream();

            if (callback != null)
            {
                var dict = new Dictionary<string, StringValues> { { "payload", WebUtility.UrlEncode(callback.ToJson()) } };
                context.Request.Form = new FormCollection(dict);
            }

            return context;
        }
        
        internal static HttpClient GetHttpClient(string returnsJson, HttpStatusCode returnsStatusCode = HttpStatusCode.OK)
        {
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage {
                    StatusCode = returnsStatusCode,
                    Content = new StringContent(returnsJson)
                }));

            return new HttpClient(mockMessageHandler.Object);
        }

        internal static IServiceScopeFactory GetServiceScopeFactory(List<ISlackTalkProcessor> serviceProviderReturn)
        {
            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider.Setup(x => x.GetService(typeof(IEnumerable<ISlackTalkProcessor>))).Returns(serviceProviderReturn);
            
            var scope = new Mock<IServiceScope>();
            scope.Setup(x => x.ServiceProvider).Returns(serviceProvider.Object);
            
            var scopeFactory = new Mock<IServiceScopeFactory>();
            scopeFactory.Setup(x => x.CreateScope()).Returns(scope.Object);
            
            return scopeFactory.Object;
        }
    }
}