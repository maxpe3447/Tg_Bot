using Tg_Bot.Enumerate;

namespace Tg_Bot.Interfaces
{
    interface IOrganizationOfInlineData
    {
        TypeOfWeek Week { get; set; }
        TypeOfDay Day { get; set; }
        TypeOfLesson Lesson { get; set; }
        TypeOfButton Button { get; set; }
        string Additionally { get; set; }
        string Crypt();

        void ParseIn(string jsonData);
    }
}
