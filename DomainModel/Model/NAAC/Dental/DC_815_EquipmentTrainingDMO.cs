using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Dental
{
    [Table("NAAC_DC_815_Equipment")]
   public class DC_815_EquipmentTrainingDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCDCEQT815_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCDCEQT815_Year { get; set; }
        public bool NCDCEQT815_ConeBeamComputedTomogramFlag { get; set; }
        public bool NCDCEQT815_CAMFacilityFlag { get; set; }
        public bool NCDCEQT815_ImagingMorphomEtricSoftwaresFlag { get; set; }
        public bool NCDCEQT815_EndodonticMicroscopeFlag { get; set; }
        public bool NCDCEQT815_DentalLASERUnitFlag { get; set; }
        public bool NCDCEQT815_ExtendedApplicationLightBasedMicroscopyFlag { get; set; }
        public bool NCDCEQT815_ImmunOhistocHemicalSetupFlag { get; set; }
        public bool NCDCEQT815_ActiveFlag { get; set; }
        public long NCDCEQT815_CreatedBy { get; set; }
        public long NCDCEQT815_UpdatedBy { get; set; }
        public DateTime? NCDCEQT815_CreatedDate { get; set; }
        public DateTime? NCDCEQT815_UpdatedDate { get; set; }
    }
}
