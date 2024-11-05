using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Medical
{
    [Table("NAAC_MC_Master_Programs_112_Details")]
    public class NAAC_MC_Master_Programs_112_Details_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMCMPR112D_Id { get; set; }
        public long NCMCMPR112_Id { get; set; }
        public long HRME_Id { get; set; }
        public string NCMCMPR112D_PrgType { get; set; }
        public long NCMCMPR112D_CreatedBy { get; set; }
        public long NCMCMPR112D_UpdatedBy { get; set; }
        public DateTime NCMCMPR112D_CreatedDate { get; set; }
        public DateTime NCMCMPR112D_UpdatedDate { get; set; }

    }
}
