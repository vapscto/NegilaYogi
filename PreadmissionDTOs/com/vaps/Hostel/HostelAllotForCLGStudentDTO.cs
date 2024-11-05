using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Hostel
{
    public class HostelAllotForCLGStudentDTO
    {
        public long HLMH_Id { get; set; }
        public long MI_Id { get; set; }
        public string HLMH_Name { get; set; }
        public long ASMAY_Id { get; set; }
        public long UserId { get; set; }
        public long HLHSREQC_Id { get; set; }
        public string HRMRM_RoomNo { get; set; }
        public string HLHSREQC_Remarks { get; set; }
        public string ASMAY_Year { get; set; }
        public Array courselist { get; set; }
        public Array semisterlist { get; set; }
        public Array Sectionlist { get; set; }
        public Array branchlist { get; set; }
        public Array floor_list { get; set; }
        public Array floor_details { get; set; }
        public Array student_Requestlist { get; set; }
        public long HLMF_Id { get; set; }
        public string HRMF_FloorName { get; set; }
        public long AMCO_Id { get; set; }
        public string AMCO_CourseName { get; set; }
        public long AMSE_Id { get; set; }
        public string AMSE_SEMName { get; set; }
        public string ACMS_SectionName { get; set; }
        public long AMB_Id { get; set; }
        public string AMB_BranchName { get; set; }
        public string Type { get; set; }
        public long HLHSALTC_Id { get; set; }
        public long ACMS_Id { get; set; }
        public bool? HLHSALTC_ActiveFlag { get; set; }
        public bool? HLHSALTC_EntireRoomReqdFlg { get; set; }
        public long HRMRM_Id { get; set; }
        public long HRMRM_Id_Old { get; set; }
        public DateTime? HLHSALTC_AllotmentDate { get; set; }
        public long HLHSALTC_NoOfBeds { get; set; }
        public string HLHSALTC_AllotRemarks { get; set; }
        public bool? HLHSALTC_VacateFlg { get; set; }
        public DateTime? HLHSALTC_VacatedDate { get; set; }
        public string HLHSALTC_VacateRemarks { get; set; }
        public string allottype { get; set; }
        public Array housewise_studentList { get; set; }
        public Array hostel_list { get; set; }
        public Array yearlist { get; set; }
        public Array student_allotlist { get; set; }
        public Array roomcatgry_list { get; set; }
        public Array room_list { get; set; }
        public Array room_Details { get; set; }
        public Array editlist { get; set; }
        public Array courseLists { get; set; }
        public Array branchLists { get; set; }
        public Array semLists { get; set; }
        public Array secLists { get; set; }
        public Array studlist { get; set; }
        public string studentname { get; set; }
        
        public bool returnval { get; set; }
        public long HLMRCA_Id { get; set; }
        public long AMCST_Id { get; set; }
        public bool duplicate { get; set; }
        public long HRMRM_BedCapacity { get; set; }
        public bool bedcount { get; set; }
        public bool? HLHSREQC_VegMessReqdFlg { get; set; }
        public bool? HLHSREQC_NonVegMessReqdFlg { get; set; }
        public bool? HLHSREQC_EntireRoomReqdFlg { get; set; }
        public bool? HLHSREQC_ACRoomReqdFlg { get; set; }
        public DateTime? HLHSREQC_RequestDate { get; set; }
        public bool? HLHSREQCC_SingleRoomFlg { get; set; }
        public string HLHSREQCC_Remarks { get; set; }
        
        public string HLHSREQC_BookingStatus { get; set; }
        public bool? HLHSREQC_ActiveFlag { get; set; }
        public DateTime? HLHSREQC_CreatedDate { get; set; }
        public DateTime? HLHSREQC_UpdatedDate { get; set; }
        public long? HLHSREQC_CreatedBy { get; set; }
        public long? HLHSREQC_UpdatedBy { get; set; }
    
    }
}
