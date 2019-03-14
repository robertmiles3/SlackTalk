using System;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace SlackTalk
{
    /// <summary>
    /// Extension methods for the SlackTalk pipelines.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Add SlackTalk pipelines for processing Slack slash commands, interactive message actions, and event notifications
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add to</param>
        public static IApplicationBuilder UseSlackTalk(this IApplicationBuilder app)
        {
            // Create a single HttpClient for reuse across all processing
            var httpClient = new HttpClient();
            
            var settings = app.ApplicationServices.GetRequiredService<IOptions<SlackTalkOptions>>();
            if (settings.Value.CommandRoute == settings.Value.ActionRoute || settings.Value.CommandRoute == settings.Value.EventRoute || settings.Value.ActionRoute == settings.Value.EventRoute)
                throw new Exception($"One or more routes are identical. Please specify distinct routes for {nameof(SlackTalkOptions.CommandRoute)}, {nameof(SlackTalkOptions.ActionRoute)}, and {nameof(SlackTalkOptions.EventRoute)}");
            
            // Setup slash command pipeline
            app.Map(settings.Value.CommandRoute, subApp =>
            {
                var router = app.ApplicationServices.GetRequiredService<ISlackTalkRouter>();
                subApp.Run(context => router.ProcessAsync(IncomingMessageType.Command, context, httpClient));
            });
            
            // Setup interactive message action pipeline
            app.Map(settings.Value.ActionRoute, subApp =>
            {
                var router = app.ApplicationServices.GetRequiredService<ISlackTalkRouter>();
                subApp.Run(context => router.ProcessAsync(IncomingMessageType.Action, context, httpClient));
            });
            
            // Setup event notification pipeline
            app.Map(settings.Value.EventRoute, subApp =>
            {
                var router = app.ApplicationServices.GetRequiredService<ISlackTalkRouter>();
                subApp.Run(context => router.ProcessAsync(IncomingMessageType.Event, context, httpClient));
            });

            return app;
        }
    }
}