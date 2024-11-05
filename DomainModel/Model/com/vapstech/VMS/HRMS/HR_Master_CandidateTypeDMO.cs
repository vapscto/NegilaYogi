using DomainModel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.VMS.HRMS
{
    [Table("HR_Master_CandidateType")]
    public class HR_Master_CandidateTypeDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMCT_Id { get; set; }
        public long HRCL_Id { get; set; }
        public string HRMCT_Name { get; set; }
        public int HRMCT_Order { get; set; }
        public string HRMCT_Desc { get; set; }
        public bool HRMCT_ActiveFlg { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
    }

}