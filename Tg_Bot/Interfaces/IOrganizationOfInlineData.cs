using Tg_Bot.Enumerate;

namespace Tg_Bot.Interfaces
{
    interface IOrganizationOfInlineData
    {
        public TypeOfWeek TypeOfWeek { get; set; }
        public TypeOfDay TypeOfDay { get; set; }
        public TypeOfLesson TypeOfLesson { get; set; }
        public TypeOfButton TypeOfButton { get; set; }
        bool Compare(IOrganizationOfInlineData other);
        string Crypt();
        IOrganizationOfInlineData Parse();
        IOrganizationOfInlineData TryParse( bool onException);
    }
}
