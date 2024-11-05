using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_MC_716_GreenCampusInitiatives")]
    public class NAAC_MC_716_GreenCampusInitiativesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMC716GCI_Id { get; set; }
        public long MI_Id { get; set; }
        public string NCMC716GCI_RestrictedentryOfAutomobilesFlag { get; set; }
        public string NCMC716GCI_BatterypoweredvehiclesFlag { get; set; }
        public string NCMC716GCI_PedestrianFriendlyPathwaysFlag { get; set; }
        public string NCMC716GCI_BanOnTheuseOfPlasticsFlag { get; set; }
        public string NCMC716GCI_LandscapingwithtreesplantsFlag { get; set; }
        public long NCMC716GCI_CreatedBy { get; set; }
        public long NCMC716GCI_UpdatedBy { get; set; }
        public DateTime NCMC716GCI_CreatedDate { get; set; }
        public DateTime NCMC716GCI_UpdatedDate { get; set; }
        public long NCMC716GCI_Year { get; set; }
        public bool NCMC716GCI_ActiveFlg { get; set; }
    }
}
