using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class IVRM_Mandatory_Setting_IWDTO
    {


        public long IVRMMSI_Id { get; set; }
        public long MI_Id { get; set; }
        public long IVRMP_Id { get; set; }
        public string IVRMMSI_FieldName { get; set; }

        public string IVRMMSI_Ngmodel { get; set; }
        public bool? IVRMMSI_MandatoryFlag { get; set; }


        public string retrunMsg { get; set; }


        public Array pagedropdown { get; set; }
        public Array pageList { get; set; }
        public Array institutiondropdown { get; set; }

        public string IVRMMP_PageName { get; set; }
        public string MI_Name { get; set; }

        public IVRM_Mandatory_Setting_IWDTO[] mandatoryfieldList { get; set; }
    }
}
