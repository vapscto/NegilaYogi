using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Dental
{
    [Table("NAAC_DC_816_SpecializedClinics")]
    public class DC_816_SpecializedClinicsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCDCSC816_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCDCSC816_Year { get; set; }
        public bool NCDCSC816_ComprehensiveclinicFlag { get; set; }
        public bool NCDCSC816_ImplantClinicFlag { get; set; }
        public bool NCDCSC816_GeriatricClinicFlag { get; set; }
        public bool NCDCSC816_SpecialHealthCareNeedsClinicFlag { get; set; }
        public bool NCDCSC816_TobaccoCessationClinicFlag { get; set; }
        public bool NCDCSC816_EstheticClinicFlag { get; set; }
        public bool NCDCSC816_ActiveFlag { get; set; }
        public long NCDCSC816_CreatedBy { get; set; }
        public long NCDCSC816_UpdatedBy { get; set; }
        public DateTime? NCDCSC816_CreatedDate { get; set; }
        public DateTime? NCDCSC816_UpdatedDate { get; set; }
    }
}
