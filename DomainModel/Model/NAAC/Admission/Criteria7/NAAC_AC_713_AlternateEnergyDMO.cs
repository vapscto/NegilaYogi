using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_713_AlternateEnergy")]
    public class NAAC_AC_713_AlternateEnergyDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC713ALTENE_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC713ALTENE_Year { get; set; }
        public string NCAC713ALTENE_PowerRequirements { get; set; }
        public long NCAC713ALTENE_TotalPowerReq { get; set; }
        public string NCAC713ALTENE_EnergySource { get; set; }
        public string NCAC713ALTENE_EnergyUsed { get; set; }
        public string NCAC713ALTENE_EnergySupplied { get; set; }
        public bool NCAC713ALTENE_ActiveFlg { get; set; }
        public long NCAC713ALTENE_CreatedBy { get; set; }
        public long NCAC713ALTENE_UpdatedBy { get; set; }
        public DateTime NCAC713ALTENE_CreatedDate { get; set; }
        public DateTime NCAC713ALTENE_UpdatedDate { get; set; }
    }
}
