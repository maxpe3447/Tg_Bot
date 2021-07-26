using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tg_Bot 
{
    class KNTHelperBotException : Exception     //свой класс эксепшин
    {
        string whatToDo; //дополнительное поле
        public KNTHelperBotException(string whatHappend) : base(whatHappend)  //основной конструктор
        {

        }
        public KNTHelperBotException(string whatHappend, string whatToDo) : this(whatHappend)//перегрузка конструтора 
        {
            this.whatToDo = whatToDo;
        }
        public string GetWhatToDo() => whatToDo;//обращение к понлю "что же мне блин делать, а то я никак не пойму что от меня хотят"

    }
}
