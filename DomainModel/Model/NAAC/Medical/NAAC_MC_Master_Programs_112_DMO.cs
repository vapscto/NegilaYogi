using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Medical
{
    [Table("NAAC_MC_Master_Programs_112")]
    public class NAAC_MC_Master_Programs_112_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMCMPR112_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCMCMPR112_Year { get; set; }
        public long NCMCMPR112_NoOfTeachersPartBos { get; set; }
        public long NCMCMPR112_NoOfTeachersAcu { get; set; }
        public DateTime NCMCMPR112_CreatedDate { get; set; }
        public DateTime NCMCMPR112_UpdatedDate { get; set; }
        public long NCMCMPR112_CreatedBy { get; set; }
        public long NCMCMPR112_UpdatedBy { get; set; }
        public bool NCMCMPR112_ActiveFlag { get; set; }

        public List<NAAC_MC_Master_Programs_112_Details_DMO> NAAC_MC_Master_Programs_112_Details_DMO { get; set; }
        public List<NAAC_MC_Master_Programs_112_Files_DMO> NAAC_MC_Master_Programs_112_Files_DMO { get; set; }

    }
}
