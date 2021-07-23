using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

using System.IO;

namespace Tg_Bot
{
    class Program
    {
        private static string Token { get; set; }
        private static TelegramBotClient client;

        static void Main(string[] args)
        {
            using (FileStream fstream = new FileStream("token.txt", FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(fstream))
                    Token = reader.ReadLine();
            }
                client = new TelegramBotClient(Token);

            Console.WriteLine(client.GetMeAsync().Result);
            try
            {
                client.StartReceiving();

                client.OnMessage += StartMessege;

                client.OnCallbackQuery += CallBackInlineQuaryForNumerator;
                //client.OnInlineQuery += StartInlineMessage;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                client.StopReceiving();
            }
            Console.ReadLine();
            client.StopReceiving();
        }

        private static async void CallBackInlineQuaryForNumerator(object sender, CallbackQueryEventArgs callBack)
        {
            await client.AnswerCallbackQueryAsync(callBack.CallbackQuery.Id, $"ля, кого я вижу, да, {callBack.CallbackQuery.From.FirstName} 🤨");
            Console.WriteLine(callBack.CallbackQuery.From.FirstName + callBack.CallbackQuery.Data + '\t' + callBack.CallbackQuery.InlineMessageId);
            string[] date = callBack.CallbackQuery.Data.Split('|');

            byte type = 0, id = 1;

            

            var inlineKeyboard_DayOfWeek = new InlineKeyboardMarkup(new[]
            {

                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Пн", $"Monday|{date[id]}"),
                    InlineKeyboardButton.WithCallbackData("Вт", $"Tuesday|{date[id]}")
                },
                //new[]
                //{
                    
                //},
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Ср", $"Wednesday|{date[id]}"),
                    InlineKeyboardButton.WithCallbackData("Чт", $"Thursday|{date[id]}")
                },
                  new[]
                {
                    InlineKeyboardButton.WithCallbackData("Пт", $"Friday|{date[id]}")
                },
                  new[]
                {
                    InlineKeyboardButton.WithCallbackData("Назад🔙", $"Back|{date[id]}")
                }
            });
            await client.SendTextMessageAsync(date[id], "Выбери день недели:", replyMarkup: inlineKeyboard_DayOfWeek);

            client.OnCallbackQuery += CallBackInlineQuaryForDayOfWeekOfNumeratot;
            client.OnCallbackQuery -= CallBackInlineQuaryForDayOfWeekOfNumeratot;
        }

        private static async void CallBackInlineQuaryForDayOfWeekOfNumeratot(object sender, CallbackQueryEventArgs callback)
        {
            string[] date = callback.CallbackQuery.Data.Split('|');
            byte day = 0, id = 1;
            await client.SendTextMessageAsync(date[id], $"Ты выбрал {date[day]}:\n");
            switch (date[day])
            {
                case "Monday":
                    await client.SendTextMessageAsync(date[id], "No info");
                    break;
                case "Tuesday":
                    await client.SendTextMessageAsync(date[id], "No info");
                    break;
                case "Wednesday":
                    await client.SendTextMessageAsync(date[id], "No info");
                    break;
                case "Thursday":
                    await client.SendTextMessageAsync(date[id], "No info");
                    break;
                case "Friday":
                    await client.SendTextMessageAsync(date[id], "No info");
                    break;
                case "Back":
                    //client.OnCallbackQuery -= CallBackInlineQuaryForDayOfWeekOfNumeratot;
                    await client.SendTextMessageAsync(date[id], "Какое расписание вы хотите?", replyMarkup: inlineKeyboard_TimeTable);
                    break;


            }
        }

        //private static async void StartInlineMessage(object sender, InlineQueryEventArgs e)
        //{
        //    var inlineMsg = e.InlineQuery;
        //    if(inlineMsg != null)
        //    {
        //        if (inlineMsg.ChatType.Value != null)
        //        {
        //            await client.SendTextMessageAsync("Hi!");
        //        }
        //    }
        //}
        static InlineKeyboardMarkup inlineKeyboard_TimeTable;
        private static async void StartMessege(object sender, MessageEventArgs e)
        {
            var msg = e.Message;
            if (msg != null)
            {
                if (msg.Text == "/start")
                    await client.SendTextMessageAsync(msg.Chat.Id,
                            "Добро пожаловать в данный чат-бот!🙃\n" +
                            "Здесь есть почти вся необходимая информация что бы учиться на 2м курсе😌\n" +
                            "Удачи в обучении!✨", replyMarkup: GetButtons());

                switch (msg.Text)
                {

                    /*
                     
                     Расписание:
                          - Числитель:
                                * Выбор дня недели:
                                     - Пн - Вт - Ср - Чт - Пт

                          - Знаменатель
                                * Выбор дня недели:
                                     - Пн - Вт - Ср - Чт - Пт

                          - Звонки      ->      Текст расписания звонков
                     
                     */

                    case "Расписание!":

                        inlineKeyboard_TimeTable = new InlineKeyboardMarkup(new[]
                        {
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Расписание по числителю!", callbackData: $"числитель|{msg.From.Id}")
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Расписание по знаменателю!", callbackData: $"знаменатель|{msg.From.Id}")
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Расписание звонков!", callbackData: $"Звонки|{msg.From.Id}")
                        }
                        
                    });
                        Console.WriteLine($"[{e.Message.From.FirstName}] - [{e.Message.From.Id}] - [{e.Message.From.Username}] | ");
                        await client.SendTextMessageAsync(msg.From.Id, "Какое расписание вы хотите?", replyMarkup: inlineKeyboard_TimeTable);

                        break;

                    /*
                     х6
                     Предметы:
                          - Предмет:
                                *Преподаватели -> Имена - Связь - 

                     */


                    case "Предметы!":
                        var inlineKeyboard_2 = new InlineKeyboardMarkup(new[]
                        {
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("1!"),
                                InlineKeyboardButton.WithCallbackData("2!")
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("3!"),
                                InlineKeyboardButton.WithCallbackData("4!")
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("5!"),
                                InlineKeyboardButton.WithCallbackData("6!")
                            }
                        });
                        await client.SendTextMessageAsync(msg.From.Id, "Выберите предмет:", replyMarkup: inlineKeyboard_2);
                        break;

                    /*

                    Ссылка на чат - "Вопрос-ответ" 

                     */

                    case "Вопрос-Ответ!":
                        var inlineKeyboard_3 = new InlineKeyboardMarkup(new[]
                        {
                            new[]
                            {
                                InlineKeyboardButton.WithUrl("Чат для вопросов!", "https://t.me/joinchat/V69YheCJ-Fb9q8mJ")
                            }
                        });

                        await client.SendTextMessageAsync(msg.From.Id, "Держи!", replyMarkup: inlineKeyboard_3);
                        break;


                    /*
                    х6
                     Предметы:
                          - Предмет:
                                *вид урока и ссылка в кнопке

                     */

                    case "Конференции!":



                        break;


                    /*

                    Мой контакт 

                     */

                    case "Связь!":
                        
                        string username;
                        using (FileStream fstream = new FileStream("username.txt", FileMode.Open))
                        {
                            using (StreamReader reader = new StreamReader(fstream))
                                username = reader.ReadLine();
                                
                        }

                        await client.SendTextMessageAsync(msg.Chat.Id, username);
                        //await client.SendContactAsync(msg.Chat.Id, );
                        break;
                    
                        //if(e.Message.Type == Telegram.Bot.Types.Enums.MessageType.)

                        
                }
                //CurrentChatId = e.Message.Chat.Id;
            }
        }

        private static IReplyMarkup GetButtons()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton> { new KeyboardButton { Text = "Расписание!" } },
                    new List<KeyboardButton> { new KeyboardButton { Text = "Предметы!" }, new KeyboardButton { Text = "Вопрос-Ответ!" }, new KeyboardButton { Text = "Конференции!" } },
                    new List<KeyboardButton> { new KeyboardButton { Text = "Связь!" } }
                }
            };
        }

    }

}

/*
 
                            КНОПКИ РАСПИСАНИЕ И ПРЕДМЕТЫ

 if(msg.Text == "Расписание!")
                {
                    
                    if (msg.Text == "Расписание по числителю!")
                    {
                        await client.SendTextMessageAsync(msg.Chat.Id, "Вот расписание по числителю:");
                    }
                    if (msg.Text == "Расписание по знаменателю!")
                    {
                        await client.SendTextMessageAsync(msg.Chat.Id, "Вот расписание по знаменателю:");
                    }
                    if (msg.Text == "Расписание звонков!")
                    {
                        await client.SendTextMessageAsync(msg.Chat.Id, "Вот расписание звонков:");
                    }

                }
 
 */



/*
 
 switch (msg.Text)
                {
                    case "Расписание по числителю!":

                        await client.AnswerCallbackQueryAsync("Расписание по числителю:");
                        break;

                    case "Расписание по знаменателю!":

                        await client.SendTextMessageAsync(msg.Chat.Id, "Расписание по знаменателю:");
                        break;

                    case "Расписание звонков!":

                        await client.SendTextMessageAsync(msg.Chat.Id, "Расписание звонков:");
                        break;
                }

                switch (msg.Text)
                {
                    case "1!":

                        await client.SendTextMessageAsync(msg.Chat.Id, "1:");
                        break;

                    case "2!":

                        await client.SendTextMessageAsync(msg.Chat.Id, "2:");
                        break;

                    case "3!":

                        await client.SendTextMessageAsync(msg.Chat.Id, "3:");
                        break;

                    case "4!":

                        await client.SendTextMessageAsync(msg.Chat.Id, "4:");
                        break;

                    case "5!":

                        await client.SendTextMessageAsync(msg.Chat.Id, "5:");
                        break;

                    case "6!":

                        await client.SendTextMessageAsync(msg.Chat.Id, "6:");
                        break;
                }
 
 */
