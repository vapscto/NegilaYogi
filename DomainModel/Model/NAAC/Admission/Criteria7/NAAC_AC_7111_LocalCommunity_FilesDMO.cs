using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_7111_LocalCommunity_Files")]
    public class NAAC_AC_7111_LocalCommunity_FilesDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC7111LOCCOMF_Id { get; set; }
        public long NCAC7111LOCCOM_Id { get; set; }
        public string NCAC7111LOCCOMF_Filedesc { get; set; }
        public string NCAC7111LOCCOMF_FileName { get; set; }
        public string NCAC7111LOCCOMF_FilePath { get; set; }
        public string NCAC7111LOCCOMF_StatusFlg { get; set; }
        public bool NCAC7111LOCCOMF_ActiveFlg { get; set; }

     
    }
}
