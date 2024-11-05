using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.HRMS
{
    [Table("HR_Employee_ResearchGuidance")]
    public class HR_Employee_ResearchGuidanceDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HREREGU_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HREREGU_ThesisTitle { get; set; }
        public string HREREGU_Course { get; set; }
        public string HREREGU_ThesisStatus { get; set; }
        public long HREREGU_Year { get; set; }
        public string HREREGU_Document { get; set; }
        public bool HREREGU_ActiveFlg { get; set; }
        public long HREREGU_CreatedBy { get; set; }
        public long HREREGU_UpdatedBy { get; set; }
        public string HREREGU_StudentName { get; set; }
        //public DateTime? CreatedDate { get; set; }
        //public DateTime? UpdatedDate { get; set; }
        public long HREREGU_RegistraionYear { get; set; }
        public long HREREGU_AwardedYear { get; set; }
        public string HREREGU_DocumentPath { get; set; }
    }
}
