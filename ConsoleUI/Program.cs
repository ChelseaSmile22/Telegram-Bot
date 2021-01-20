using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using MihaZupan;
using System.Net;

namespace ConsoleUI
{
    class Program
    {
        private static bool Pizdezh;
        private static long person;
        private static TelegramBotClient bot;
        private static List<string> people = new List<string>();
        private static readonly long adminId = 102954480;
        static void Main(string[] args)
        {

            //var proxy = new HttpToSocks5Proxy("nl.windscribe.com", 1080, "rznpc2k9-2tq5u68", "yrpcxvuz35");
            //proxy.ResolveHostnamesLocally = true;
            bot = new TelegramBotClient("");

            bot.OnMessage += Bot_OnMessageAsync;
            bot.OnCallbackQuery += Bot_OnCallbackQuery;
            bot.StartReceiving();
            Console.ReadLine();
            bot.StopReceiving();

        }

        private static async void Bot_OnCallbackQuery(object sender, Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            // chaton 1488228
            if (e.CallbackQuery.Data.Split(' ').First() == "chaton")
            {
                Pizdezh = true;
                person = long.Parse(e.CallbackQuery.Data.Split(' ').Last()); 
            }
            else if (e.CallbackQuery.Data.Split(' ').First() == "chatoff")
            {
                _ = await bot.SendTextMessageAsync(long.Parse(e.CallbackQuery.Data.Split(' ').Last()), "Выход");
            }
        }

        private static async void Bot_OnMessageAsync(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            if (e.Message.Type == MessageType.Text)
            {
                if(e.Message.Text.StartsWith("напомни через"))
                {
                    string[] message = e.Message.Text.Split(' ');
                    string word3 = message[2];
                    string count = word3.Substring(0, word3.Length - 1);
                    int iCount = int.Parse(count);

                    string secminhour = word3.Substring(word3.Length - 1, 1);
                    switch (secminhour)
                    {
                        case "с":
                            iCount = (int)TimeSpan.FromSeconds(iCount).TotalMilliseconds;
                            break;
                        case "м":
                            iCount = (int)TimeSpan.FromMinutes(iCount).TotalMilliseconds;
                            break;
                        case "ч":
                            iCount = (int)TimeSpan.FromHours(iCount).TotalMilliseconds;
                            break;
                    }

                    string msg = "";
                    for (int i = 3; i < message.Length; i++)
                    {
                        msg = msg + $" {message[i]}";
                    }

                    Reminder rem = new Reminder(iCount, msg, bot, e.Message.Chat.Id);
                    rem.Run();
                    await bot.SendTextMessageAsync(e.Message.Chat.Id, "Замётано");
                }
            }
            if(e.Message.Type == MessageType.Text)
            {
                if(e.Message.Text.StartsWith("/shame"))
                {
                    var msg = await bot.SendTextMessageAsync(e.Message.Chat.Id, "*ругательства*");
                }
            }
            if (e.Message.Text == "/chat")
            {
                InlineKeyboardMarkup inlineKeyboardMarkup = new InlineKeyboardMarkup(
                   new[]
                   {
                        new InlineKeyboardButton() {Text = "Принять чат от", CallbackData = $"chaton {e.Message.Chat.Id}"},
                        new InlineKeyboardButton() {Text = "Отклонить", CallbackData = $"chatoff {e.Message.Chat.Id}"}  
                   });
                var msg = await bot.SendTextMessageAsync(e.Message.Chat.Id, "Передаю запрос");
                _ = await bot.SendTextMessageAsync(adminId, $"{e.Message.Chat.Id} хочет пообщаться, принимаем запрос?", replyMarkup: inlineKeyboardMarkup);




            }
        }
    }
  
}
  

    