using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tg_Bot 
{
    class KNTHelperBotException : Exception
    {
        string whatToDo;
        public KNTHelperBotException(string whatHappend) : base(whatHappend)
        {

        }
        public KNTHelperBotException(string whatHappend, string whatToDo) : this(whatHappend)
        {
            this.whatToDo = whatToDo;
        }

        public string GetWhatToDo()
        {
            return whatToDo;
        }
    }
}
