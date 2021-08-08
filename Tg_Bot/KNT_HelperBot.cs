using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using Tg_Bot.ServiceClass;
using Tg_Bot.Interfaces;
using Tg_Bot.Enumerate;

namespace Tg_Bot
{
    class KNT_HelperBot : IManegeKNTBot
    {
        private string Token { get; set; }
        private TelegramBotClient client;

        public delegate void PauseForWork();
        public event PauseForWork PauseForWorking;

        Server.Server server;
        Thread serv;
        public KNT_HelperBot()
        {
            using (FileStream fstream = new FileStream(FileName.Token, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(fstream))
                    Token = reader.ReadLine();
            }
            client = new TelegramBotClient(Token);

            server = new Server.Server();

        }

        [Obsolete]
        public void StartReciving()
        {
            Console.WriteLine(client.GetMeAsync().Result);
            try
            {
                client.StartReceiving();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                client.StopReceiving();
            }
            serv = new Thread(new ThreadStart(server.TurnOn));
            serv.Start();
        }

        [Obsolete]
        public void TurnOn_OfEvent()
        {
            try
            {
                client.OnMessage += StartMessege;
                client.OnCallbackQuery += CallBackInlineQuaryMain;
                //client.OnCallbackQuery += CallBackInlineQuaryForDayOfWeek;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                client.StopReceiving();
            }
        }
        public void StopReciving()
        {
            if (PauseForWorking == null)
            {
                client.StopReceiving();

                throw new KNTHelperBotException("------>\nThe object has no pause event\n<------\n", "add to Event of =>PauseForWorking<= function for pause:\n" +
                    "for Example:\n" +
                    "public void PauseIvent()\n" +
                    "{\n" +
                    "Console.ReadKey();\n" +
                    "}\n");
            }
            PauseForWorking?.Invoke();
            client.StopReceiving();

        }

        private async void StartMessege(object sender, MessageEventArgs e)
        {
            var msg = e.Message;
            if (msg != null)
            {
                bool userInBlackList = false;

                try
                {
                    userInBlackList = TelegramClientCheck.InBlackList(msg.From.Id.ToString());
                }
                catch (KNTHelperBotException ex)
                {
                    Console.WriteLine(ex.Message + "\n=======\n" + ex.GetWhatToDo());
                }

                if (!userInBlackList)
                {
                    if (msg.Text == "/start")
                    {

                        if (!TelegramClientCheck.IsFamiliar(msg.From.Id.ToString()))
                        {
                            await client.SendTextMessageAsync(msg.Chat.Id, $"Слушай, {msg.From.FirstName}🤨 ты не отсюдого, тебе низя 😋");
                            await client.SendTextMessageAsync(msg.Chat.Id, "😏");

                            Console.WriteLine($"[{e.Message.From.FirstName}] - [{e.Message.From.Id}] - [{e.Message.From.Username}] | BAN!");
                            goto EndOfListenOfMsg;
                        }

                        using (FileStream fstream = new FileStream(FileName.Welcome_text, FileMode.Open))
                        using (StreamReader reader = new StreamReader(fstream))
                            await client.SendTextMessageAsync(msg.Chat.Id, reader.ReadToEnd(), replyMarkup: new ButtonGenerator().GetKeyBoardButtons());
                    }

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

                        case "📋Расписание!📋":

                            //              ||
                            //TODO Replace-\||/ (Logger)

                            Console.WriteLine($"[{e.Message.From.FirstName}] - [{e.Message.From.Id}] - [{e.Message.From.Username}] - [{e.Message.Chat.Id}] | ");

                            await client.SendTextMessageAsync(
                                chatId: msg.From.Id,
                                text: "Какое расписание вы хотите?",
                                replyMarkup: new ButtonGenerator().GetInlineButtons_TimeTable()
                                );

                            break;

                        /*
                         х6
                         Предметы:
                              - Предмет:
                                    *Преподаватели -> Имена - Связь - 

                         */


                        case "📚Предметы!📚":

                            //TODO Logger

                            await client.SendTextMessageAsync(
                                chatId: msg.From.Id,
                                text: "Выберите предмет:",
                                replyMarkup: new ButtonGenerator().GetInlineButtons_Lessons());
                            break;

                        /*

                        Ссылка на чат - "Вопрос-ответ" 

                         */

                        case "⁉️Вопрос-Ответ!⁉️":

                            //TODO Logger

                            await client.SendTextMessageAsync(
                                chatId: msg.From.Id,
                                text: "Держи!",
                                replyMarkup: (InlineKeyboardMarkup)new ButtonGenerator().GetInlineButton_QuestionAnswe());
                            break;


                        /*
                        х6
                         Предметы:
                              - Предмет:
                                    *вид урока и ссылка в кнопке

                         */

                        case "💻Конференции!💻":

                            //TODO Logger

                            await client.SendTextMessageAsync(
                                chatId: msg.From.Id,
                                text: "Выберите предмет:",
                                replyMarkup: new ButtonGenerator().GetinlineKeyboard_Conf());

                            break;


                        /*

                        Мой контакт 

                         */

                        case "📲Связь!📲":

                            using (FileStream fstream = new FileStream(FileName.ComunicationAnswer, FileMode.Open))
                            using (StreamReader reader = new StreamReader(fstream))
                                await client.SendTextMessageAsync(msg.Chat.Id, reader.ReadToEnd());

                            break;
                        case "💰На Сервер!💰":
                            await client.SendTextMessageAsync(msg.Chat.Id, FileName.DonateLink);
                            await client.SendTextMessageAsync(msg.Chat.Id, "Или воспользуйтесь Qr-кодом для совершения доната, заранее спасибки🤗😌");
                            await client.SendPhotoAsync (msg.Chat.Id, FileName.DonateQrCode);
                            break;
                        default:

                            //              ||
                            //TODO Replace-\||/
                            Console.WriteLine($"[{e.Message.From.FirstName}] - [{e.Message.From.Id}] - [{e.Message.From.Username}] - [{e.Message.Chat.Id}] |\nTextMsg:\n" +
                    $"{msg.Text} \n--------------\n");
                            break;
                    }
                }
                else
                    Console.WriteLine($"[{e.Message.From.FirstName}] - [{e.Message.From.Id}] - [{e.Message.From.Username}] | BAN!");

                EndOfListenOfMsg:;
            }
        }
        [Obsolete]
        private void CallBackInlineQuaryMain(object sender, CallbackQueryEventArgs callBack)
        {
            InlineData inlineData = new InlineData(callBack.CallbackQuery.Data);

            //TODO Logger
            Console.WriteLine($"[{callBack.CallbackQuery.From.FirstName}] - [{callBack.CallbackQuery.From.Id}] - [{callBack.CallbackQuery.From.Username}] "
                + $"\t{callBack.CallbackQuery.InlineMessageId}\t|{inlineData.TypeOfButton}");
            
            switch (inlineData.TypeOfButton)
            {
                case TypeOfButton.None:
                    throw new KNTHelperBotException("Unredefined call", "It is not clear who called the function");
                    break;
                case TypeOfButton.TimeTable:
                    TimeTable(inlineData, callBack);
                    break;
                case TypeOfButton.DayOfWeek:
                    DayOfWeek(inlineData, callBack);
                    break;
                case TypeOfButton.Lessons:
                    break;
                case TypeOfButton.Conferences:
                    break;
                default:
                    break;
            }
        }
        private async void TimeTable(InlineData data, CallbackQueryEventArgs callBack)
        {
            await client.AnswerCallbackQueryAsync(callBack.CallbackQuery.Id, $"ля, кого я вижу, да, {callBack.CallbackQuery.From.FirstName} 🤨");

            switch (data.TypeOfWeek)
            {
                case Enumerate.TypeOfWeek.None:
                    throw new KNTHelperBotException("Unredefined call", $"It is not clear who called the function *Timetable*");
                    
                case Enumerate.TypeOfWeek.Numerator:
                    await client.SendTextMessageAsync(
                chatId: callBack.CallbackQuery.From.Id,
                text: "Выбери день недели:",
                replyMarkup: new ButtonGenerator().GetinlineKeyboard_DayOfWeek(data));
                    break;

                case Enumerate.TypeOfWeek.Denominator:
                    await client.SendTextMessageAsync(
                chatId: callBack.CallbackQuery.From.Id,
                text: "Выбери день недели:",
                replyMarkup: new ButtonGenerator().GetinlineKeyboard_DayOfWeek(data));
                    break;

                case Enumerate.TypeOfWeek.Call_:
                    await client.AnswerCallbackQueryAsync(callBack.CallbackQuery.Id);
                    await client.SendPhotoAsync(callBack.CallbackQuery.From.Id, FileName.TimeTable);
                    break;
                default:
                    break;
            }
        }
        private async void DayOfWeek(InlineData data, CallbackQueryEventArgs callBack)
        {
            await client.AnswerCallbackQueryAsync(callBack.CallbackQuery.Id);
            await client.SendTextMessageAsync(callBack.CallbackQuery.From.Id, $"Ты выбрал {data.TypeOfDay}, тип недели {data.TypeOfWeek}:\n");

            //TODO
            //reading by file
            await client.SendTextMessageAsync(callBack.CallbackQuery.From.Id, "Пока информации нету😅");
        }
    }
}
