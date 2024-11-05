using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Master_Employee_Documents")]
    public class Master_Employee_Documents :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMEDS_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HRMEDS_DocumentName { get; set; }
        public string HRMEDS_DocumentImageName { get; set; }
        public string HRMEDS_DucumentDescription { get; set; }

    }
}
