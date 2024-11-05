using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("IVRM_MandatorySetting_IW")]
    public class IVRM_Mandatory_Setting_IW : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMMSI_Id { get; set; }
        public long MI_Id { get; set; }
        public long IVRMP_Id { get; set; }
        public string IVRMMSI_FieldName { get; set; }

        public string IVRMMSI_Ngmodel { get; set; }
        public bool? IVRMMSI_MandatoryFlag { get; set; }



    }
}
