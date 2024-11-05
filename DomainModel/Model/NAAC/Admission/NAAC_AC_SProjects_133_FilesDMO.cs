using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_SProjects_133_Files")]
    public class NAAC_AC_SProjects_133_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCACSPR133F_Id { get; set; }
        public long NCACSPR133_Id { get; set; }
        public string NCACSPR133F_FileName { get; set; }
        public string NCACSPR133F_Filedesc { get; set; }
        public string NCACSPR133F_FilePath { get; set; }
        public string NCACSPR133F_StatusFlg { get; set; }
        public bool? NCACSPR133F_ActiveFlg { get; set; }
        public bool? NCACSPR133F_ApprovedFlg { get; set; }
        public string NCACSPR133F_Remarks { get; set; }
    }
}
