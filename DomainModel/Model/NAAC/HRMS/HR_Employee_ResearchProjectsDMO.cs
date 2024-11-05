using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.HRMS
{
    [Table("HR_Employee_ResearchProjects")]
    public class HR_Employee_ResearchProjectsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HREREPR_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HREREPR_ProjectTitle { get; set; }
        public string HREREPR_FundsReceived { get; set; }
        public string HREREPR_FundingAgency { get; set; }
        public long HREREPR_Period { get; set; }
        public string HREREPR_Role { get; set; }
        public string HREREPR_ProjectStatus { get; set; }
        public long HREREPR_Year { get; set; }
        public string HREREPR_Document { get; set; }
        public bool HREREPR_ActiveFlg { get; set; }
        public long HREREPR_CreatedBy { get; set; }
        public long HREREPR_UpdatedBy { get; set; }
        //public DateTime? CreatedDate { get; set; }
        //public DateTime? UpdatedDate { get; set; }
        public decimal HREREPR_SanctionedAmount { get; set; }
        public string HREREPR_TypeFlg { get; set; }
        public string HREREPR_DocumentPath { get; set; }
        public string HREREPR_PInvestigatorId { get; set; }
    }
}
