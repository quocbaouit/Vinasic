using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VINASIC.Object;

namespace VINASIC.Business.Interface.Model
{
    public class ModelTiming : T_Timing
    {
    }

    public class DiaryEvent
    {

        public int ID;
        public string Title;
        public int SomeImportantKeyID;
        public string StartDateString;
        public string EndDateString;
        public string StatusString;
        public string StatusColor;
        public string ClassName;
    }
}
