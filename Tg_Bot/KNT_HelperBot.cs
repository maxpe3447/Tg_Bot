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
        private string Token { get; set; }  //храниться токен
        private TelegramBotClient client;   //обьект клиент-бот
        private InlineKeyboardMarkup inlineKeyboard_TimeTable; //переменная в которой храняться кнопки с расписанием
        byte type = 0, day = 1, id = 2; //индексы для инлайн кнопок(далее будет понятно зачем)

        public delegate void PauseForWork();        //делегат для события поуза
        public event PauseForWork PauseForWorking;  //событие пауза

        public KNT_HelperBot()  //конструктор
        {
            using (FileStream fstream = new FileStream("token.txt", FileMode.Open))    //считываем токен
            {
                using (StreamReader reader = new StreamReader(fstream))
                    Token = reader.ReadLine();  //ложим в перемнную
            }
            client = new TelegramBotClient(Token);  //связываем бота с токеном

        }

        public void StartReciving() //метод для начала прослушивания сигналов от пользователя
        {
            Console.WriteLine(client.GetMeAsync().Result);  //выводим на консоль сообщение о том, что бот заработал
            try     //на всякий случай ловим исключение, что бы корректно окончить работу бота(остановить прослушку)
            {
                client.StartReceiving();    //начало прослушки
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);  //выводим исключение 
                client.StopReceiving();         //останавливаем прослушку
            }
        }
        public void TurnOn_OfEvent()            //что бы бот отвечал на нажатия кнопок нужно прописать события, все события собраны в этом методе
        {
            try //ловим исключение по той же причине
            {
                client.OnMessage += StartMessege;   //события на прием сообщений
                client.OnCallbackQuery += CallBackInlineQuaryMain;  //собитие на нажатие первых(главных) встроенных кнопок
                client.OnCallbackQuery += CallBackInlineQuaryForDayOfWeek;  //собитие на вывод информации по дням недели(инлайн кнопки)
                client.OnCallbackQuery += CallBackInlineQuaryForSubjType;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                client.StopReceiving();
            }
        }

        public void StopReciving()
        {
            if (PauseForWorking == null)    //проверяем наше событие, которое мы написали в этом классе на то включено оно или нет, если оно выключено
            {                               //то завершаем работу бота, помнишь ты писала Console.ReadKey() тоесть пока не нажата кнопка - бот работает, так вот 
                                            //к этому ивенту будет подключаться метод с паузой, сделано это для того, что б этот класс был более универсальный
                                            //и при переходе на грфический интерфейс, мы легко смогли бы использовать этот класс без сильных изменений 
                client.StopReceiving();     //останавливаем работу бота, если паузы нету

                //выбрасываем исключение нашего класса эксепшин с описанием того, что произошло и как это исправить
                throw new KNTHelperBotException("------>\nThe object has no pause event\n<------\n", "add to Event of =>PauseForWorking<= function for pause:\n" +
                    "for Example:\n" +
                    "public void PauseIvent()\n" +
                    "{\n" +
                    "Console.ReadKey();\n" +
                    "}\n");
            }


            PauseForWorking?.Invoke();  //если пауза существует - запускаем
            client.StopReceiving();     //по завершению паузы останавливаем работу бота

        }

        private async void StartMessege(object sender, MessageEventArgs e)
        {
            var msg = e.Message;
            if (msg != null)
            {
                bool userInBlackList = false;   //создам флажек, что б запомнить работу метода Telegram_Client.CheckInBlackList(msg.From.Id.ToString());
                                                //это флажек отвечает за то в черном списке человек или нет
                try
                {
                    userInBlackList = Telegram_Client.CheckInBlackList(msg.From.Id.ToString()); //проверяем и за одно ловим исключение 
                }
                catch (KNTHelperBotException ex)
                {
                    Console.WriteLine(ex.Message + "\n=======\n" + ex.GetWhatToDo());   //если поймали выводим что это и как с этим бороться
                }

                if (!userInBlackList)   //если пользователь не в черном списке - ему доступны сообщения 
                {
                    if (msg.Text == "/start")
                    {

                        if (!Telegram_Client.CheckingClient_IsFamiliar(msg.From.Id.ToString())) //если это не один из наших, то записываем в черный список и отправляем уведомление

                        {
                            await client.SendTextMessageAsync(msg.Chat.Id, $"Слушай, {msg.From.FirstName}🤨 ты не отсюдого, тебе низя 😋");
                            await client.SendTextMessageAsync(msg.Chat.Id, "😏");

                            Console.WriteLine($"[{e.Message.From.FirstName}] - [{e.Message.From.Id}] - [{e.Message.From.Username}] | BAN!");
                            goto EndOfListenOfMsg; //перепрыгиваем  концу метода
                        }
                        //дальше все по стандарту
                        await client.SendTextMessageAsync(msg.Chat.Id,
                                "Добро пожаловать в данный чат-бот!🙃\n" +
                                "Здесь есть почти вся необходимая информация что бы учиться на 2м курсе😌\n" +
                                "Удачи в обучении!✨", replyMarkup: GetKeyBoardButtons());
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
                                InlineKeyboardButton.WithCallbackData("АиСД!"),
                                InlineKeyboardButton.WithCallbackData("ВМ!")
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Л-МВ!"),
                                InlineKeyboardButton.WithCallbackData("ОПИ!")
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Теор Вер!")
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

                            var inlineKeyboard_4 = new InlineKeyboardMarkup(new[]
                            {
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("АиСД!", callbackData: $"AiSD||{msg.From.Id}"),
                                InlineKeyboardButton.WithCallbackData("ВМ!", callbackData: $"VM||{msg.From.Id}")
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Л-МВ!", callbackData: $"LMV||{msg.From.Id}"),
                                InlineKeyboardButton.WithCallbackData("ОПИ!", callbackData: $"OPI||{msg.From.Id}")
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Теор Вер!", callbackData: $"TeorVer||{msg.From.Id}")
                            }
                            });
                            await client.SendTextMessageAsync(msg.From.Id, "Выберите предмет:", replyMarkup: inlineKeyboard_4);

                            break;


                        /*

                        Мой контакт 

                         */

                        case "Связь!":

                            string phone, name;

                            using (FileStream fstream = new FileStream("phone.txt", FileMode.Open))
                            {
                                using (StreamReader reader = new StreamReader(fstream))
                                    phone = reader.ReadToEnd();
                            }
                            using (FileStream fstream = new FileStream("name.txt", FileMode.Open))
                            {
                                using (StreamReader reader = new StreamReader(fstream))
                                    name = reader.ReadToEnd();
                            }

                            await client.SendTextMessageAsync(msg.Chat.Id, "Вот ваша староста Анастасия😌\n" +
                                "Её номерок: " + phone +
                                "😉\nЕё ник: " + name + " 🙂");
                            break;
                    }
                }
                else
                    Console.WriteLine($"[{e.Message.From.FirstName}] - [{e.Message.From.Id}] - [{e.Message.From.Username}] | BAN!");

                EndOfListenOfMsg:;
            }
        }

        [Obsolete]


        private void CallBackInlineQuaryMain(object sender, CallbackQueryEventArgs callBack)//обработка нажатия инлайн кнопки
        {
           
            Console.WriteLine($"[{callBack.CallbackQuery.From.FirstName}] - [{callBack.CallbackQuery.From.Id}] - [{callBack.CallbackQuery.From.Username}] | " 
                + callBack.CallbackQuery.Data + '\t' + callBack.CallbackQuery.InlineMessageId);//выводим оповищение о нажатии на консоль

            string stype = callBack.CallbackQuery.Data.Split('|')[type];//тип кнопки
            
            if ( stype != "Call" && callBack.CallbackQuery.Data.Split('|')[day] == "")//если тип числитель или наменатель, а день еще не задан - спрашиваем за дни
                TypeOfWeek(callBack);
            else if(stype == "Call")//если звонки 
                GetCallBordImage(callBack);//отправляем картинку
        }
        private async void GetCallBordImage(CallbackQueryEventArgs callBack)//метод отправки картинки
        {
            await client.AnswerCallbackQueryAsync(callBack.CallbackQuery.Id);
            await client.SendPhotoAsync(callBack.CallbackQuery.Data.Split('|')[id], "https://github.com/maxpe3447/Tg_Bot/blob/develop/Tg_Bot/bin/Debug/net5.0/CallBoard.jpg?raw=true");
        }
        private async void TypeOfWeek(CallbackQueryEventArgs callBack)//метод который выводит дни после выбора недели
        {
            await client.AnswerCallbackQueryAsync(callBack.CallbackQuery.Id, $"ля, кого я вижу, да, {callBack.CallbackQuery.From.FirstName} 🤨");

            string[] date = callBack.CallbackQuery.Data.Split('|');//снова разбиваем на массив данных

            var inlineKeyboard_DayOfWeek = GetinlineKeyboard_DayOfWeek(date); //генерируем кнопки дней недели

            await client.SendTextMessageAsync(date[id], "Выбери день недели:", replyMarkup: inlineKeyboard_DayOfWeek);//выводим кнопки и порсим выбрать день
        }
        InlineKeyboardMarkup GetinlineKeyboard_DayOfWeek(string[] date)//генерация кнопок дней недели

        {
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

        private async void CallBackInlineQuaryForDayOfWeek(object sender, CallbackQueryEventArgs callBack)//обработка нажатия на день недели

        {

            string[] date = callBack.CallbackQuery.Data.Split('|');

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

        private IReplyMarkup GetKeyBoardButtons()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton> { new KeyboardButton { Text = "Расписание!" } },
                    new List<KeyboardButton> { new KeyboardButton { Text = "Предметы!" }, new KeyboardButton { Text = "Конференции!" } },
                    new List<KeyboardButton> { new KeyboardButton { Text = "Вопрос-Ответ!" } },
                    new List<KeyboardButton> { new KeyboardButton { Text = "Связь!" } }
                }
            };
        }


        private async void CallBackInlineQuaryForSubjType(object sender, CallbackQueryEventArgs callBack)
        {

            string subj = callBack.CallbackQuery.ToString();

            switch(subj)
            {
                case "AiSD":

                    var inlineKeyboard_AiSD = new InlineKeyboardMarkup(new[]
                    {
                    new[]
                    {
                        InlineKeyboardButton.WithUrl("Лекция!", "link_for_lection")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithUrl("Лб 1!", "https://t.me/joinchat/V69YheCJ-Fb9q8mJ"),
                        InlineKeyboardButton.WithUrl("Лб 2!", "https://t.me/joinchat/V69YheCJ-Fb9q8mJ"),
                    }
                    });

                    await client.SendTextMessageAsync(callBack.CallbackQuery.Id, "Держи!", replyMarkup: inlineKeyboard_AiSD);

                    break;

                case "VM":

                    var inlineKeyboard_VM = new InlineKeyboardMarkup(new[]
                    {
                    new[]
                    {
                        InlineKeyboardButton.WithUrl("Лекция!", "link_for_lection")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithUrl("Лб 1!", "link_for_lb"),
                        InlineKeyboardButton.WithUrl("Лб 2!", "link_for_lb"),
                    }
                    });

                    await client.SendTextMessageAsync(callBack.CallbackQuery.Id, "Держи!", replyMarkup: inlineKeyboard_VM);

                    break;

                case "LMV":

                    var inlineKeyboard_LMV = new InlineKeyboardMarkup(new[]
                    {
                    new[]
                    {
                        InlineKeyboardButton.WithUrl("Лекция!", "link_for_lection")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithUrl("Лб 1!", "link_for_lb"),
                        InlineKeyboardButton.WithUrl("Лб 2!", "link_for_lb"),
                    }
                    });

                    await client.SendTextMessageAsync(callBack.CallbackQuery.Id, "Держи!", replyMarkup: inlineKeyboard_LMV);

                    break;

                case "OPI":

                    var inlineKeyboard_OPI = new InlineKeyboardMarkup(new[]
                    {
                    new[]
                    {
                        InlineKeyboardButton.WithUrl("Лекция!", "link_for_lection")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithUrl("Лб 1!", "link_for_lb"),
                        InlineKeyboardButton.WithUrl("Лб 2!", "link_for_lb"),
                    }
                    });

                    await client.SendTextMessageAsync(callBack.CallbackQuery.Id, "Держи!", replyMarkup: inlineKeyboard_OPI);

                    break;

                case "TeorVer":

                    var inlineKeyboard_TeorVer = new InlineKeyboardMarkup(new[]
                    {
                    new[]
                    {
                        InlineKeyboardButton.WithUrl("Лекция!", "link_for_lection")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithUrl("Лб 1!", "link_for_lb"),
                        InlineKeyboardButton.WithUrl("Лб 2!", "link_for_lb"),
                    }
                    });

                    await client.SendTextMessageAsync(callBack.CallbackQuery.Id, "Держи!", replyMarkup: inlineKeyboard_TeorVer);

                    break;

            }

            
            
        }

    }
}
