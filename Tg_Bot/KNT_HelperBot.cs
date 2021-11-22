using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using Tg_Bot.ServiceClass;
using Tg_Bot.Interfaces;
using Tg_Bot.Enumerate;
using Microsoft.Data.Sqlite;
using System.Linq;

namespace Tg_Bot
{
    class KNT_HelperBot : IManegeKNTBot
    {
        private string Token { get; set; }
        private TelegramBotClient client;

        public delegate void PauseForWork();
        public event PauseForWork PauseForWorking;

        private Server.Server server;

        private Timer timer;
        private SortedDictionary<DateTime, string> events;

        public KNT_HelperBot()
        {

            Token = File.ReadLines(FileName.Token).First();
            
            client = new TelegramBotClient(Token);

            server = new Server.Server();

            Init();
            
        }
        void Init()
        {
            server.NewEvent += Server_NewEvent;
            timer = new Timer(TimerCallback, null, 0, 1000);
            events = new SortedDictionary<DateTime, string>();
        }

        private void TimerCallback(Object o)
        {
            if (events == null || events.Count == 0)
            {
                return;
            }
            if(events.First().Key <= DateTime.Now.ToUniversalTime())
            {
                SendAllMsg(events.First().Value);
                events.Remove(events.First().Key);
            }
        }
        private async void SendAllMsg(string msg)
        {
            using (SqliteConnection connection = new SqliteConnection($"Data Source ={FileName.DBName}"))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand();

                command.Connection = connection;

                command.CommandText = "SELECT * FROM FriendUsers";

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                await client.SendTextMessageAsync(long.Parse(reader["Tg_id"].ToString()), $"{msg}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"No chat: {reader["Name"]}");
                            }
                        }
                    }
                }
            }
        }
        private void Server_NewEvent()
        {
            DateTime date = DateTime.ParseExact(server.LastEvent.Item1, "dd/MM/yyyy HH:mm:ss tt", null);
            events.Add(date, server.LastEvent.Item2);
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
            server.TurnOnAsync();
        }

        [Obsolete]
        public void TurnOn_OfEvent()
        {
            try
            {
                client.OnMessage += StartMessege;
                client.OnCallbackQuery += CallBackInlineQuaryMain;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                client.StopReceiving();
            }
        }

        [Obsolete]
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

        [Obsolete]
        private async void StartMessege(object sender, MessageEventArgs e)
        {
            var msg = e.Message;
            if (msg != null)
            {
                bool userInBlackList = false;

                try
                {
                    userInBlackList = TelegramClientCheck.InBlackList(msg.From);
                }
                catch (KNTHelperBotException ex)
                {
                    Console.WriteLine(ex.Message + "\n=======\n" + ex.GetWhatToDo());
                }

                TelegramClientCheck.AddToNewUsers(msg.From);

                if (!userInBlackList)
                {
                    if (msg.Text == "/start")
                    {
                        if (!TelegramClientCheck.IsFriend(msg.From))
                        {
                            await client.SendTextMessageAsync(msg.From.Id, $"Слушай, {msg.From.FirstName}🤨 ты не отсюдого, тебе низя 😋");
                            await client.SendTextMessageAsync(msg.From.Id, "😏");

                            TelegramBotLogger.PrintBanInfo(e.Message.From.FirstName, e.Message.From.Id.ToString(), e.Message.From.Username, msg.Text);
                            goto EndOfListenOfMsg;
                        }

                            await client.SendTextMessageAsync(msg.Chat.Id, File.ReadAllText(FileName.Welcome_text), replyMarkup: new ButtonGenerator().GetKeyBoardButtons());
                                                
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

                            TelegramBotLogger.PrintInfo(e.Message.From.FirstName, e.Message.From.Id.ToString(), e.Message.From.Username, TypeOfButton.Сommunication.ToString());

                            await client.SendTextMessageAsync(msg.Chat.Id, File.ReadAllText(FileName.ComunicationAnswer));

                            break;


                        case "💰На Сервер!💰":

                            TelegramBotLogger.PrintInfo(e.Message.From.FirstName, e.Message.From.Id.ToString(), e.Message.From.Username, TypeOfButton.ToServer.ToString());

                            await client.SendTextMessageAsync(msg.Chat.Id, File.ReadAllText(FileName.DonateLink));
                            await client.SendTextMessageAsync(msg.Chat.Id, "Или воспользуйтесь Qr-кодом для совершения доната, заранее спасибки🤗😌");
                            await client.SendPhotoAsync(msg.Chat.Id, FileName.DonateQrCode);
                            break;
                        default:

                            TelegramBotLogger.PrintInfo(e.Message.From.FirstName, e.Message.From.Id.ToString(), e.Message.From.Username, msg.Text);

                            break;
                    }
                }
                else
                    TelegramBotLogger.PrintBanInfo(e.Message.From.FirstName, e.Message.From.Id.ToString(), e.Message.From.Username, msg.Text);

                EndOfListenOfMsg:;
            }
        }
        [Obsolete]
        private void CallBackInlineQuaryMain(object sender, CallbackQueryEventArgs callBack)
        {
            InlineData inlineData =  InlineData.Parse(callBack.CallbackQuery.Data);

            switch (inlineData.Button)
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
                    Conf(inlineData, callBack);
                    break;
                default:
                    break;
            }
        }

        [Obsolete]
        private async void TimeTable(InlineData data, CallbackQueryEventArgs callBack)
        {
            await client.AnswerCallbackQueryAsync(callBack.CallbackQuery.Id, $"ля, кого я вижу, {callBack.CallbackQuery.From.FirstName} 🤨");

            TelegramBotLogger.PrintInfo(callBack.CallbackQuery.From.FirstName, callBack.CallbackQuery.From.Id.ToString(),
               callBack.CallbackQuery.From.Username, data.Week.ToString());

            switch (data.Week)
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

        [Obsolete]
        private async void DayOfWeek(InlineData data, CallbackQueryEventArgs callBack)
        {
            await client.AnswerCallbackQueryAsync(callBack.CallbackQuery.Id);
            //await client.SendTextMessageAsync(callBack.CallbackQuery.From.Id, $"Ты выбрал {data.TypeOfDay}, тип недели {data.TypeOfWeek}:\n");

            TelegramBotLogger.PrintInfo(callBack.CallbackQuery.From.FirstName, callBack.CallbackQuery.From.Id.ToString(),
               callBack.CallbackQuery.From.Username, $"{data.Week}-{data.Day}");

            string file = FileName.MainDir + $@"{data.Week}/{data.Day}.txt";

            ReadOrCreateFiles(file, 153, callBack.CallbackQuery.From.Id.ToString());
        }

        [Obsolete]
        private async void LessonsInfo(InlineData data, CallbackQueryEventArgs callBack)
        {
            await client.AnswerCallbackQueryAsync(callBack.CallbackQuery.Id);

            TelegramBotLogger.PrintInfo(callBack.CallbackQuery.From.FirstName, callBack.CallbackQuery.From.Id.ToString(),
               callBack.CallbackQuery.From.Username, data.Lesson.ToString());

            string file = FileName.LissonInfoDir + $"{data.Lesson}-info.txt";

            ReadOrCreateFiles(file, 151, callBack.CallbackQuery.From.Id.ToString());
        }

        private async void Conf(InlineData data, CallbackQueryEventArgs callBack)
        {
            await client.AnswerCallbackQueryAsync(callBack.CallbackQuery.Id);
            string fileName = FileName.ConferencDir + $"{data.Lesson}.txt";
            ReadOrCreateFiles(fileName, 0, callBack.CallbackQuery.From.Id.ToString());

            TelegramBotLogger.PrintInfo(callBack.CallbackQuery.From.FirstName, callBack.CallbackQuery.From.Id.ToString(), callBack.CallbackQuery.From.Username, data.Lesson.ToString());
        }

        private async void ReadOrCreateFiles(string fileName, long size, string id)
        {

            //string fileName = FileName.MainDir + $"{data.TypeOfWeek}_{data.TypeOfDay}.txt";
            if (!Directory.Exists(FileName.LissonInfoDir)) Directory.CreateDirectory(FileName.LissonInfoDir);
            if (!Directory.Exists(FileName.DenominatorDir)) Directory.CreateDirectory(FileName.DenominatorDir);
            if (!Directory.Exists(FileName.NumeratorDir)) Directory.CreateDirectory(FileName.NumeratorDir);
            if (!Directory.Exists(FileName.ConferencDir)) Directory.CreateDirectory(FileName.ConferencDir);


            if (!File.Exists(fileName))
            {
                using (File.Create(fileName)) { }
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
                        await client.SendTextMessageAsync(id, File.ReadAllText(fileName));
                }
            }
        }
    }
}
