using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_Master_Student_Group_Installment")]
    public class FeeStudentGroupInstallmentMappingDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMSGI_Id { get; set; }
        public long FMSG_Id { get; set; }
        public long FMH_ID { get; set; }
        public long FTI_ID { get; set; }
    }
}
