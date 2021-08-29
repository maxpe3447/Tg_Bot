using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tg_Bot.ServiceClass
{
    static class FileName
    {
        public static string MainDir { get; } = @"ServiceFiles/";
        public static string NumeratorDir { get; } = MainDir + @"Numerator/";
        public static string DenominatorDir { get; } = MainDir + @"Denominator/";
        public static string LissonInfoDir { get; } = MainDir + @"LessonInfo/";
        
        public static string Token { get; } = MainDir + @"token.txt";
        public static string DBName { get; } = MainDir + @"db.db";
        public static string ComunicationAnswer { get; } = MainDir + @"Communication.txt";
        public static string Welcome_text { get; } = MainDir + @"Welcome.txt";
        public static string QuesAnsw { get; } = MainDir + @"QuesAnsw.txt";
        public static string TimeTable { get; } = @"https://github.com/maxpe3447/Tg_Bot/blob/develop/Tg_Bot/Image/CallBoard.jpg?raw=true";
        public static string DonateLink { get; } = @"https://send.monobank.ua/jar/2Toamtu3qm";
        public static string DonateQrCode { get; } = @"https://github.com/maxpe3447/Tg_Bot/blob/main/Tg_Bot/Image/QrCode.png?raw=true";
    }
}
