using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_Y_Payment_Staff")]
    public class Fee_Y_Payment_StaffDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FYPS_Id { get; set; }
        [ForeignKey("FYP_Id")]
        public long FYP_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long FYPS_TotalPaidAmount { get; set; }
        public long FYPS_TotalWaivedAmount { get; set; }
        public long FYPS_TotalConcessionAmount { get; set; }
        public decimal FYPS_TotalFineAmount { get; set; }
       
    }
}
