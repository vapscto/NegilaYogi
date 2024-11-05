using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_515_VET_Files")]
    public class NAAC_AC_515_VETFilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC515VETF_Id { get; set; }
        public long NCAC515VET_Id { get; set; }
        public string NCAC515VETF_FileName { get; set; }
        public string NCAC515VETF_Filedesc { get; set; }
        public string NCAC515VETF_FilePath { get; set; }
        public string NCAC515VETF_StatusFlg { get; set; }
        public bool? NCAC515VETF_ActiveFlg { get; set; }
        
 

    }
}
