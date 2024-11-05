using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_7114_HumanValues_Files")]
    public class NAAC_AC_7114_HumanValues_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC7114HUVALF_Id { get; set; }
        public long NCAC7114HUVAL_Id { get; set; }
        public string NCAC7114HUVALF_FileName { get; set; }
        public string NCAC7114HUVALF_Filedesc { get; set; }
        public string NCAC7114HUVALF_FilePath { get; set; }
        public string NCAC7114HUVALF_StatusFlg { get; set; }
        public bool NCAC7114HUVALF_ActiveFlg { get; set; }
    }
}
