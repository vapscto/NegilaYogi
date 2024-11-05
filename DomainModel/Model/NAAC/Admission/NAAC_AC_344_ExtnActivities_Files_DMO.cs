using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{

    [Table("NAAC_AC_344_ExtnActivities_Files")]
    public class NAAC_AC_344_ExtnActivities_Files_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCACET343F_Id { get; set; }
        public long NCACET343_Id { get; set; }
        public string NCACET343F_FileName { get; set; }
        public string NCACET343F_Filedesc { get; set; }
        public string NCACET343F_FilePath { get; set; }
        public string NCACET343F_StatusFlg { get; set; }
        public bool NCACET343F_ActiveFlg { get; set; }
    }
}
