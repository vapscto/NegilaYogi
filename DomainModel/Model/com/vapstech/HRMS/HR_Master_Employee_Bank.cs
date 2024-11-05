using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Master_Employee_Bank")]
    public class HR_Master_Employee_Bank :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMEB_Id { get; set; }
        public long HRME_Id { get; set; }
        public long HRMBD_Id { get; set; }
        public string HRMEB_AccountNo { get; set; }
        //public long HRMEB_AccountNo { get; set; }
        public string HRMEB_ActiveFlag { get; set; }
    }
}
