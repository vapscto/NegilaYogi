using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class mastercasteDTO :CommonParamDTO
    {
        public long IC_Id { get; set; }
        public long? IMCC_Id { get; set; }
        public string IC_CasteName { get; set; }
        public string IC_CasteDesc { get; set; }
        public Array mastercastename { get; set; }
        public Array GridviewDetails { get; set; }
        public string CategoryName { get; set; }
        public string subcasteName { get; set; }
        public string subcasteCategoryName { get; set; }
        public long? MI_Id { get; set; }
        public string msg { get; set; }
        public bool returnVal { get; set; }
        public int count { get; set; }
        public int count1 { get; set; }

        public bool returnVal_update { get; set; }
        public bool duplicate_caste_name_bool { get; set; }
        public Array getcaste { get; set; }
        public Array getsubcastedetails { get; set; }

        public long IMSC_ID { get; set; }
        public long? IMC_ID { get; set; }
        public string IMSC_Caste_Name { get; set; }
        public string IMSC_Caste_Desc { get; set; }
        public string message { get; set; }
        public Array geteditdata { get; set; }
        public Array getcastelist { get; set; }

    }
}
