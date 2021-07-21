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
        private static string Token { get; set; } = "1872032989:AAEdfnOLIQiQj1OJCheWGDBmPSuAhsBH7gw";
        private static TelegramBotClient client;

        static void Main(string[] args)
        {
            client = new TelegramBotClient(Token);

            client.StartReceiving();

            client.OnMessage += StartMessege;

            Console.ReadLine();
            client.StopReceiving();
        }

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

                        var inlineKeyboard_1 = new InlineKeyboardMarkup(new[]
                        {
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Расписание по числителю!")
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Расписание по знаменателю!")
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Расписание звонков!")
                        }
                    });
                        await client.SendTextMessageAsync(msg.From.Id, "Какое расписание вы хотите?", replyMarkup: inlineKeyboard_1);

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
                        await client.SendContactAsync(msg.Chat.Id, "+380675115257", "Настя", "Никулина");
                        break;

                }

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
