using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.University
{
 public   class HSU_316_Dept_AwardsDTO
    {
        public long NMC316DA_Id { get; set; }
        public long NCMCTT343_Id { get; set; }
        public long NCMCTT343F_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long HRMD_Id { get; set; }
        public long NMC316DA_Year { get; set; }
        public string NMC316DA_Scheme { get; set; }
        public string NMC316DA_Agency { get; set; }
        public string NMC316DA_FoundProvided { get; set; }
        public string NMC316DA_Duration { get; set; }
        public DateTime? NMC316DA_CreatedDate { get; set; }
        public DateTime? NMC316DA_UpdatedDate { get; set; }
        public bool NMC316DA_ActiveFlag { get; set; }

        public Array allacademicyear { get; set; }
        public Array alldata1 { get; set; }
        public Array institutionlist { get; set; }
        public string ASMAY_Year { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public long asmaY_Id { get; set; }
        public long hrmD_Id { get; set; }
        public bool duplicate { get; set; }
        public HSU_316_Dept_AwardsDTO[] filelist { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public string msg { get; set; }
        public bool returnval { get; set; }
        public Array editlist { get; set; }
        public Array editFileslist { get; set; }
        public Array viewuploadflies { get; set; }
        public Array departmentlist { get; set; }
        public long NMC316DAF_Id { get; set; }
    }
}
