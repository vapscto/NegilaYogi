using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Master_Quarter_Month")]
    public class HR_Master_Quarter_Month
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long HRMQM_Id { get; set; }
        public long HRMQ_Id { get; set; }
        //public long IVRM_Month { get; set; }
        public bool HRMQM_ActiveFlg { get; set; }
        public long HRMQM_CreatedBy { get; set; }
        public long HRMQM_UpdatedBy { get; set; }

    }
}
