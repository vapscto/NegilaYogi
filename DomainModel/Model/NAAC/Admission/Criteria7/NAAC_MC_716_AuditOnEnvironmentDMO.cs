using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_MC_716_AuditOnEnvironment")]
    public class NAAC_MC_716_AuditOnEnvironmentDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMC716AOE_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCMC716AOE_Year { get; set; }
        public string NCMC716AOE_GreenauditFlag { get; set; }
        public string NCMC716AOE_EnergyAuditFlag { get; set; }
        public string NCMC716AOE_EnvironmentAuditFlag { get; set; }
        public string NCMC716AOE_CleanandgreenCampusRecognitionsFlag { get; set; }
        public bool NCMC716AOE_ActiveFlag { get; set; }
        public long NCMC716AOE_CreatedBy { get; set; }
        public long NCMC716AOE_UpdatedBy { get; set; }
        public DateTime NCMC716AOE_CreatedDate { get; set; }
        public DateTime NCMC716AOE_UpdatedDate { get; set; }
    }
}
