using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_MC_715_WaterConservFacilities")]
    public class NAAC_MC_715_WaterConservFacilitiesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMC715WCF_Id { get; set; }
        public long MI_Id { get; set; }
        public string NCMC715WCF_RainWaterHarvestingFlag { get; set; }
        public string NCMC715WCF_BorewellOpenwellRecFlag { get; set; }
        public string NCMC715WCF_ConstructionOftanksbundsFlag { get; set; }
        public string NCMC715WCF_MaintenanceOfWaterbodiesDSFlag { get; set; }
        public string NCMC715WCF_WastewaterrecyclingFlag { get; set; }
        public long NCMC715WCF_CreatedBy { get; set; }
        public long NCMC715WCF_UpdatedBy { get; set; }
        public DateTime NCMC715WCF_CreatedDate { get; set; }
        public DateTime NCMC715WCF_UpdatedDate { get; set; }
        public long NCMC715WCF_Year { get; set; }
        public bool NCMC715WCF_ActiveFlg { get; set; }
        public string NCMC715WCF_StatusFlg { get; set; }
    }
}
