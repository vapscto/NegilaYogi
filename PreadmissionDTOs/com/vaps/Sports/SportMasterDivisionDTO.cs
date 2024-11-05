using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Sport
{
    public class SportMasterDivisionDTO:CommonParamDTO
    {
        public long SPCCMD_Id { get; set; }
        public long MI_Id { get; set; }
        public string SPCCMD_DivisionName { get; set; }
        public string SPCCMD_DivisionDescription { get; set; }
        public bool SPCCMD_ActiveFlag { get; set; }
       
        public Array mastercastename { get; set; }
        public Array GridviewDetails { get; set; }
        public int count { get; set; }
        public string msg { get; set; }
        public bool returnVal { get; set; }
        public bool returnVal_update { get; set; }
        public bool duplicate_caste_name_bool { get; set; }
    }
}
