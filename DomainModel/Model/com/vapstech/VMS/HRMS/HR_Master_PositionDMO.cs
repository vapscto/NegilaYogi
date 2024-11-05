using DomainModel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.VMS.HRMS
{
    [Table("HR_Master_Position")]
    public class HR_Master_PositionDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMP_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMP_Position { get; set; }
        public string HRMP_Skills { get; set; }
        public string HRMP_Desc { get; set; }
        public bool HRMP_ActiveFlg { get; set; }
        public long HRMP_CreatedBy { get; set; }
        public long HRMP_UpdatedBy { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
    }

}