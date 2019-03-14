using System.Net;
using SlackTalk.Models;
using Microsoft.AspNetCore.Http;

namespace SlackTalk
{
    internal static class FormCollectionExtensions
    {
        internal static SlackCommand AsSlackCommand(this IFormCollection form)
        {
            if (form == null)
                return null;

            var command = new SlackCommand
            {
                token = form[nameof(SlackCommand.token)].ToString(),
                team_id = form[nameof(SlackCommand.team_id)].ToString(),
                team_domain = form[nameof(SlackCommand.team_domain)].ToString(),
                enterprise_id = form[nameof(SlackCommand.enterprise_id)].ToString(),
                enterprise_name = form[nameof(SlackCommand.enterprise_name)].ToString(),
                channel_id = form[nameof(SlackCommand.channel_id)].ToString(),
                channel_name = form[nameof(SlackCommand.channel_name)].ToString(),
                user_id = form[nameof(SlackCommand.user_id)].ToString(),
                user_name = form[nameof(SlackCommand.user_name)].ToString(),
                command = form[nameof(SlackCommand.command)].ToString(),
                text = form[nameof(SlackCommand.text)].ToString(),
                response_url = form[nameof(SlackCommand.response_url)].ToString(),
                trigger_id = form[nameof(SlackCommand.trigger_id)].ToString(),
            };

            return command;
        }
        
        internal static SlackCallback AsSlackCallback(this IFormCollection form)
        {
            if (form == null || !form.ContainsKey("payload"))
                return null;

            return WebUtility.UrlDecode(form["payload"].ToString()).FromJson<SlackCallback>();
        }
    }
}