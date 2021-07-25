using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace Tg_Bot
{
    class Telegram_Client
    {
        public static string cl_for_check;

        Telegram_Client(string client)
        {
            cl_for_check = client;

        }
        public static bool Checking_Client()
        {

            string[] my_clients;

            using (FileStream fstream = new FileStream("token.txt", FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(fstream))

                    my_clients = reader.ReadToEnd().Split('\n');
            }

            for (int i = 0; i < my_clients.Length; i++)
            {
                if (cl_for_check == my_clients[i])
                {
                    return true;
                }
            }

            return false;
        }
    }
}
