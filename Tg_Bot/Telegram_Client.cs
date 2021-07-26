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
        private static string fileBlackList { get; set; } = "Black_List.txt";//файл черного списка
        private static string fileClientId { get; } = "client_id.txt"; //свои


        public static bool CheckingClient_IsFamiliar(string client)//проверка пользователя свой - истинна, чужой - ложь

        {

            string[] my_clients;

            using (FileStream fstream = new FileStream(fileClientId, FileMode.Open))
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
            using (FileStream fStream = new FileStream(fileBlackList, FileMode.Append))//если его не оказалось в списке своих - пишем в чнрный список
            {
                StreamWriter writer = new StreamWriter(fStream);
                writer.WriteLine(client);
                writer.Dispose();
            }
            return false;
        }

        public static bool CheckInBlackList(string id)//проверка в черном списке
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
