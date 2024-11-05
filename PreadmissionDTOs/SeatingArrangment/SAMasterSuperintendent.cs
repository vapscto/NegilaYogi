using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.SeatingArrangment
{
    public class SAMasterSuperintendent
    {
        public long ESASINTDNT_Id { get; set; }
        public long ESAMALSTU_Id { get; set; }
        public long ESACHCRD_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public long ASMAY_Id { get; set; }
        public int EME_Id { get; set; }
        public long ESAUE_Id { get; set; }
        public string message { get; set; }
        public bool returnval { get; set; }
        public string ASMAY_Year { get; set; }
        public string EME_IVRSExamName { get; set; }
        public string ESAUE_ExamName { get; set; }
        public string ESASINTDNT_ChiefSupName { get; set; }
        public string ESASINTDNT_ChiefSupCollege { get; set; }
        public string ESASINTDNT_DeptChiefSupName { get; set; }
        public string ESASINTDNT_DeptChiefSupCollege { get; set; }
        public string AMCST_FirstName { get; set; }
        public string AMCST_MiddleName { get; set; }
        public string AMCST_LastName { get; set; }
        public DateTime ESASINTDNT_FromDate { get; set; }
        public DateTime ESASINTDNT_ToDate { get; set; }
        public bool ESASINTDNT_ActiveFlg { get; set; }
        public bool ESAMALSTU_ActiveFlg { get; set; }
        public DateTime ESASINTDNT_CreatedDate { get; set; }
        public DateTime ESASINTDNT_UpdatedDate { get; set; }
        public long ESASINTDNT_CreatedBy { get; set; }
        public long ESASINTDNT_UpdatedBy { get; set; }
        public long ESAABSTU_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long ESAROOM_Id { get; set; }
        public long ESAESLOT_Id { get; set; }
        public string ESAABSTU_LABTheoryFlg { get; set; }
        public string ESAMALSTU_LABTHEORYFlg { get; set; }
        public string ESAMALSTU_StudentUSN { get; set; }
        public string ESAABSTU_StudentUSN { get; set; }
        public string ISMS_SubjectName { get; set; }
        public DateTime ESAABSTU_ExamDate { get; set; }
        public DateTime ESAMALSTU_ExamDate { get; set; }
        public bool ESAABSTU_ActiveFlg { get; set; }
        public string ESACHCRD_ChiefCordName { get; set; }
        public string ESACHCRD_Add1 { get; set; }
        public string ESACHCRD_Add2 { get; set; }
        public string ESACHCRD_Add3 { get; set; }
        public string ESACHCRD_Add4 { get; set; }
        public string ESACHCRD_Add5 { get; set; }
        public string ESACHCRD_Add6 { get; set; }
        public string Flag { get; set; }
        public bool ESACHCRD_ActiveFlg { get; set; }
        public long ISMS_OrderFlag { get; set; }

        public Array superintendentgridlist { get; set; }
        public Array yearlst { get; set; }
        public Array examlist { get; set; }
        public Array university_examlist { get; set; }
        public Array edit_suplist { get; set; }
        public Array edit_aslist { get; set; }
        public Array edit_mpslist { get; set; }
        public Array edit_cclist { get; set; }
        public Array courselist { get; set; }
        public Array branchlist { get; set; }
        public Array semesterlist { get; set; }
        public Array subjectlist { get; set; }
        public Array roomlist { get; set; }
        public Array slotlist { get; set; }
        public Array studentlist { get; set; }
        public Array absentstudentlist { get; set; }
        public Array malpracticestudentlist { get; set; }
        public Array chiefcoordinatorlist { get; set; }
        
    }
}
