using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.OnlineExam
{
    [Table("LMS_Master_OE_Setting")]
    public class LMS_Master_OE_SettingDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long LMSMOES_Id { get; set; }

        public long MI_Id { get; set; }
        public long LMSMOES_NoofQns { get; set; }
        public decimal LMSMOES_TotalMarks { get; set; }
        public string LMSMOES_TotalDuration { get; set; }
        public decimal LMSMOES_EachQnsMarks { get; set; }
        public string LMSMOES_EachQnsDuration { get; set; }
        public long LMSMOES_NoOfOptions { get; set; }


    }
}
