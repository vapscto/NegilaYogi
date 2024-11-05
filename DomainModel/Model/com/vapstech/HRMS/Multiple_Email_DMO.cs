using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Master_Employee_EmailId")]
    public class Multiple_Email_DMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMEEM_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HRMEM_EmailId { get; set; }
        public string HRMEM_DeFaultFlag { get; set; }
    }
}
