using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.University
{
    [Table("NAAC_HSU_323_ResearchProjectsRatio")]
   public class HSU_323_ResearchProjectsRatioDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NC323RPR_Id { get; set; }
        public long MI_Id { get; set; }
        public long NC323RPR_Year { get; set; }
        public string NC323RPR_ProjName { get; set; }
        public string NC323RPR_PricipalName { get; set; }
        public string NC323RPR_AgencyName { get; set; }
        public string NC323RPR_Type { get; set; }
        public string NC323RPR_DeptName { get; set; }
        public decimal NC323RPR_FundProvided { get; set; }
        public string NC323RPR_ProjDuration { get; set; }
        public bool NC323RPR_ActiveFlag { get; set; }
        public long NC323RPR_CreatedBy { get; set; }
        public long NC323RPR_UpdatedBy { get; set; }
        public DateTime? NC323RPR_CreatedDate { get; set; }
        public DateTime? NC323RPR_UpdatedDate { get; set; }
        public List<HSU_323_ResearchProjectsRatio_FilesDMO> HSU_323_ResearchProjectsRatio_FilesDMO { get; set; }
    }
}
