using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.University
{
    [Table("NAAC_MC_342_HRIncentives")]
   public class MC_342_HRIncentivesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMCHRI342_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCMCHRI342_Year { get; set; }
        public bool NCMCHRI342_CareerAdvancement { get; set; }
        public bool NCMCHRI342_IncrementInSalary { get; set; }
        public bool NCMCHRI342_RecThroughWebsiteNotification { get; set; }
        public bool NCMCHRI342_CommnCertAndCashaward { get; set; }
        public long NCMCHRI342_CreatedBy { get; set; }
        public long NCMCHRI342_UpdatedBy { get; set; }
        public DateTime? NCMCHRI342_CreatedDate { get; set; }
        public DateTime? NCMCHRI342_UpdatedDate { get; set; }
        public bool NCMCHRI342_ActiveFlag { get; set; }
    }
}
