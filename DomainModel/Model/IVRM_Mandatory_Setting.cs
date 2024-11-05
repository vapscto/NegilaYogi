using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("IVRM_Mandatory_Setting")]
    public class IVRM_Mandatory_Setting:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMMS_Id { get; set; }
        public long IVRMP_Id { get; set; }
        public string IVRMMS_FieldName { get; set; }

        public string IVRMMS_Ngmodel { get; set; }

        public bool? IVRMMS_MandatoryFlag { get; set; }

    }
}
