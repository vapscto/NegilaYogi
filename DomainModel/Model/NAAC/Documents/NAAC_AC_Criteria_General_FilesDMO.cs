using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.NAAC.Documents
{
    [Table("NAAC_AC_Criteria_General_AddFiles")]
    public class NAAC_AC_Criteria_General_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public long  NCACCRGENF_Id { get; set; }
        public long NCACCRGEN_Id { get; set; }
        public string NCACCRGENAF_AdditionalFileDesc { get; set; }
        public string NCACCRGENAF_FileName { get; set; }
        public string NCACCRGENAF_FilePath { get; set; }
        public bool NCACCRGENAF_ActiveFlg { get; set; }
        public long NCACCRGENAF_CreatedBy { get; set; }
        public long NCACCRGENAF_UpdatedBy { get; set; }
        public DateTime NCACCRGENAF_CreatedDate { get; set; }
        public DateTime NCACCRGENAF_UpdatedDate { get; set; }
    }
}
