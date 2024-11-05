using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Master_BankDeatils")]
    public class HR_Master_BankDeatils:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long HRMBD_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMBD_BankName { get; set; }
        public string HRMBD_BankAccountNo { get; set; }

        public string HRMBD_BankAddress { get; set; }
        public string HRMBD_BranchName { get; set; }
        public string HRMBD_IFSCCode { get; set; }
        public bool HRMBD_ActiveFlag { get; set; }


    }
}
