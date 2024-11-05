using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_Y_Payment_ThirdParty")]
    public class Fee_Y_Payment_ThirdPartyDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FYPTP_Id { get; set; }
        public long FYP_Id { get; set; }
        public string FYPTP_Name { get; set; }
        public decimal FTP_TotalPaidAmount { get; set; }
        public string FYPTP_Towards { get; set; }
        public long FMH_Id { get; set; }
    }
}