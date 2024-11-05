using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Portals.IVRM;

namespace DomainModel.Model.com.vapstech.Portals.Employee
{
    [Table("IVRM_NoticeBoard")]
    public class IVRM_NoticeBoardDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INTB_Id { get; set; }
        public long MI_Id { get; set; }
        public string INTB_Title { get; set; }
        public string INTB_Description { get; set; }
        public string INTB_Attachment { get; set; }
        public string INTB_FilePath { get; set; }
        public DateTime? INTB_DisplayDate { get; set; }
        public DateTime INTB_StartDate { get; set; }
        public DateTime INTB_EndDate { get; set; }
        public bool INTB_ActiveFlag { get; set; }
        public long? INTB_CreatedBy { get; set; }
        public long? INTB_UpdatedBy { get; set; }
        public string NTB_TTSylabusFlg { get; set; }
        public bool INTB_DispalyDisableFlg { get; set; }
        public bool? INTB_ToStaffFlg { get; set; }
        public bool? INTB_ToStudentFlg { get; set; }

       
        public List<IVRM_NoticeBoard_Class_DMO> IVRM_NoticeBoard_Class_DMO { get; set; }
        public List<IVRM_NoticeBoard_CoBranchDMO> IVRM_NoticeBoard_CoBranchDMO { get; set; }

        public List<IVRM_NoticeBoard_Student_DMO> IVRM_NoticeBoard_Student_DMO { get; set; }
        public List<IVRM_NoticeBoard_Student_CollegeDMO> IVRM_NoticeBoard_Student_CollegeDMO { get; set; }
    }
}
