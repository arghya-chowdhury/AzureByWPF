using System;
using System.Threading.Tasks;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace Microsoft.Bot.Sample.SimpleEchoBot
{
    [Serializable]
    public class EchoDialog : IDialog<object>
    {
        protected int count = 1;

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;

            if (message.Text == "reset")
            {
                PromptDialog.Confirm(
                    context,
                    AfterResetAsync,
                    "Are you sure you want to reset the count?",
                    "Didn't get that!",
                    promptStyle: PromptStyle.Auto);
            }
            else if (message.Text.ToLower().Contains("of"))
            {
                var city = message.Text.ToLower().Split(new[] { "of" }, StringSplitOptions.RemoveEmptyEntries)[1];
                Regex rgx = new Regex("[^a-zA-Z -]");
                city = rgx.Replace(city, "");
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync($"https://api.openweathermap.org/data/2.5/weather?q={city}&units=metric&appid=90c7480e6ed5979d397a49ea1a6f8355");
                if (response.IsSuccessStatusCode)
                {
                    var res = await response.Content.ReadAsStringAsync();
                    if (res.Contains("temp"))
                    {
                        var temp = res.Split(new[] { "\"temp\":" }, StringSplitOptions.RemoveEmptyEntries)[1];
                        temp = temp.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)[0];
                        await context.PostAsync($"Current temperature is {temp}");
                        context.Wait(MessageReceivedAsync);
                    }
                }
                else
                {
                    await context.PostAsync($"I've not find anything");
                    context.Wait(MessageReceivedAsync);
                }
            }
            else
            {
                await context.PostAsync($"{this.count++}: You said {message.Text}");
                context.Wait(MessageReceivedAsync);
            }
        }

        public async Task AfterResetAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            var confirm = await argument;
            if (confirm)
            {
                this.count = 1;
                await context.PostAsync("Reset count.");
            }
            else
            {
                await context.PostAsync("Did not reset count.");
            }
            context.Wait(MessageReceivedAsync);
        }

    }
}