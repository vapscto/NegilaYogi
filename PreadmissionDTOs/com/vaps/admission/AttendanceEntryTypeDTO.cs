using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class AttendanceEntryTypeDTO
    {
        public long ASAET_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCL_Id { get; set; }
        public string ASAET_Att_Type { get; set; }

        public string AcedemicYear { get; set; }
        public string ClassName { get; set; }

        public string InstituteName { get; set; }

        public Array yeardropDown { get; set; }
        public Array classdropDown { get; set; }

        public Array AttendanceEntryTypeList { get; set; }

        public List<School_M_ClassDTO> SelectedClassDetails { get; set; }
        public string message { get; set; }
        public bool returnval { get; set; }
        public int count { get; set; }
        public string typechangeflag { get; set; }
        public long yearid { get; set; }

        public Array loadallyear { get; set; }

       // public AdmissionClassDTO[] SelectedClassDetails { get; set; }

    }
}
