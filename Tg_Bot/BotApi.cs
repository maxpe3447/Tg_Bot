using System;
using Tg_Bot.ServiceClass;

namespace Tg_Bot
{
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