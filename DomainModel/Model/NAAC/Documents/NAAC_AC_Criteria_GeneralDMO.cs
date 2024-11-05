using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.NAAC.Documents
{
    [Table("NAAC_AC_Criteria_General")]
    public class NAAC_AC_Criteria_GeneralDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public long NCACCRGEN_Id { get; set; }
        public long  MI_Id { get; set; }
        public long MT_Id { get; set; }
        public long  NAACSL_Id { get; set; }
        public string  NCACCRGEN_CriteriaDescription { get; set; }
        public bool  NCACCRGEN_ActiveFlg { get; set; }
        public long  NCACCRGEN_CreatedBy { get; set; }
        public long  NCACCRGEN_UpdatedBy { get; set; }
        public DateTime  NCACCRGEN_CreatedDate { get; set; }
        public DateTime NCACCRGEN_UpdatedDate { get; set; }
        public List<NAAC_AC_Criteria_General_FilesDMO> NAAC_AC_Criteria_General_FilesDMO { get; set; }
        public List<NAAC_AC_Criteria_General_LinkDMO> NAAC_AC_Criteria_General_LinkDMO { get; set; }
    }
}
