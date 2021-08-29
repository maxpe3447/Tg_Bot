using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tg_Bot
{
    class TelegramBotLogger
    {

        private static int count = 0;
        public static void PrintInfo(string name, string id, string username, string msg = null)
        {

            count++;
            if (count == 1)
            {
                Header_Print();
            }
            if (count == 25)
            {
                count = 0;
            }


            if (msg != null)
            {
                string date = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year + " " + (DateTime.Now.Hour + 3) + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                Console.WriteLine($"| {name,-15} | {id,-10} | {username,-20} | {msg,-20} | {date,-22} |");
            }
        }
        private static void Header_Print()
        {
            string user = "     User", id = "    ID", username = "      Username", msg = "     Operation", date = "     Date & Time";
            
            Console.WriteLine("_______________________________________________________________________________________________________");
            Console.WriteLine($"| {user, -15} | {id, -10} | {username, -20} | {msg, -20} | {date, -22} |");
        }
        public static void PrintBanInfo(string name, string id, string username, string msg = null, string date = null)
        {

            if (msg != null)
            {

                Console.WriteLine($"\n- - - - - - - - - -\n" +
                    $"| {name} | {id} | {username} | {msg} | {date} | -> BAN\n" +
                    $"- - - - - - - - - -\n");
                count = 0;
            }

        }
    }
}
