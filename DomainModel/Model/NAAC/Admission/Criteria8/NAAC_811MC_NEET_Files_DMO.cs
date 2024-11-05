using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission.Criteria8
{
    [Table("NAAC_MC_811_NEET_Files")]
    public class NAAC_811MC_NEET_Files_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMC811NEETF_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCMC811NEET_Id { get; set; }
        public string NCMC811NEETF_FileName { get; set; }
        public string NCMC811NEETF_FilePath { get; set; }
        public string NCMC811NEETF_FileDesc { get; set; }
        public string NCMC811NEETF_StatusFlg { get; set; }
        public bool NCMC811NEETF_ActiveFlg { get; set; }
        public long NCMC811NEETF_CreatedBy { get; set; }
        public long NCMC811NEETF_UpdatedBy { get; set; }
        public DateTime NCMC811NEETF_CreatedDate { get; set; }
        public DateTime NCMC811NEETF_UpdatedDate { get; set; }

    }
}
