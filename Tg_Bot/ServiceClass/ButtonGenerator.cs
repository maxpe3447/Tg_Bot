using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using System.IO;

namespace Tg_Bot.ServiceClass
{
    class ButtonGenerator
    {
        public IReplyMarkup GetKeyBoardButtons() {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton> { new KeyboardButton { Text = "📋Расписание!📋" } },
                    new List<KeyboardButton> { new KeyboardButton { Text = "📚Предметы!📚" } /*, new KeyboardButton { Text = "💻Конференции!💻" }*/ },
                    new List<KeyboardButton> { new KeyboardButton { Text = "⁉️Вопрос-Ответ!⁉️" } },
                    new List<KeyboardButton> { new KeyboardButton { Text = "📲Связь!📲" } },
                    new List<KeyboardButton> { new KeyboardButton { Text = "💰На Сервер!💰" } }
                }
            };
        }

        public InlineKeyboardButton GetInlineButton_QuestionAnswe()
        {
            InlineKeyboardButton button = new InlineKeyboardButton();

            button.Text = "Чат для вопросов!";
            using (FileStream fstream = new FileStream(FileName.QuesAnsw, FileMode.Open))
            using (StreamReader reader = new StreamReader(fstream)) 
            button.Url = reader.ReadLine();

            return button;
        }

        public InlineKeyboardMarkup GetInlineButtons_TimeTable()
        {
            return new InlineKeyboardMarkup(new[]
                            {
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData(
                                text: "Расписание по числителю!", 
                                callbackData: new InlineData(){
                            TypeOfButton = Enumerate.TypeOfButton.TimeTable,
                            TypeOfWeek = Enumerate.TypeOfWeek.Numerator,
                            TypeOfDay = Enumerate.TypeOfDay.None,
                            TypeOfLesson = Enumerate.TypeOfLesson.None
                            }.Crypt())
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData(
                                text:"Расписание по знаменателю!",
                                callbackData: new InlineData(){
                            TypeOfButton = Enumerate.TypeOfButton.TimeTable,
                            TypeOfWeek = Enumerate.TypeOfWeek.Denominator,
                            TypeOfDay = Enumerate.TypeOfDay.None,
                            TypeOfLesson = Enumerate.TypeOfLesson.None
                            }.Crypt())
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData(
                                text: "Расписание звонков!", 
                                callbackData: new InlineData(){
                            TypeOfButton = Enumerate.TypeOfButton.TimeTable,
                            TypeOfWeek = Enumerate.TypeOfWeek.Call_,
                            TypeOfDay = Enumerate.TypeOfDay.None,
                            TypeOfLesson = Enumerate.TypeOfLesson.None
                            }.Crypt())
                        }
                     });
        }

        public InlineKeyboardMarkup GetInlineButtons_Lessons()
        {
            return new InlineKeyboardMarkup(new[]
                            {
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(
                                    text: "АиСД!",
                                    callbackData: new InlineData(){
                            TypeOfButton = Enumerate.TypeOfButton.Lessons,
                            TypeOfWeek = Enumerate.TypeOfWeek.None,
                            TypeOfDay = Enumerate.TypeOfDay.None,
                            TypeOfLesson = Enumerate.TypeOfLesson.ASD
                            }.Crypt()
                                    ),
                                InlineKeyboardButton.WithCallbackData(
                                    text: "ВМ!",
                                    callbackData: new InlineData(){
                            TypeOfButton = Enumerate.TypeOfButton.Lessons,
                            TypeOfWeek = Enumerate.TypeOfWeek.None,
                            TypeOfDay = Enumerate.TypeOfDay.None,
                            TypeOfLesson = Enumerate.TypeOfLesson.VM
                            }.Crypt())
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(
                                    text: "Л-МВ!",
                                    callbackData: new InlineData(){
                            TypeOfButton = Enumerate.TypeOfButton.Lessons,
                            TypeOfWeek = Enumerate.TypeOfWeek.None,
                            TypeOfDay = Enumerate.TypeOfDay.None,
                            TypeOfLesson = Enumerate.TypeOfLesson.LMV
                            }.Crypt()),
                                InlineKeyboardButton.WithCallbackData(
                                    text: "ОПИ",
                                    callbackData: new InlineData(){
                            TypeOfButton = Enumerate.TypeOfButton.Lessons,
                            TypeOfWeek = Enumerate.TypeOfWeek.None,
                            TypeOfDay = Enumerate.TypeOfDay.None,
                            TypeOfLesson = Enumerate.TypeOfLesson.OPI
                            }.Crypt())
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(
                                    text: "Теор Вер",
                                    callbackData: new InlineData(){
                            TypeOfButton = Enumerate.TypeOfButton.Lessons,
                            TypeOfWeek = Enumerate.TypeOfWeek.None,
                            TypeOfDay = Enumerate.TypeOfDay.None,
                            TypeOfLesson = Enumerate.TypeOfLesson.TV
                                }.Crypt())
                                //InlineKeyboardButton.WithCallbackData("6!")
                            }
                        });
        }

        public InlineKeyboardMarkup GetinlineKeyboard_Conf()
        {
            return new InlineKeyboardMarkup(new[]
                            {
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(
                                    text: "АиСД!",
                                    callbackData: new InlineData(){
                            TypeOfButton = Enumerate.TypeOfButton.Conferences,
                            TypeOfWeek = Enumerate.TypeOfWeek.None,
                            TypeOfDay = Enumerate.TypeOfDay.None,
                            TypeOfLesson = Enumerate.TypeOfLesson.ASD
                                }.Crypt()),
                                InlineKeyboardButton.WithCallbackData(
                                    text: "ВМ!",
                                    callbackData: new InlineData(){
                            TypeOfButton = Enumerate.TypeOfButton.Conferences,
                            TypeOfWeek = Enumerate.TypeOfWeek.None,
                            TypeOfDay = Enumerate.TypeOfDay.None,
                            TypeOfLesson = Enumerate.TypeOfLesson.VM
                                }.Crypt())
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(
                                    text: "Л-МВ!",
                                    callbackData: new InlineData(){
                            TypeOfButton = Enumerate.TypeOfButton.Conferences,
                            TypeOfWeek = Enumerate.TypeOfWeek.None,
                            TypeOfDay = Enumerate.TypeOfDay.None,
                            TypeOfLesson = Enumerate.TypeOfLesson.LMV
                                }.Crypt()),
                                InlineKeyboardButton.WithCallbackData(
                                    text: "ОПИ!",
                                    callbackData: new InlineData(){
                            TypeOfButton = Enumerate.TypeOfButton.Conferences,
                            TypeOfWeek = Enumerate.TypeOfWeek.None,
                            TypeOfDay = Enumerate.TypeOfDay.None,
                            TypeOfLesson = Enumerate.TypeOfLesson.OPI
                                }.Crypt())
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(
                                    text: "Теор Вер!",
                                    callbackData: new InlineData(){
                            TypeOfButton = Enumerate.TypeOfButton.Conferences,
                            TypeOfWeek = Enumerate.TypeOfWeek.None,
                            TypeOfDay = Enumerate.TypeOfDay.None,
                            TypeOfLesson = Enumerate.TypeOfLesson.TV
                                }.Crypt())
                            }
                            });
        }

        public InlineKeyboardMarkup GetinlineKeyboard_DayOfWeek(InlineData data)
        {
            return new InlineKeyboardMarkup(new[]
            {
            new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        text: "Пн", 
                        callbackData: new InlineData(){
                            TypeOfButton = Enumerate.TypeOfButton.DayOfWeek,
                            TypeOfWeek = data.TypeOfWeek,
                            TypeOfDay = Enumerate.TypeOfDay.Monday,
                            TypeOfLesson = Enumerate.TypeOfLesson.None
                                }.Crypt()), //for read from (for example NumeratorMonday.txt)
                    InlineKeyboardButton.WithCallbackData(
                        text: "Вт",
                        callbackData: new InlineData(){
                            TypeOfButton = Enumerate.TypeOfButton.DayOfWeek,
                            TypeOfWeek = data.TypeOfWeek,
                            TypeOfDay = Enumerate.TypeOfDay.Tuesday,
                            TypeOfLesson = Enumerate.TypeOfLesson.None
                                }.Crypt())
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        text: "Ср",
                        callbackData: new InlineData(){
                            TypeOfButton = Enumerate.TypeOfButton.DayOfWeek,
                            TypeOfWeek = data.TypeOfWeek,
                            TypeOfDay = Enumerate.TypeOfDay.Wednesday,
                            TypeOfLesson = Enumerate.TypeOfLesson.None
                                }.Crypt()),
                    InlineKeyboardButton.WithCallbackData(
                        text: "Чт", 
                        callbackData: new InlineData(){
                            TypeOfButton = Enumerate.TypeOfButton.DayOfWeek,
                            TypeOfWeek = data.TypeOfWeek,
                            TypeOfDay = Enumerate.TypeOfDay.Thursday,
                            TypeOfLesson = Enumerate.TypeOfLesson.None
                                }.Crypt())
                },
                  new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        text: "Пт", 
                        callbackData: new InlineData(){
                            TypeOfButton = Enumerate.TypeOfButton.DayOfWeek,
                            TypeOfWeek = data.TypeOfWeek,
                            TypeOfDay = Enumerate.TypeOfDay.Friday,
                            TypeOfLesson = Enumerate.TypeOfLesson.None
                                }.Crypt())
                }
            });
        }
    }
}
