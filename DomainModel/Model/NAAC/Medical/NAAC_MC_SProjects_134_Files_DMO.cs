using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Medical
{
    [Table("NAAC_MC_SProjects_134_Files")]
    public class NAAC_MC_SProjects_134_Files_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMCSP134F_Id { get; set; }
        public string NCMCSP134F_FileName { get; set; }
        public string NCMCSP134F_Filedesc { get; set; }
        public string NCMCSP134F_FilePath { get; set; }
        public long NCMCSP134_Id { get; set; }
        public bool? NCMCSP134F_ActiveFlg { get; set; }
        public string NCMCSP134F_StatusFlg { get; set; }
        public string NCMCSP134F_Remarks { get; set; }
        public bool? NCMCSP134F_ApprovedFlg { get; set; }
    }
}
