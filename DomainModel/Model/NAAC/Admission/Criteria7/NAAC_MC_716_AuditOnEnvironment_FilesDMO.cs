using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_MC_716_AuditOnEnvironment_Files")]
    public class NAAC_MC_716_AuditOnEnvironment_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMC716AOEF_Id { get; set; }
        public long NCMC716AOE_Id { get; set; }
        public string NCMC716AOEF_FileName { get; set; }
        public string NCMC716AOEF_Filedesc { get; set; }
        public string NCMC716AOEF_FilePath { get; set; }
    }
}
