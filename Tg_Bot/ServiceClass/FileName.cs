using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tg_Bot.ServiceClass
{
    static class FileName
    {
        public static string Token { get; } = @"ServiceFiles/token.txt";
        public static string FriendClient { get; } = @"ServiceFiles/permitted_client_id.txt";
        public static string BlackList { get; } = @"ServiceFiles/Black_List.txt";
        public static string ComunicationAnswer { get; } = @"ServiceFiles/Communication.txt";
        public static string Welcome_text { get; } = @"ServiceFiles/Welcome.txt";
        public static string QuesAnsw { get; } = @"ServiceFiles/QuesAnsw.txt";
        public static string TimeTable { get; } = @"https://github.com/maxpe3447/Tg_Bot/blob/develop/Tg_Bot/Image/CallBoard.jpg?raw=true";
        public static string DonateLink { get; } = @"https://send.monobank.ua/jar/2Toamtu3qm";
        public static string DonateQrCode { get; } = @"https://github.com/maxpe3447/Tg_Bot/blob/main/Tg_Bot/Image/QrCode.png?raw=true";
    }
}
