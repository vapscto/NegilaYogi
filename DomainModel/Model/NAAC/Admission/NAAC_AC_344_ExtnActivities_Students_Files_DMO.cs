using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_344_ExtnActivities_Students_Files")]
    public class NAAC_AC_344_ExtnActivities_Students_Files_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCACET343SF_Id { get; set; }
        public long NCACET343S_Id { get; set; }
        public string NCACET343SF_FileName { get; set; }
        public string NCACET343SF_Filedesc { get; set; }
        public string NCACET343SF_FilePath { get; set; }
        public string NCACET343SF_StatusFlg { get; set; }
        public bool NCACET343SF_ActiveFlg { get; set; }
    }
}
