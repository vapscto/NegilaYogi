using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.University
{
    [Table("NAAC_MC_352_RevenueGenerated")]
  public  class HSU_352_RevenueGeneratedDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMCRG352_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCMCRG352_Year { get; set; }
        public string NCMCRG352_ConsultantName { get; set; }
        public string NCMCRG352_AdvisoryName { get; set; }
        public string NCMCRG352_ConsultingORSpnAgencyCD { get; set; }
        public decimal NCMCRG352_RevenueGeneratedAmount { get; set; }
        public long NCMCRG352_CreatedBy { get; set; }
        public long NCMCRG352_UpdatedBy { get; set; }
        public bool NCMCRG352_ActiveFlag { get; set; }
        public DateTime? NCMCRG352_CreatedDate { get; set; }
        public DateTime? NCMCRG352_UpdatedDate { get; set; }
        public List<HSU_352_RevenueGenerated_FilesDMO> HSU_352_RevenueGenerated_FilesDMO { get; set; }
    }
}
