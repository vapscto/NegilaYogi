using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.OnlineExam
{
    [Table("PA_Master_OE_Setting")]
    public class PA_Master_OE_SettingDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PAMOES_Id { get; set; }
        public long MI_Id { get; set; }
        public long PAMOES_NoofQns { get; set; }
        public decimal PAMOES_TotalMarks { get; set; }
        public string PAMOES_TotalDuration { get; set; }
        public decimal PAMOES_EachQnsMarks { get; set; }
        public string PAMOES_EachQnsDuration { get; set; }
        public long PAMOES_NoOfOptions { get; set; }

    }
}
