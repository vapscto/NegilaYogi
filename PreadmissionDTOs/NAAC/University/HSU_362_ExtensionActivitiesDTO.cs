using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.University
{
  public  class HSU_362_ExtensionActivitiesDTO
    {
        public long NCACSA343_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long NCACSA343F_Id { get; set; }
        public long NCACSA343_Year { get; set; }
        public long NCACSA343_NoOfTeachers { get; set; }
        public long NCACSA343_NoOfStudents { get; set; }
        public DateTime? NCACSA343_ActivityDate { get; set; }
        public string NCACSA343_TypeOfActivity { get; set; }
        public string NCACSA343_OrgAgency { get; set; }
        public string NCACSA343_Place { get; set; }
        public string NCACSA343_Duration { get; set; }
        public bool NCACSA343_ActiveFlg { get; set; }
        public long NCACSA343_CreatedBy { get; set; }
        public long NCACSA343_UpdatedBy { get; set; }
        public DateTime? NCACSA343_CreatedDate { get; set; }
        public DateTime? NCACSA343_UpdatedDate { get; set; }

        public Array allacademicyear { get; set; }
        public Array alldata1 { get; set; }
        public Array institutionlist { get; set; }
        public string ASMAY_Year { get; set; }
        public long asmaY_Id { get; set; }
        public bool duplicate { get; set; }
        public HSU_362_ExtensionActivitiesDTO[] filelist { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public string msg { get; set; }
        public bool returnval { get; set; }
        public Array editlist { get; set; }
        public Array editFileslist { get; set; }
        public Array viewuploadflies { get; set; }
        public long NCMC8110IMMF_Id { get; set; }
    }
}
