using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.LeaveManagement
{
    [Table("HR_Master_Department")]
    public class HR_Master_Department_DMO :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMD_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public int? HRMD_Order { get; set; }
        public bool HRMD_ActiveFlag { get; set; }

    }
}
