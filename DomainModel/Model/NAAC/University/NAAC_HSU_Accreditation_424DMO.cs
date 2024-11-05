using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.University
{
    [Table("NAAC_HSU_Accreditation_424")]
    public class NAAC_HSU_Accreditation_424DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public long NCHSUA424_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCHSUA424_Year { get; set; }
        public bool NCHSUA424_NabhAcrFlag { get; set; }
        public bool NCHSUA424_NablAcrFlag { get; set; }
        public bool NCHSUA424_IntAcrFlag { get; set; }
        public bool NCHSUA424_ISOCertFlag { get; set; }
        public bool NCHSUA424_GplorGcplFlag { get; set; }
        public long NCHSUA424_CreatedBy { get; set; }
        public long NCHSUA424_UpdatedBy { get; set; }
        public DateTime NCHSUA424_CreatedDate { get; set; }
        public DateTime NCHSUA424_UpdatedDate { get; set; }

    }
}
