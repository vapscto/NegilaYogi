using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Hostel
{
   public class HostelAllotForStudent_DTO
    {
        public long HLMH_Id { get; set; }
        public long MI_Id { get; set; }
        public string HLMH_Name { get; set; }
        public long ASMAY_Id { get; set; }
        public long UserId { get; set; }
        public string ASMAY_Year { get; set; }
        public long ASMCL_Id { get; set; }
        public string ASMCL_ClassName { get; set; }
        public long ASMS_Id { get; set; }
        public string ASMC_SectionName { get; set; }
        public long HRMRM_Id { get; set; }
        public long HRMRM_BedCapacity { get; set; }

        public string studentName { get; set; }
        public string AMST_AdmNo { get; set; }
        public long HLHSALT_Id { get; set; }       
        public DateTime HLHSALT_AllotmentDate { get; set; }        
        public long HLMRCA_Id { get; set; }
        public long AMST_Id { get; set; }        
        public long HLHSALT_NoOfBeds { get; set; }
        public string HLHSALT_AllotRemarks { get; set; }
        public bool HLHSALT_VacateFlg { get; set; }
        public DateTime? HLHSALT_VacatedDate { get; set; }
        public string HLHSALT_VacateRemarks { get; set; }
        public bool HLHSALT_ActiveFlag { get; set; }

        public bool duplicate { get; set; }
        public bool returnval { get; set; }

        public Array hostel_list { get; set; }
        public Array yearlist { get; set; }
        public Array student_allotlist { get; set; }
        public Array housewise_studentList { get; set; }
        public Array classlist { get; set; }
        public Array sectionlist { get; set; }
        public Array roomcatgry_list { get; set; }
        public Array room_list { get; set; }
        public Array editlist { get; set; }




    }
}
