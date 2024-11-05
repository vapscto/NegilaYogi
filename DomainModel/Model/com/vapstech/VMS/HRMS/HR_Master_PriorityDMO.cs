using DomainModel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.VMS.HRMS
{
    [Table("HR_Master_Priority")]
    public class HR_Master_PriorityDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMPR_Id { get; set; }
        public string HRMP_Name { get; set; }
        public int HRMP_Order { get; set; }
        public bool HRMP_ActiveFlag { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
        public long MI_Id { get; set; }
        public long HRMP_CreatedBy { get; set; }
        public long HRMP_UpdatedBy { get; set; }
    }

}