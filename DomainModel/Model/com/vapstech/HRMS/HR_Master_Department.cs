using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Master_Department")]
    public class HR_Master_Department:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long HRMD_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public int? HRMD_Order { get; set; }
        public bool HRMD_ActiveFlag { get; set; }
        public long? HRMDC_ID { get; set; }
        public decimal? HRMD_InternalTrainingMinimumHrs { get; set; }
         

    }
}
