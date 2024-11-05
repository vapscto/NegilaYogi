using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.FeedBack
{
    [Table("Feedback_Master_Type")]
    public class FeedBackMasterTypeDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMTY_Id { get; set; }
        public long MI_Id { get; set; }
        public string FMTY_FeedbackTypeName { get; set; }
        public string FMTY_FeedbackTypeRemarks { get; set; }
        public int FMTY_FTOrder { get; set; }
        public string FMTY_StakeHolderFlag { get; set; }
        public bool FMTY_SubjectSpecificFlag { get; set; }
        public bool FMTY_ActiveFlag { get; set; }
        public long FMTY_CreatedBy { get; set; }
        public long FMTY_UpdatedBy { get; set; }
        public bool FMTY_QuestionwiseOptionFlg { get; set; }
        public bool? FMTY_StudentFlag { get; set; }
        public long? FMTY_NOFPerYearByStudent { get; set; }
        public bool? FMTY_StaffFlag { get; set; }
        public long? FMTY_NOFPerYearByStaff { get; set; }
        public bool? FMTY_ParentFlag { get; set; }
        public long? FMTY_NOFPerYearByParent { get; set; }
        public bool? FMTY_AlumniFlag { get; set; }
        public long? FMTY_NOFPerYearByAlumni { get; set; }
    }
}
