using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_MC_713_AlternateEnergy")]
    public class NAAC_MC_713_AlternateEnergyDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMC713ALTENE_Id { get; set; }
        public long MI_Id { get; set; }
        public string NCMC713ALTENE_SolarenergyFlag { get; set; }
        public string NCMC713ALTENE_WindenergyFlag { get; set; }
        public string NCMC713ALTENE_SensorbasedEnergyFlag { get; set; }
        public string NCMC713ALTENE_BiogasPlantFlag { get; set; }
        public string NCMC713ALTENE_LEDbulbsORPowerefficEquipFlag { get; set; }
        public long NCMC713ALTENE_CreatedBy { get; set; }
        public long NCMC713ALTENE_UpdatedBy { get; set; }
        public DateTime NCMC713ALTENE_CreatedDate { get; set; }
        public DateTime NCMC713ALTENE_UpdatedDate { get; set; }
        public long NCMC713ALTENE_Year { get; set; }
        public bool NCMC713ALTENE_ActiveFlg { get; set; }
    }
}
