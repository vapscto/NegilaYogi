using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Group_Dept_Dessg")]
    public class HRGroupDeptDessgDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRGTDDS_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMGT_Id { get; set; }
        public long HRMD_Id { get; set; }
        public long HRMDES_Id { get; set; }
        public bool HRGTDDS_ActiveFlg { get; set; }
        public long HRGTDDS_CreatedBy { get; set; }
        public long HRGTDDS_UpdatedBy { get; set; }
        public DateTime HRGTDDS_CreatedDate { get; set; }
        public DateTime HRGTDDS_UpdatedDate { get; set; }
    }
}
