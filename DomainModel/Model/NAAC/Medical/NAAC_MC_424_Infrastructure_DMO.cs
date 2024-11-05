using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Medical
{
    [Table("NAAC_MC_424_Infrastructure")]
   public class NAAC_MC_424_Infrastructure_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long  NCMCI424_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCMCI424_Year { get; set; }
        public bool NCMCI424_AttchSatellitePrimaryHealthCenterFlag { get; set; }
        public bool NCMCI424_AttchRuralHealthCenterFlag { get; set; }
        public bool NCMCI424_ResFacilityForStudentsORtraineesFlag { get; set; }
        public long NCMCI424_CreatedBy { get; set; }
        public long NCMCI424_UpdatedBy { get; set; }
        public DateTime NCMCI424_CreateDate { get; set; }
        public DateTime NCMCI424_UpdatedDate { get; set; }
        public bool NCMC423ICBL_AttcurbanHCTrainingOfStudentsFlag { get; set; }
    }
}
