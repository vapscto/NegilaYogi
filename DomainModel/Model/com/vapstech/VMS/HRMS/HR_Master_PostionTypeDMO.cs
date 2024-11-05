using DomainModel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.VMS.HRMS
{
    [Table("HR_Master_PostionType")]
    public class HR_Master_PostionTypeDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMPT_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMPT_Name { get; set; }
        public string HRMPT_Desc { get; set; }
        public bool HRMPT_ActiveFlg { get; set; }
        public long HRMPT_CreatedBy { get; set; }
        public long HRMPT_UpdatedBy { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
    }

}