using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.SeatingArrangment
{
    public class SA_Exam_TitetableDTO
    {
        public long ESAETT_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public int EME_Id { get; set; }
        public long ESAUE_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public bool ESAETT_ActiveFlg { get; set; }
        public DateTime? ESAETT_CreatedDate { get; set; }
        public DateTime? ESAETT_UpdatedDate { get; set; }
        public long ESAETT_CreatedBy { get; set; }
        public long ESAETT_UpdatedBy { get; set; }
        public DateTime ESAETT_FromDate { get; set; }
        public DateTime ESAETT_ToDate { get; set; }
        public string message { get; set; }

        public Array yearlst { get; set; }
        public Array examlist { get; set; }
        public Array university_examlist { get; set; }
        public Array branchlist { get; set; }
        public Array semesterlist { get; set; }
        public Array courselist { get; set; }
        public Array satimetablelist { get; set; }
        public Array edit_tt_list { get; set; }
        public Array edit_tt_details{ get; set; }
        public Array examslotlist { get; set; }
        public Array subjectschemalist { get; set; }
        public Array subjectlist { get; set; }
        public Array view_tt_details { get; set; }
        public examdetailsarray1[] examdetailsarray { get; set; }
     

        public class examdetailsarray1
        {
            public long ESAESLOT_Id { get; set; }
            public long ACSS_Id { get; set; }
            public long ISMS_Id { get; set; }
            public DateTime ESAETT_ExamDate { get; set; }
        }
        
    }
}
