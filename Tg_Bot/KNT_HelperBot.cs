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
                    if (!TelegramClientCheck.IsAdmins(msg.From.Id.ToString()))
                    {

                        DateTime release = new DateTime(2021, 08, 31, 05, 30, 00);
                        release = release.ToUniversalTime();

                        if (DateTime.Now.ToUniversalTime() < release)
                        {
                            TimeSpan date = release.Subtract(DateTime.Now.ToUniversalTime());

                            TelegramBotLogger.PrintInfo(e.Message.From.FirstName, e.Message.From.Id.ToString(), e.Message.From.Username, msg.Text);

                            await client.SendTextMessageAsync(msg.Chat.Id, $"До релиза бота осталось: {date.Days} дней {date.Hours} ч. {date.Minutes} м.");
                            return;
                        }
                    }

                    if (msg.Text == "/start")
                    {
                        if (!TelegramClientCheck.IsFamiliar(msg.From.Id.ToString()))
                        {
                            await client.SendTextMessageAsync(msg.Chat.Id, $"Слушай, {msg.From.FirstName}🤨 ты не отсюдого, тебе низя 😋");
                            await client.SendTextMessageAsync(msg.Chat.Id, "😏");

                            TelegramBotLogger.PrintBanInfo(e.Message.From.FirstName, e.Message.From.Id.ToString(), e.Message.From.Username, msg.Text);
                            goto EndOfListenOfMsg;
                        }

                        using (FileStream fstream = new FileStream(FileName.Welcome_text, FileMode.Open))
                        using (StreamReader reader = new StreamReader(fstream))
                            await client.SendTextMessageAsync(msg.Chat.Id, reader.ReadToEnd(), replyMarkup: new ButtonGenerator().GetKeyBoardButtons());

                        //await client .SendTextMessageAsync(msg.Chat.Id, "test");
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

                            //Console.WriteLine($"[{e.Message.From.FirstName}] - [{e.Message.From.Id}] - [{e.Message.From.Username}] - [{e.Message.Chat.Id}] | ");
                            TelegramBotLogger.PrintInfo(e.Message.From.FirstName, e.Message.From.Id.ToString(), e.Message.From.Username, TypeOfButton.TimeTable.ToString());

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
                            TelegramBotLogger.PrintInfo(e.Message.From.FirstName, e.Message.From.Id.ToString(), e.Message.From.Username, TypeOfButton.Lessons.ToString());

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
                            TelegramBotLogger.PrintInfo(e.Message.From.FirstName, e.Message.From.Id.ToString(), e.Message.From.Username, TypeOfButton.QuesAnsw.ToString());

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
                            TelegramBotLogger.PrintInfo(e.Message.From.FirstName, e.Message.From.Id.ToString(), e.Message.From.Username, TypeOfButton.Conferences.ToString());

                            await client.SendTextMessageAsync(
                                chatId: msg.From.Id,
                                text: "Выберите предмет:",
                                replyMarkup: new ButtonGenerator().GetinlineKeyboard_Conf());

                            break;


                        /*

                        Мой контакт 

                         */

                        case "📲Связь!📲":

                            //TODO Logger
                            TelegramBotLogger.PrintInfo(e.Message.From.FirstName, e.Message.From.Id.ToString(), e.Message.From.Username, TypeOfButton.Сommunication.ToString());

                            using (FileStream fstream = new FileStream(FileName.ComunicationAnswer, FileMode.Open))
                            using (StreamReader reader = new StreamReader(fstream))
                                await client.SendTextMessageAsync(msg.Chat.Id, reader.ReadToEnd());

                            break;


                        case "💰На Сервер!💰":

                            //TODO Logger
                            TelegramBotLogger.PrintInfo(e.Message.From.FirstName, e.Message.From.Id.ToString(), e.Message.From.Username, TypeOfButton.ToServer.ToString());

                            await client.SendTextMessageAsync(msg.Chat.Id, FileName.DonateLink);
                            await client.SendTextMessageAsync(msg.Chat.Id, "Или воспользуйтесь Qr-кодом для совершения доната, заранее спасибки🤗😌");
                            await client.SendPhotoAsync(msg.Chat.Id, FileName.DonateQrCode);
                            break;
                        default:

                            //              ||
                            //TODO Replace-\||/

                            TelegramBotLogger.PrintInfo(e.Message.From.FirstName, e.Message.From.Id.ToString(), e.Message.From.Username, msg.Text);

                            break;
                    }
                }
                else
                    //Console.WriteLine($"[{e.Message.From.FirstName}] - [{e.Message.From.Id}] - [{e.Message.From.Username}] | BAN!");
                    TelegramBotLogger.PrintBanInfo(e.Message.From.FirstName, e.Message.From.Id.ToString(), e.Message.From.Username, msg.Text);

                EndOfListenOfMsg:;
            }
        }
        [Obsolete]
        private void CallBackInlineQuaryMain(object sender, CallbackQueryEventArgs callBack)
        {
            InlineData inlineData = new InlineData(callBack.CallbackQuery.Data);

            switch (inlineData.TypeOfButton)
            {
                case TypeOfButton.None:
                    throw new KNTHelperBotException("Unredefined call", "It is not clear who called the function");

                case TypeOfButton.TimeTable:
                    TimeTable(inlineData, callBack);
                    break;
                case TypeOfButton.DayOfWeek:
                    DayOfWeek(inlineData, callBack);
                    break;
                case TypeOfButton.Lessons:
                    LessonsInfo(inlineData, callBack);
                    break;
                case TypeOfButton.Conferences:
                    break;
                default:
                    break;
            }
        }
        private async void TimeTable(InlineData data, CallbackQueryEventArgs callBack)
        {
            await client.AnswerCallbackQueryAsync(callBack.CallbackQuery.Id, $"ля, кого я вижу, {callBack.CallbackQuery.From.FirstName} 🤨");

            TelegramBotLogger.PrintInfo(callBack.CallbackQuery.From.FirstName, callBack.CallbackQuery.From.Id.ToString(),
               callBack.CallbackQuery.From.Username, data.TypeOfWeek.ToString());

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
            //await client.SendTextMessageAsync(callBack.CallbackQuery.From.Id, $"Ты выбрал {data.TypeOfDay}, тип недели {data.TypeOfWeek}:\n");

            TelegramBotLogger.PrintInfo(callBack.CallbackQuery.From.FirstName, callBack.CallbackQuery.From.Id.ToString(),
               callBack.CallbackQuery.From.Username, $"{data.TypeOfWeek}-{data.TypeOfDay}");

            string file = FileName.MainDir + $@"{data.TypeOfWeek}/{data.TypeOfDay}.txt";

            ReadOrCreateFiles(file, 153, callBack.CallbackQuery.From.Id.ToString());
        }

        private async void LessonsInfo(InlineData data, CallbackQueryEventArgs callBack)
        {
            await client.AnswerCallbackQueryAsync(callBack.CallbackQuery.Id);

            TelegramBotLogger.PrintInfo(callBack.CallbackQuery.From.FirstName, callBack.CallbackQuery.From.Id.ToString(),
               callBack.CallbackQuery.From.Username, data.TypeOfLesson.ToString());

            string file = FileName.LissonInfoDir + $"{data.TypeOfLesson}-info.txt";

            ReadOrCreateFiles(file, 151, callBack.CallbackQuery.From.Id.ToString());
        }


        private async void ReadOrCreateFiles(string fileName, long size, string id)
        {

            //string fileName = FileName.MainDir + $"{data.TypeOfWeek}_{data.TypeOfDay}.txt";
            if (!Directory.Exists(FileName.LissonInfoDir)) Directory.CreateDirectory(FileName.LissonInfoDir);
            if (!Directory.Exists(FileName.DenominatorDir)) Directory.CreateDirectory(FileName.DenominatorDir);
            if (!Directory.Exists(FileName.NumeratorDir)) Directory.CreateDirectory(FileName.NumeratorDir);


            if (!File.Exists(fileName))
            {
                using (File.Create(fileName)) ;
            }
            if (File.Exists(fileName))
            {
                long size_;
                using (FileStream fileStream = File.Open(fileName, FileMode.Open))
                    size_ = fileStream.Length;

                if (size_ == 0 || size_ == size)
                {
                    await client.SendTextMessageAsync(id, "Пока информации нету😅");
                }
                else
                {
                    using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
                    using (StreamReader reader = new StreamReader(fileStream))
                        await client.SendTextMessageAsync(id, reader.ReadToEnd());


                }
            }
        }
    }
}
