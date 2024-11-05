using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Process_Authorisation")]
    public class HR_PROCESSDMO :CommonParamDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRPA_Id { get; set; }
        public long MI_Id { get; set; }
       // public long HRMLN_Id { get; set; }
        public long HRMGT_Id { get; set; }
        public long HRMD_Id { get; set; }
        public long HRMDES_Id { get; set; }
        public long HRMG_Id { get; set; }
        public string HRLP_EmailTo { get; set; }
        public string HRLP_EmailCC { get; set; }
        public string HRPA_TypeFlag { get; set; }
        public long? HRPA_UpdatedBy { get; set; }
        public long? HRPA_CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public List<HR_Process_Auth_OrderNoDMO> HR_Process_Auth_OrderNoDMO { get; set; }
    }
}
