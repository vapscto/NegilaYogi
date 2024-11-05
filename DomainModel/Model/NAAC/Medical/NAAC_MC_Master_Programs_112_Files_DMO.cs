using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Medical
{
    [Table("NAAC_MC_Master_Programs_112_Files")]
    public class NAAC_MC_Master_Programs_112_Files_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMCMPR112DF_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCMCMPR112_Id { get; set; }
        public string NCMCMPR112DF_FileDesc { get; set; }
        public string NCMCMPR112DF_FileName { get; set; }
        public string NCMCMPR112DF_FilePath { get; set; }
        public bool NCMCMPR112DF_ActiveFlg { get; set; }
        public long NCMCMPR112DF_CreatedBy { get; set; }
        public long NCMCMPR112DF_UpdatedBy { get; set; }
        public DateTime NCMCMPR112DF_CreatedDate { get; set; }
        public DateTime NCMCMPR112DF_UpdatedDate { get; set; }

    }
}
