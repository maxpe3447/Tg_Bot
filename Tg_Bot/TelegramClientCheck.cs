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

        public static bool IsFamiliar(string id)
        {
            Regex regex = new Regex(id);
            using (FileStream fStream = new FileStream(FileName.FriendClient, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(fStream))
                {
                    
                    if (regex.IsMatch(reader.ReadToEnd()))
                    {
                        return true;
                    }
                }
            }           
            
            using (FileStream fStream = new FileStream(FileName.BlackList, FileMode.Append))
            {
                StreamWriter writer = new StreamWriter(fStream);
                writer.WriteLine(id);
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

        public static bool IsAdmins(string id)
        {
            Regex regex = new Regex(id);
            using (FileStream fStream = new FileStream(FileName.Admin, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(fStream))
                {
                    return regex.IsMatch(reader.ReadToEnd());
                }
            }
        }
    }
}
