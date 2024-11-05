using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_719_DifferentlyAbled_Files")]
    public class NAAC_AC_719_DifferentlyAbled_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC719DIFFABF_Id { get; set; }
        public long NCAC719DIFFAB_Id { get; set; }
        public string NCAC719DIFFABF_FileName { get; set; }
        public string NCAC719DIFFABF_Filedesc { get; set; }
        public string NCAC719DIFFABF_FilePath { get; set; }
    }
}
