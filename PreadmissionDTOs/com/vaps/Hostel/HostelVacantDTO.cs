using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Hostel
{
    public class StudentVacantDTO
    {
        public long HLHSALT_Id { get; set; }
        public long MI_Id { get; set; }
        public DateTime HLHSALT_AllotmentDate { get; set; }
        public long ASMAY_Id { get; set; }
        public long HLMH_Id { get; set; }
        public long HLMRCA_Id { get; set; }
        public long? AMST_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long HRMRM_Id { get; set; }
        public long HLHSALT_NoOfBeds { get; set; }
        public string HLHSALT_AllotRemarks { get; set; }
        public bool HLHSALT_VacateFlg { get; set; }
        public DateTime? HLHSALT_VacatedDate { get; set; }
        public string HLHSALT_VacateRemarks { get; set; }
        public bool HLHSALT_ActiveFlag { get; set; }
        public DateTime? HLHSALT_CreatedDate { get; set; }
        public DateTime? HLHSALT_UpdatedDate { get; set; }
        public long HLHSALT_UpdatedBy { get; set; }
        public long HLHSALT_CreatedBy { get; set; }
        public long HRMD_Id { get; set; }
        public long HRMDES_Id { get; set; }
        public Array staffinflist { get; set; }
        public Array stuinflist { get; set; }
        public long UserId { get; set; }
        public Array loaddata { get; set; }
        public Array get_Detail { get; set; }
        public Array Editlist { get; set; }
        public Array classlist { get; set; }
        public Array sectionlist { get; set; }
        public Array list1 { get; set; }
        public Array list { get; set; }
        public bool duplicate { get; set; }
        public string msg { get; set; }
        public string HRMRM_RoomNo { get; set; }
        public string HLMH_Name { get; set; }
        public string ASMC_SectionName { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string HRMDES_DesignationName { get; set; }

        public string AMST_AdmNo { get; set; }
        public string studentname { get; set; }
        public string staffname { get; set; }
        public Array studentdata { get; set; }
        public Array staffdata { get; set; }
        public Array guestdata { get; set; }

        public string type { get; set; }

        //staff
        public long HLHSTALT_Id { get; set; }
        //public long MI_Id { get; set; }
        public DateTime HLHSTALT_AllotmentDate { get; set; }
        //public long HLMH_Id { get; set; }
        //public long HLMRCA_Id { get; set; }
        public long? HRME_Id { get; set; }
        //public long HRMRM_Id { get; set; }
        public long HLHSTALT_NoOfBeds { get; set; }
        public string HLHSTALT_AllotRemarks { get; set; }
        public bool HLHSTALT_VacateFlg { get; set; }
        public DateTime? HLHSTALT_VacatedDate { get; set; }
        public string HLHSTALT_VacateRemarks { get; set; }
        public string HLHGSTALT_VacateRemarks { get; set; }
        public bool HLHSTALT_ActiveFlag { get; set; }
        public DateTime? HLHSTALT_CreatedDate { get; set; }
        public DateTime? HLHSTALT_UpdatedDate { get; set; }
        public DateTime? HLHGSTALT_AllotmentDate { get; set; }
        public DateTime? HLHGSTALT_VacatedDate { get; set; }
        public long HLHSTALT_UpdatedBy { get; set; }
        public long HLHSTALT_CreatedBy { get; set; }
        public Array gridlistdata { get; set; }
        public Array namelist { get; set; }
        public string HLHGSTALT_GuestName { get; set; }
        public long HLHGSTALT_Id { get; set; }
        public Array guest_details { get; set; }
    }
}
