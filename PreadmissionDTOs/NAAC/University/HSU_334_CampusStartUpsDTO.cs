using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.University
{
   public class HSU_334_CampusStartUpsDTO
    {
        public long NCHSU324CS_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long NCHSU324CS_Year { get; set; }
        public string NCHSU324CS_StartUpName { get; set; }
        public string NCHSU324CS_NatureOfStartUp { get; set; }
        public string NCHSU324CS_Comments { get; set; }
        public string NCHSU324CS_Contactinfo { get; set; }
        public bool NCHSU324CS_ActiveFlag { get; set; }
        public long NCHSU324CS_CreatedBy { get; set; }
        public long NCHSU324CS_UpdatedBy { get; set; }
        public DateTime? NCHSU324CS_CreatedDate { get; set; }
        public DateTime? NCHSU324CS_UpdatedDate { get; set; }


        public Array allacademicyear { get; set; }
        public Array alldata1 { get; set; }
        public Array institutionlist { get; set; }
        public string ASMAY_Year { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public long asmaY_Id { get; set; }
        public long hrmD_Id { get; set; }
        public bool duplicate { get; set; }
        public HSU_334_CampusStartUpsDTO[] filelist { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public string msg { get; set; }
        public bool returnval { get; set; }
        public Array editlist { get; set; }
        public Array editFileslist { get; set; }
        public Array viewuploadflies { get; set; }
        public Array departmentlist { get; set; }
        public long NCHSU324CSF_Id { get; set; }

    }
}
