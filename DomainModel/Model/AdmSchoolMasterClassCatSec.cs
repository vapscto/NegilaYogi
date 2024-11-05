using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model
{
    [Table("Adm_School_Master_Class_Cat_Sec")]
    public class AdmSchoolMasterClassCatSec
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ASMCCS_Id { get; set; }
        public long ASMCC_Id { get; set; }
        public long ASMS_Id { get; set; }
        public bool ASMCCS_ActiveFlg { get; set; }
    }
}
