using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Adm_School_M_Class_Category")]
    public class Masterclasscategory : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ASMCC_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMC_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public bool Is_Active { get; set; }
        public List<AdmSchoolMasterClassCatSec> AdmSchoolMasterClassCatSec { get; set; }
    }
}
