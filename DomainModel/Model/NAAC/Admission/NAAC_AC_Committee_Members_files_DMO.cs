using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_Committee_Members_files")]
    public class NAAC_AC_Committee_Members_files_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCACCOMMMF_Id { get; set; }
        public long NCACCOMMM_Id { get; set; }
        public string NCACCOMMMF_FileName { get; set; }
        public string NCACCOMMMF_FileDesc { get; set; }
        public string NCACCOMMMF_FilePath { get; set; }
        public string NCACCOMMMF_StatusFlg { get; set; }
        public bool NCACCOMMMF_ActiveFlg { get; set; }
    }
}
