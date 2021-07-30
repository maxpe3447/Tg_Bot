using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tg_Bot.Interfaces;
using Tg_Bot.Enumerate;
using Tg_Bot.Enumerate.indexes;

namespace Tg_Bot.ServiceClass
{
    class InlineData : IOrganizationOfInlineData
    {
        public InlineData()
        {
            TypeOfButton = Enumerate.TypeOfButton.None;
            TypeOfWeek = Enumerate.TypeOfWeek.None;
            TypeOfDay = Enumerate.TypeOfDay.None;
            TypeOfLesson = Enumerate.TypeOfLesson.None;
            Additionally = "";
        }
        public InlineData(string data)
        {
            InlineData inlineData = (InlineData)Parse(data);

            TypeOfButton = inlineData.TypeOfButton;
            TypeOfWeek = inlineData.TypeOfWeek;
            TypeOfDay = inlineData.TypeOfDay;
            TypeOfLesson = inlineData.TypeOfLesson;
            Additionally = inlineData.Additionally;
        }
        public TypeOfWeek TypeOfWeek { get; set; } = TypeOfWeek.None;
        public TypeOfDay TypeOfDay { get; set; } = TypeOfDay.None;
        public TypeOfLesson TypeOfLesson { get; set; } = TypeOfLesson.None;
        public TypeOfButton TypeOfButton { get; set; } = TypeOfButton.None;
        public string Additionally { get; set; } = "";

        public static bool operator == (InlineData first, InlineData second) => first.Compare(second);
        public static bool operator !=(InlineData first, InlineData second) => !first.Compare(second);
        public bool Compare(IOrganizationOfInlineData other) =>
            TypeOfWeek == other.TypeOfWeek && TypeOfDay == other.TypeOfDay && TypeOfLesson == other.TypeOfLesson && TypeOfButton == other.TypeOfButton;
        public string Crypt()
        {
            cryptData = ((byte)TypeOfButton).ToString() + '|' + ((byte)TypeOfWeek).ToString() + '|' + ((byte)TypeOfDay).ToString()
                + '|' + ((byte)TypeOfLesson).ToString() + '|' + Additionally;

            return cryptData;
        }
        public IOrganizationOfInlineData Parse()
        {
            string[] unParse = cryptData.Split('|');
            return new InlineData()
            {
                TypeOfButton = (TypeOfButton)byte.Parse(unParse[(byte)IndexForInlineData.TypeOfButton]),
                TypeOfWeek = (TypeOfWeek)byte.Parse(unParse[(byte)IndexForInlineData.TypeOfWeek]),
                TypeOfDay = (TypeOfDay)byte.Parse(unParse[(byte)IndexForInlineData.TypeOfDay]),
                TypeOfLesson = (TypeOfLesson)byte.Parse(unParse[(byte)IndexForInlineData.TypeOfLesson]),
                Additionally = unParse[(byte)IndexForInlineData.Additionally]
            };
        }
        public IOrganizationOfInlineData Parse(string data)
        {
            string[] unParse = data.Split('|');
            return new InlineData()
            {
                TypeOfButton = (TypeOfButton)byte.Parse(unParse[(byte)IndexForInlineData.TypeOfButton]),
                TypeOfWeek = (TypeOfWeek)byte.Parse(unParse[(byte)IndexForInlineData.TypeOfWeek]),
                TypeOfDay = (TypeOfDay)byte.Parse(unParse[(byte)IndexForInlineData.TypeOfDay]),
                TypeOfLesson = (TypeOfLesson)byte.Parse(unParse[(byte)IndexForInlineData.TypeOfLesson]),
                Additionally = unParse[(byte)IndexForInlineData.Additionally]
            };
        }
        public IOrganizationOfInlineData TryParse(bool onException)
        {
            string[] unParse = cryptData.Split('|');
            
                byte temp;
                for (int i = 0; onException && i < unParse.Length; i++)
                {

                    if(!byte.TryParse(unParse[i], out temp))
                    {
                        throw new KNTHelperBotException("It is impossible to parse data in an inline button\n", $"Check passed values| index {i}");
                    }
                }
            return Parse();
        }
        public string GetCryptData() => cryptData;
        private InlineData GetNone()
        {
            return new InlineData()
            {
                TypeOfButton = TypeOfButton.None,
                TypeOfWeek = TypeOfWeek.None,
                TypeOfDay = TypeOfDay.None,
                TypeOfLesson = TypeOfLesson.None,
                Additionally = ""
            };
        }
        private string cryptData;
    }
}
