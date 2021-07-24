using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace Tg_Bot
{
    class KNT_HelperBot
    {
        private string Token { get; set; }
        private TelegramBotClient client;
        private InlineKeyboardMarkup inlineKeyboard_TimeTable;
        byte type = 0, day = 1, id = 2;

        //public delegate T CompletionSign<T> ();
        //public event CompletionSign<ConsoleKey> completeSign;
        public KNT_HelperBot()
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

                client.OnCallbackQuery += CallBackInlineQuaryMain;
                client.OnCallbackQuery += CallBackInlineQuaryForDayOfWeek;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                client.StopReceiving();
            }
            Console.ReadKey();
            //completeSign?.Invoke();

            client.StopReceiving();
        }

        [Obsolete]
        private void CallBackInlineQuaryMain(object sender, CallbackQueryEventArgs callBack)
        {
           
            Console.WriteLine($"[{callBack.CallbackQuery.From.FirstName}] - [{callBack.CallbackQuery.From.Id}] - [{callBack.CallbackQuery.From.Username}] | " 
                + callBack.CallbackQuery.Data + '\t' + callBack.CallbackQuery.InlineMessageId);

            string stype = callBack.CallbackQuery.Data.Split('|')[type];
            
            if ( stype != "Call" && callBack.CallbackQuery.Data.Split('|')[day] == "")
                TypeOfWeek(callBack);
            else if(stype == "Call")
                GetCallBordImage(callBack);

        }
        private async void GetCallBordImage(CallbackQueryEventArgs callBack)
        {
            await client.SendPhotoAsync(callBack.CallbackQuery.Data.Split('|')[id], "CallBord.jpg");
        }
        private async void TypeOfWeek(CallbackQueryEventArgs callBack)
        {
            await client.AnswerCallbackQueryAsync(callBack.CallbackQuery.Id, $"ля, кого я вижу, да, {callBack.CallbackQuery.From.FirstName} 🤨");

            string[] date = callBack.CallbackQuery.Data.Split('|');

            byte type = 0, id = 2;

            var inlineKeyboard_DayOfWeek = GetinlineKeyboard_DayOfWeek(date);

            await client.SendTextMessageAsync(date[id], "Выбери день недели:", replyMarkup: inlineKeyboard_DayOfWeek);
        }
        InlineKeyboardMarkup GetinlineKeyboard_DayOfWeek(string[] date)
        {
            byte type = 0, id = 2;
            return new InlineKeyboardMarkup(new[]
            {
            new[]
                {
                    InlineKeyboardButton.WithCallbackData("Пн", $"{date[type]}|Monday|{date[id]}"), //for read from (for example NumeratorMonday.txt)
                    InlineKeyboardButton.WithCallbackData("Вт", $"{date[type]}|Tuesday|{date[id]}")
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Ср", $"{date[type]}|Wednesday|{date[id]}"),
                    InlineKeyboardButton.WithCallbackData("Чт", $"{date[type]}|Thursday|{date[id]}")
                },
                  new[]
                {
                    InlineKeyboardButton.WithCallbackData("Пт", $"{date[type]}|Friday|{date[id]}")
                }
            });
        }
        private async void CallBackInlineQuaryForDayOfWeek(object sender, CallbackQueryEventArgs callBack)
        {

            string[] date = callBack.CallbackQuery.Data.Split('|');
            byte type = 0, day = 1, id = 2;

            if (date[day] == "")
                return;
            await client.AnswerCallbackQueryAsync(callBack.CallbackQuery.Id);


            
            await client.SendTextMessageAsync(date[id], $"Ты выбрал {date[day]}, тип недели {date[type]}:\n");
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
            }
        }
        private async void StartMessege(object sender, MessageEventArgs e)
        {
            var msg = e.Message;
            if (msg != null)
            {
                if (msg.Text == "/start")
                    await client.SendTextMessageAsync(msg.Chat.Id,
                            "Добро пожаловать в данный чат-бот!🙃\n" +
                            "Здесь есть почти вся необходимая информация что бы учиться на 2м курсе😌\n" +
                            "Удачи в обучении!✨", replyMarkup: GetKeyBoardButtons());

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
                            InlineKeyboardButton.WithCallbackData("Расписание по числителю!", callbackData: $"Numerator||{msg.From.Id}")
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Расписание по знаменателю!", callbackData: $"Denominator||{msg.From.Id}")
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Расписание звонков!", callbackData: $"Call||{msg.From.Id}")
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
                        break;
                }
            }
        }

        private IReplyMarkup GetKeyBoardButtons()
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
