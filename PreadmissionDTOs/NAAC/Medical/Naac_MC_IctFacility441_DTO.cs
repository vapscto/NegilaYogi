using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Medical
{
   public class Naac_MC_IctFacility441_DTO
    {
        public long UserId { get; set; }
  
        public Array institutionlist { get; set; }
        public Array allacademicyear { get; set; }
        public Array alldata1 { get; set; }
        public string ASMAY_Year { get; set; }
        public bool returnval { get; set; }

        public long NCMCCTTF441_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCMCCTTF441_Year { get; set; }
        public long ASMAY_Id { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string msg { get; set; }
        public string cfiledesc { get; set; }
        public Array editFileslist { get; set; }
        public Array editlist { get; set; }
        public long NCMCCTTF441F_Id { get; set; }
        public Array viewuploadflies { get; set; }
        public Naac_MC_IctFacility441_DTO[] filelist { get; set; }
        public long NCMCCTTF441_NoOfClassroomsSeminarHallsLCD { get; set; }
        public long NCMCCTTF441_NoOfClassroomsSeminarHallsLCDLan { get; set; }
        public long NCMCCTTF441_NoOfClassroomsSeminarHallsLCDSmartboardLan { get; set; }
        public long NCMCCTTF441_NoOfClassroomsSeminarHallsLCDSmartboardLanAuVi { get; set; }
        public long NCMCCTTF441_TotalNoOfClassSeminarHalls { get; set; }
        public long NCMCCTTF441_CreatedBy { get; set; }
        public long NCMCCTTF441_UpdatedBy { get; set; }
        public DateTime NCMCCTTF441_CreateDate { get; set; }
        public DateTime NCMCCTTF441_UpdatedDate { get; set; }


    }
}
