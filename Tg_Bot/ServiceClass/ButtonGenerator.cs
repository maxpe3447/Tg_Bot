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
                            Button = Enumerate.TypeOfButton.TimeTable,
                            Week = Enumerate.TypeOfWeek.Numerator,
                            Day = Enumerate.TypeOfDay.None,
                            Lesson = Enumerate.TypeOfLesson.None
                            }.Crypt())
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData(
                                text:"Расписание по знаменателю!",
                                callbackData: new InlineData(){
                            Button = Enumerate.TypeOfButton.TimeTable,
                            Week = Enumerate.TypeOfWeek.Denominator,
                            Day = Enumerate.TypeOfDay.None,
                            Lesson = Enumerate.TypeOfLesson.None
                            }.Crypt())
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData(
                                text: "Расписание звонков!", 
                                callbackData: new InlineData(){
                            Button = Enumerate.TypeOfButton.TimeTable,
                            Week = Enumerate.TypeOfWeek.Call_,
                            Day = Enumerate.TypeOfDay.None,
                            Lesson = Enumerate.TypeOfLesson.None
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
                            Button = Enumerate.TypeOfButton.Lessons,
                            Week = Enumerate.TypeOfWeek.None,
                            Day = Enumerate.TypeOfDay.None,
                            Lesson = Enumerate.TypeOfLesson.ASD
                            }.Crypt()
                                    ),
                                InlineKeyboardButton.WithCallbackData(
                                    text: "ВМ!",
                                    callbackData: new InlineData(){
                            Button = Enumerate.TypeOfButton.Lessons,
                            Week = Enumerate.TypeOfWeek.None,
                            Day = Enumerate.TypeOfDay.None,
                            Lesson = Enumerate.TypeOfLesson.VM
                            }.Crypt())
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(
                                    text: "Л-МВ!",
                                    callbackData: new InlineData(){
                            Button = Enumerate.TypeOfButton.Lessons,
                            Week = Enumerate.TypeOfWeek.None,
                            Day = Enumerate.TypeOfDay.None,
                            Lesson = Enumerate.TypeOfLesson.LMV
                            }.Crypt()),
                                InlineKeyboardButton.WithCallbackData(
                                    text: "ОПИ",
                                    callbackData: new InlineData(){
                            Button = Enumerate.TypeOfButton.Lessons,
                            Week = Enumerate.TypeOfWeek.None,
                            Day = Enumerate.TypeOfDay.None,
                            Lesson = Enumerate.TypeOfLesson.OPI
                            }.Crypt())
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(
                                    text: "Теор Вер",
                                    callbackData: new InlineData(){
                            Button = Enumerate.TypeOfButton.Lessons,
                            Week = Enumerate.TypeOfWeek.None,
                            Day = Enumerate.TypeOfDay.None,
                            Lesson = Enumerate.TypeOfLesson.TV
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
                            Button = Enumerate.TypeOfButton.Conferences,
                            Week = Enumerate.TypeOfWeek.None,
                            Day = Enumerate.TypeOfDay.None,
                            Lesson = Enumerate.TypeOfLesson.ASD
                                }.Crypt()),
                                InlineKeyboardButton.WithCallbackData(
                                    text: "ВМ!",
                                    callbackData: new InlineData(){
                            Button = Enumerate.TypeOfButton.Conferences,
                            Week = Enumerate.TypeOfWeek.None,
                            Day = Enumerate.TypeOfDay.None,
                            Lesson = Enumerate.TypeOfLesson.VM
                                }.Crypt())
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(
                                    text: "Л-МВ!",
                                    callbackData: new InlineData(){
                            Button = Enumerate.TypeOfButton.Conferences,
                            Week = Enumerate.TypeOfWeek.None,
                            Day = Enumerate.TypeOfDay.None,
                            Lesson = Enumerate.TypeOfLesson.LMV
                                }.Crypt()),
                                InlineKeyboardButton.WithCallbackData(
                                    text: "ОПИ!",
                                    callbackData: new InlineData(){
                            Button = Enumerate.TypeOfButton.Conferences,
                            Week = Enumerate.TypeOfWeek.None,
                            Day = Enumerate.TypeOfDay.None,
                            Lesson = Enumerate.TypeOfLesson.OPI
                                }.Crypt())
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(
                                    text: "Теор Вер!",
                                    callbackData: new InlineData(){
                            Button = Enumerate.TypeOfButton.Conferences,
                            Week = Enumerate.TypeOfWeek.None,
                            Day = Enumerate.TypeOfDay.None,
                            Lesson = Enumerate.TypeOfLesson.TV
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
                            Button = Enumerate.TypeOfButton.DayOfWeek,
                            Week = data.Week,
                            Day = Enumerate.TypeOfDay.Monday,
                            Lesson = Enumerate.TypeOfLesson.None
                                }.Crypt()), //for read from (for example NumeratorMonday.txt)
                    InlineKeyboardButton.WithCallbackData(
                        text: "Вт",
                        callbackData: new InlineData(){
                            Button = Enumerate.TypeOfButton.DayOfWeek,
                            Week = data.Week,
                            Day = Enumerate.TypeOfDay.Tuesday,
                            Lesson = Enumerate.TypeOfLesson.None
                                }.Crypt())
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        text: "Ср",
                        callbackData: new InlineData(){
                            Button = Enumerate.TypeOfButton.DayOfWeek,
                            Week = data.Week,
                            Day = Enumerate.TypeOfDay.Wednesday,
                            Lesson = Enumerate.TypeOfLesson.None
                                }.Crypt()),
                    InlineKeyboardButton.WithCallbackData(
                        text: "Чт", 
                        callbackData: new InlineData(){
                            Button = Enumerate.TypeOfButton.DayOfWeek,
                            Week = data.Week,
                            Day = Enumerate.TypeOfDay.Thursday,
                            Lesson = Enumerate.TypeOfLesson.None
                                }.Crypt())
                },
                  new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        text: "Пт", 
                        callbackData: new InlineData(){
                            Button = Enumerate.TypeOfButton.DayOfWeek,
                            Week = data.Week,
                            Day = Enumerate.TypeOfDay.Friday,
                            Lesson = Enumerate.TypeOfLesson.None
                                }.Crypt())
                }
            });
        }
    }
}
