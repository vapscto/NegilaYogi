using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.admission
{
    [Table("Adm_Master_FatherMobileNo")]
    public class Adm_M_Student_FatherMobileNo :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AMSTFMNO_Id { get; set; }
        public long MI_Id { get; set; }
        [ForeignKey("AMST_Id")]
        public long AMST_Id { get; set; }
        public long? AMST_FatherMobile_No { get; set; }
        public long? AMSTFMNO_CreatedBy { get; set; }
        public long? AMSTFMNO_UpdatedBy { get; set; }

        public string ACSTSMS_CoutryCode { get; set; }

    }
}
