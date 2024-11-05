using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Master_Employee_MobileNo")]
    public class Multiple_Mobile_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMEMNO_Id { get; set; }
        [ForeignKey("HRME_Id")]
        public long HRME_Id { get; set; }
        public long HRMEMNO_MobileNo { get; set; }
        public string HRMEMNO_DeFaultFlag { get; set; }

    }
}
