using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("IVRM_Virtual_School")]
    public class VirtaulSchool : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRM_Virtual_School_Id { get; set; }
        public long IVRM_MO_Id { get; set; }
        public long IVRM_MI_Id { get; set; }

        public string IVRM_Sub_Domain_Name { get; set; }

        public string ivrm_school_code { get; set; }
        public string IVRM_OTP_ADMNO { get; set; }

        public long? IVRM_CreatedBy { get; set; }
        public long? IVRM_UpdatedBy { get; set; }

    }
}
