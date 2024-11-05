using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_413_ICT_Files")]

   public class NAAC_AC_413_ICT_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC413ICTF_Id { get; set; }
        public long NCAC413ICT_Id { get; set; }
        public string NCAC413ICTF_FileName { get; set;}
        public string NCAC413ICTF_FilePath { get; set; }
        public string NCAC413ICTF_Filedesc { get; set; }
        public string NCAC413ICTF_StatusFlg { get; set; }
        public bool NCAC413ICTF_ActiveFlg { get; set; }
    }

}
