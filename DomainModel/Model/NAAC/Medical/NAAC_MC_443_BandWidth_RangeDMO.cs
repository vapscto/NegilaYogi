using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Medical
{
    [Table("NAAC_MC_443_BandWidth_Range")]
    public class NAAC_MC_443_BandWidth_RangeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMC443BWR_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCMC443BWR_Year { get; set; }
        public string NCMC443BWR_Range { get; set; }
        public bool NCMC443BWR_OneOrMoreGBPS { get; set; }
        public bool NCMC443BWR_500MBPSTo1GBPS { get; set; }
        public bool NCMC443BWR_250MBPSTo500MBPS { get; set; }
        public bool NCMC443BWR_50MBPSTo250MBPS { get; set; }
        public bool NCMC443BWR_LessThan50MBPS { get; set; }
        public DateTime? NCMC443BWR_CreatedDate { get; set; }
        public DateTime? NCMC443BWR_UpdatedDate { get; set; }
        public long NCMC443BWR_CreatedBy { get; set; }
        public long NCMC443BWR_UpdatedBy { get; set; }

    }
}
