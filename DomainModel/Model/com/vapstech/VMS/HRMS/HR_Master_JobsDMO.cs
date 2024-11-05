using DomainModel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.VMS.HRMS
{
    [Table("HR_Master_Jobs")]
    public class HR_Master_JobsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMJ_Id { get; set; }
        public string HRMJ_JobCode { get; set; }
        public string HRMJ_JobTiTle { get; set; }
        public long HRMLO_Id { get; set; }
        public string HRMJ_Posted { get; set; }
        public long HRC_Id { get; set; }
        public bool HRMJ_ActiveFlg { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
    }

}