using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Hostel
{
   public class HostelAllotForStaff_DTO
    {

        public long HLHSTALT_Id { get; set; }
        public long MI_Id { get; set; }
        public DateTime HLHSTALT_AllotmentDate { get; set; }
        public long HLMH_Id { get; set; }
        public long HLMRCA_Id { get; set; }
        public long HRME_Id { get; set; }
        public long HRMRM_Id { get; set; }
        //public long HLHSTALT_NoOfBeds { get; set; }
        public string HLHSTALT_AllotRemarks { get; set; }
        public bool HLHSTALT_VacateFlg { get; set; }
        public DateTime HLHSTALT_VacatedDate { get; set; }
        public string HLHSTALT_VacateRemarks { get; set; }
        public bool HLHSTALT_ActiveFlag { get; set; }
        public DateTime? HLHSTALT_CreatedDate { get; set; }
        public DateTime? HLHSTALT_UpdatedDate { get; set; }
        public long HLHSTALT_UpdatedBy { get; set; }
        public long HLHSTALT_CreatedBy { get; set; }
        public long UserId { get; set; }
        public long ASMAY_Id { get; set; }
        public string ASMAY_Year { get; set; }
        public Array yearlist { get; set; }
        public string HLMH_Name { get; set; }
        public Array hostel_list { get; set; }
        public Array roomcatgry_list { get; set; }
        public Array deptlist { get; set; }
        public Array desglist { get; set; }
        public Array room_list { get; set; }
        public Array emp_list { get; set; }
        public Array student_allotlist { get; set; }
        public Array housewise_studentList { get; set; }
        public long HRMD_Id { get; set; }
        public bool duplicate { get; set; }
        public bool ret { get; set; }
        public bool returnval { get; set; }
        public long HRMDES_Id { get; set; }
        public long HRMRM_BedCapacity { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public Array editlist { get; set; }

    }
}
