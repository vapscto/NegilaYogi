using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Library
{
    public class MasterLanguageDTO:CommonParamDTO
    {
        public long LML_Id { get; set; }
        public long MI_Id { get; set; }
        public string LML_LanguageName { get; set; }
        public bool LML_ActiveFlg { get; set; }

        public bool duplicate { get; set; }
        public bool returnval { get; set; }
       public Array langlist { get; set; }
       
    }
}
