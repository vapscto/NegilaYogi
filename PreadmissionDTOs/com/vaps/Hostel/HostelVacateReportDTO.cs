using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Hostel
{
    public class HostelVacateReportDTO
    {
        //student
        public long HLHSALT_Id { get; set; }
        public long MI_Id { get; set; }
        public DateTime? HLHSALT_AllotmentDate { get; set; }
        public long ASMAY_Id { get; set; }
        public long HLMH_Id { get; set; }
        public long HLMRCA_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long HRMRM_Id { get; set; }
        public long HLHSALT_NoOfBeds { get; set; }
        public string HLHSALT_AllotRemarks { get; set; }
        public bool HLHSALT_VacateFlg { get; set; }
        public DateTime? HLHSALT_VacatedDate { get; set; }
        public string HLHSALT_VacateRemarks { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string AMST_FirstName { get; set; }
        public bool HLHSALT_ActiveFlag { get; set; }
        public DateTime? HLHSALT_CreatedDate { get; set; }
        public DateTime? HLHSALT_UpdatedDate { get; set; }
        public long HLHSALT_UpdatedBy { get; set; }
        public long HLHSALT_CreatedBy { get; set; }
        public long UserId { get; set; }
        public Array loaddata { get; set; }
        public Array get_report { get; set; }
        public Array yerlist { get; set; }
        public Array studentlist { get; set; }
        public Array stafflist { get; set; }
        public Array hostelist { get; set; }
        public Array guestlist { get; set; }
        public string type { get; set; }
        public string type2 { get; set; }


        //staff
        public long HLHSTALT_Id { get; set; }
        public DateTime HLHSTALT_AllotmentDate { get; set; }
        public long HRME_Id { get; set; }
        public long HLHSTALT_NoOfBeds { get; set; }
        public string HLHSTALT_AllotRemarks { get; set; }
        public bool HLHSTALT_VacateFlg { get; set; }
        public DateTime? HLHSTALT_VacatedDate { get; set; }
        public string HLHSTALT_VacateRemarks { get; set; }
        public bool HLHSTALT_ActiveFlag { get; set; }
        public DateTime? HLHSTALT_CreatedDate { get; set; }
        public DateTime? HLHSTALT_UpdatedDate { get; set; }
        public long HLHSTALT_UpdatedBy { get; set; }
        public long HLHSTALT_CreatedBy { get; set; }

        //guest
        public long HLHGSTALT_Id { get; set; }
        public DateTime HLHGSTALT_AllotmentDate { get; set; }
        public string HLHGSTALT_GuestName { get; set; }
        public long HLHGSTALT_GuestPhoneNo { get; set; }
        public string HLHGSTALT_GuestEmailId { get; set; }
        public string HLHGSTALT_GuestAddress { get; set; }
        public string HLHGSTALT_GuestPhoto { get; set; }
        public string HLHGSTALT_AddressProof { get; set; }
        public long HLHGSTALT_NoOfBeds { get; set; }
        public string HLHGSTALT_AllotRemarks { get; set; }
        public bool HLHGSTALT_VacateFlg { get; set; }
        public DateTime? HLHGSTALT_VacatedDate { get; set; }
        public string HLHGSTALT_VacateRemarks { get; set; }
        public bool HLHGSTALT_ActiveFlag { get; set; }
        public DateTime? HLHGSTALT_CreatedDate { get; set; }
        public DateTime? HLHGSTALT_UpdatedDate { get; set; }
        public DateTime? Fromdate { get; set; }
        public DateTime? ToDate { get; set; }
        public long HLHGSTALT_UpdatedBy { get; set; }
        public long HLHGSTALT_CreatedBy { get; set; }
        public Array gridlistdata { get; set; }
        public Array Classlist { get; set; }
        public Array Sectionlist { get; set; }
        public Array departmentlist { get; set; }
        public Array designationlist { get; set; }
        public SelectedHostellist_DTO[] SelectedHostellist { get; set; }
        public Studentlist_DTO[] SelectedStudentlist { get; set; }
        public stafflist_DTO[] Selectedstafflist { get; set; }
        public guestlist_DTO[] Selectedguestlist { get; set; }
    }
    public class SelectedHostellist_DTO
    {
        public long HLMH_Id { get; set; }
    }
    public class Studentlist_DTO
    {
        public long AMST_Id { get; set; }
    }
    public class stafflist_DTO
    {
        public long HRME_Id { get; set; }
    }
    public class guestlist_DTO
    {
        public long HLHGSTALT_Id { get; set; }
    }
}
