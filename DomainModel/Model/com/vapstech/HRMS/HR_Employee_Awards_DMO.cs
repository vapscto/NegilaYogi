using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Employee_Awards")]
    public class HR_Employee_Awards_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HREAW_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HREAW_AwardName { get; set; }
        public long HREAW_AwardYear { get; set; }
        public decimal? HREAW_IncentiveAmount { get; set; }
        public string HREAW_FileName { get; set; }
        public string HREAW_FilePath { get; set; }
        public bool HREAW_ActiveFlg { get; set; }
        public long HREAW_CreatedBy { get; set; }
        public long HREAW_UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string HREAW_LevelAwards { get; set; }
        public string HREAW_AgencyName { get; set; }


    }
}
