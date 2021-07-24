using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace Tg_Bot
{
    static class  Telegram_Client
    {
        private static string fileBlackList { get; set; } = "Black_List.txt";
        private static string fileClientId { get; } = "client_id.txt";
        public static bool NewCheckOfUser_Result
        {
            get { return NewCheckOfUser_Result; }
            private set { NewCheckOfUser_Result = value; }
        }

        //public static Telegram_Client(/*string client*/)
        //{
        //    //cl_for_check = client;
        //}
        public static bool CheckingClient_IsFamiliar(string client)
        {

            string[] my_clients;

            using (FileStream fstream = new FileStream(fileClientId, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(fstream))

                    my_clients = reader.ReadToEnd().Split('\n');
            }

            for (int i = 0; i < my_clients.Length; i++)
            {
                if (client == my_clients[i])
                {
                    return true;
                }
            }
            using (FileStream fStream = new FileStream(fileBlackList, FileMode.Append))
            {
                StreamWriter writer = new StreamWriter(fStream);
                writer.WriteLine(client);
                writer.Dispose();
            }
            return false;
        }

        public static bool CheckInBlackList(string id)
        {
            if (!File.Exists(fileBlackList))
                throw new KNTHelperBotException("File >Black_List.txt< is missing", "Add this File for standart working");

            Regex regex = new Regex(id);
            using (FileStream fStream = new FileStream(fileBlackList, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(fStream))
                {
                    return regex.IsMatch(reader.ReadToEnd());
                }
            }
            
        }
    }
}
