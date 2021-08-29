using System;
using Newtonsoft.Json;
using Tg_Bot.Interfaces;
using Tg_Bot.Enumerate;

namespace Tg_Bot.ServiceClass
{
    class InlineData :IOrganizationOfInlineData
    {
        public TypeOfWeek Week { get; set; } = TypeOfWeek.None;
        public TypeOfDay Day { get; set; } = TypeOfDay.None;
        public TypeOfLesson Lesson { get; set; } = TypeOfLesson.None;
        public TypeOfButton Button { get; set; } = TypeOfButton.None;
        public string Additionally { get; set; } = String.Empty;

        public string Crypt() => JsonConvert.SerializeObject(this);
        public static string Crypt(InlineData data) => JsonConvert.SerializeObject(data);
        public void ParseIn(string jsonData)
        {
            InlineData obj = JsonConvert.DeserializeObject<InlineData>(jsonData);
            Week = obj.Week;
            Button = obj.Button;
            Day = obj.Day;
            Lesson = obj.Lesson;
            Additionally = obj.Additionally;
        }
        public static InlineData Parse(string jsonData) => JsonConvert.DeserializeObject<InlineData>(jsonData);

    }
}
