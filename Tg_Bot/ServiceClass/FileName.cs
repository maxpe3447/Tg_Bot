using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tg_Bot.ServiceClass
{
    static class FileName
    {
        public static string Token { get; } = @"ServiceFiles\token.txt";
        public static string FriendClient { get; } = @"ServiceFiles\permitted_client_id.txt";
        public static string BlackList { get; } = @"ServiceFiles\Black_List.txt";
        public static string ComunicationAnswer { get; } = @"ServiceFiles\Communication.txt";
    }
}
