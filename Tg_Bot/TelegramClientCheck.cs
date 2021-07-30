using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using Tg_Bot.ServiceClass;

namespace Tg_Bot
{
    static class  TelegramClientCheck
    {
        public static bool NewCheckOfUser_Result
        {
            get { return NewCheckOfUser_Result; }
            private set { NewCheckOfUser_Result = value; }
        }

        public static bool IsFamiliar(string client)
        {

            string[] my_clients;

            using (FileStream fstream = new FileStream(FileName.FriendClient, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(fstream))

                    my_clients = reader.ReadToEnd().Split('\n');
            }

            for (int i = 0; i < my_clients.Length; i++)
            {
                if (client == my_clients[i].Trim('\r'))
                {
                    return true;
                }
            }
            using (FileStream fStream = new FileStream(FileName.BlackList, FileMode.Append))
            {
                StreamWriter writer = new StreamWriter(fStream);
                writer.WriteLine(client);
                writer.Dispose();
            }
            return false;
        }

        public static bool InBlackList(string id)
        {
            if (!File.Exists(FileName.BlackList))
                throw new KNTHelperBotException("File >Black_List.txt< is missing", "Add this File for standart working");

            Regex regex = new Regex(id);
            using (FileStream fStream = new FileStream(FileName.BlackList, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(fStream))
                {
                    return regex.IsMatch(reader.ReadToEnd());
                }
            }
            
        }
    }
}
