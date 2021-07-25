using System;
using System.Collections.Generic;

using System.IO;

namespace Tg_Bot
{
    class Pause
    {
        static public void PauseIvent() { Console.ReadKey(); }
    }
    class Program
    {

        static void Main(string[] args)
        {
            KNT_HelperBot bot = null;
            try
            {
                bot = new KNT_HelperBot();
                bot.StartReciving();
                bot.TurnOn_OfEvent();
                bot.PauseForWorking += Pause.PauseIvent;
                try
                {
                    bot.StopReciving();
                }

                catch (KNTHelperBotException ex)
                {
                    Console.WriteLine(ex.Message + "\n=======\n" + ex.GetWhatToDo());
                }
            }
            catch (Exception ex)
            {
                bot?.StopReciving();
                Console.WriteLine($"\n-=-=-=-=-=-=\n{ex.Message}\n-=-=-=-=-=-=\n");
            }
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
