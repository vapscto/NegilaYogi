using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_344_ExtnActivities_Staff_Files")]
    public class NAAC_AC_344_ExtnActivities_Staff_Files_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCACET344STFF_Id { get; set; }
        public string NCACET344STFF_FileName { get; set; }
        public string NCACET344STFF_Filedesc { get; set; }
        public string NCACET344STFF_FilePath { get; set; }
        public long NCACET344STF_Id { get; set; }
        public string NCACET344STFF_StatusFlg { get; set; }
        public bool NCACET344STFF_ActiveFlg { get; set; }

    }
}
