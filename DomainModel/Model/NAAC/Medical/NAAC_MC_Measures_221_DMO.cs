using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Medical
{
    [Table("NAAC_MC_Measures_221")]
    public class NAAC_MC_Measures_221_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMCM221_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCMCM221_Year { get; set; }
        public bool NCMCM221_MesCrFolldRegSlowPerFlag { get; set; }
        public bool NCMCM221_MesCrFolldAdLersFlag { get; set; }
        public bool NCMCM221_SpecialprogCrLowORAdlersFlag { get; set; }
        public bool NCMCM221_ProclsMeasureAchsFlag { get; set; }
        public long NCMCM221_CreatedBy { get; set; }
        public long NCMCM221_UpdatedBy { get; set; }
        public DateTime NCMCM221_CreatedDate { get; set; }
        public DateTime NCMCM221_UpdatedDate { get; set; }
    }
}
