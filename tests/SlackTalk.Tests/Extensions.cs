using System.Collections.Generic;
using System.IO;
using Devalp.SlackTalk.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Devalp.SlackTalk.Tests
{
    internal static class Extensions
    {
        internal static string ReadAsString(this Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
        
        internal static FormCollection AsForm(this SlackCommand command) => new FormCollection(new Dictionary<string, StringValues>
        {
            { nameof(SlackCommand.token)           ,command.token },
            { nameof(SlackCommand.team_id)         ,command.team_id },
            { nameof(SlackCommand.team_domain)     ,command.team_domain },
            { nameof(SlackCommand.enterprise_id)   ,command.enterprise_id },
            { nameof(SlackCommand.enterprise_name) ,command.enterprise_name },
            { nameof(SlackCommand.channel_id)      ,command.channel_id },
            { nameof(SlackCommand.channel_name)    ,command.channel_name },
            { nameof(SlackCommand.user_id)         ,command.user_id },
            { nameof(SlackCommand.user_name)       ,command.user_name },
            { nameof(SlackCommand.command)         ,command.command },
            { nameof(SlackCommand.text)            ,command.text },
            { nameof(SlackCommand.response_url)    ,command.response_url },
            { nameof(SlackCommand.trigger_id)      ,command.trigger_id },
        });
    }
}