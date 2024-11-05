using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.LeaveManagement
{
    [Table("IVRM_Month")]
    public class IVRM_Month_DMO :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRM_Month_Id { get; set; }
        public string IVRM_Month_Name { get; set; }
        public bool Is_Active { get; set; }
        public int IVRM_Month_Max_Days { get; set; }
    }
}
