using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_423_Memberships_Files")]
    public class NAAC_AC_423_Memberships_Files_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC423MEMF_Id { get; set; }
        public string NCAC423MEMF_FileName { get; set; }
        public string NCAC423MEMF_Filedesc { get; set; }
        public string NCAC423MEMF_FilePath { get; set; }
        public long NCAC423MEM_Id { get; set; }
        public string NCAC423MEMF_StatusFlg { get; set; }
        public bool NCAC423MEMF_ActiveFlg { get; set; }
    }
}
