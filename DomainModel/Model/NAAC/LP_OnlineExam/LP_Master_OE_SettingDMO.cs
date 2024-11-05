using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.NAAC.LP_OnlineExam
{
    [Table("LP_Master_OE_Setting")]
    public class LP_Master_OE_SettingDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long LPMOES_Id { get; set; }
        public long MI_Id { get; set; }
        public long LPMOES_NoofQns { get; set; }
        public decimal? LPMOES_TotalMarks { get; set; }
        public string LPMOES_TotalDuration { get; set; }
        public decimal? LPMOES_EachQnsMarks { get; set; }
        public string LPMOES_EachQnsDuration { get; set; }
        public long LPMOES_NoOfOptions { get; set; }
        public bool LPMOES_ActiveFlg { get; set; }
        public long LPMOES_CreatedBy { get; set; }
        public long LPMOES_UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
