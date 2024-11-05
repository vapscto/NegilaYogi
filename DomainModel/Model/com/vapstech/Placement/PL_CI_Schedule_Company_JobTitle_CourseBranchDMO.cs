using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Placement
{
    [Table("PL_CI_Schedule_Company_JobTitle_CourseBranch")]
    public class PL_CI_Schedule_Company_JobTitle_CourseBranchDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public long PLCISCHCOMJTCB_Id { get; set; }
        public long PLCISCHCOMJT_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public string PLCISCHCOMJTCB_ApplicableSEM { get; set; }
        public bool PLCISCHCOMJTCB_ActiveFlag { get; set; }
        public DateTime? PLCISCHCOMJTCB_CreatedDate { get; set; }
        public long PLCISCHCOMJTCB_CreatedBy { get; set; }
        public DateTime? PLCISCHCOMJTCB_UpdatedDate { get; set; }  
        public long PLCISCHCOMJTCB_UpdatedBy { get; set; }







    }
}
