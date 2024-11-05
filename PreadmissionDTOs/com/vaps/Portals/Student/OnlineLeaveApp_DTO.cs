using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Portals.Student
{
   public class OnlineLeaveApp_DTO:CommonParamDTO
    {

        public long ASLA_Id { get; set; }
        public string ASLA_Reason { get; set; }
        public int ASLA_LeaveId { get; set; }
        public DateTime ASLA_ApplyDate { get; set; }
        public DateTime ASLA_FromDate { get; set; }
        public DateTime ASLA_ToDate { get; set; }
        public string ASLA_Status { get; set; }
        public string ASLA_Flag { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public DateTime? ASLA_ApprovedFromDate { get; set; }
        public DateTime? ASLA_ApprovedToDate { get; set; }
        public bool ASLA_ActiveFlag { get; set; }
        public Array student_s_list { get; set; }
        public Array section_s_list { get; set; }
        public Array class_s_list { get; set; }
        public Array student_leave_list { get; set; }
        public student_id_list1[] student_id_list { get; set; }

        public long ASLAP_Id { get; set; }
        public DateTime? ASLAP_AppRejDate { get; set; }
        public DateTime? ASLAP_AppFromDate { get; set; }
        public DateTime? ASLAP_AppToDate { get; set; }
        public string ASLAP_LeaveStatus { get; set; }
        public string ASALP_RejectReason { get; set; }
        public long? IVRMALU_Id { get; set; }
    

        public long ASAP_Id { get; set; }
        public string ASAP_ApprovalBy { get; set; }
        public bool ASAP_ActiveFlg { get; set; }
        public DateTime? ASAP_ApprovalDate { get; set; }


        public long ASAPCS_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public bool ASAPCS_ActiveFlg { get; set; }



        public string AMST_FirstName { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string AMST_RegistrationNo { get; set; }
        public string AMST_emailId { get; set; }
        public long AMST_MobileNo { get; set; }
        public Array studentdetails { get; set; }
        public string roletype { get; set; }
        public string flag { get; set; }
        public long UserId { get; set; }
        public long IVRMRT_Id { get; set; }
        public string flag_Type { get; set; }
        public bool returnval { get; set; }
        public Array allstuddata { get; set; }
        public Array editlist { get; set; }
        public Array pendingleave { get; set; }
        public long HRME_Id { get; set; }
        public bool duplicate { get; set; }

        public class student_id_list1
        {
            public long AMST_Id { get; set; }
        }

    }
}
