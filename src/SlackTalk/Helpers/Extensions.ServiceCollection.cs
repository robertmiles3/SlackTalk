using System;
using Microsoft.Extensions.DependencyInjection;

namespace Devalp.SlackTalk
{
    /// <summary>
    /// Extension methods
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add SlackTalk routing to the services collection
        /// </summary>
        /// <param name="services">The services collection to configure.</param>
        /// <param name="configureOptions">An Action to configure options for the SlackTalk router.</param>
        public static IServiceCollection AddSlackTalk(this IServiceCollection services, Action<SlackTalkOptions> configureOptions = null)
        {
            if (configureOptions != null)
            {
                services.Configure(configureOptions);
            }

            // Add singleton router
            services.AddSingleton<ISlackTalkRouter, SlackTalkRouter>();
            
            return services;
        }
    }
}