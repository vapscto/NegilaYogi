using DomainModel.Model.com.vaps.Fee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_Y_Payment_PA_Application", Schema = "CLG")]
    public class Fee_Y_Payment_PA_Application 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FYPPA_Id { get; set; }
        public long FYP_Id { get; set; }
        public long PACA_Id { get; set; }
        public decimal FYPPA_TotalPaidAmount { get; set; }
        public long FYPPA_ActiveFlag { get; set; }
        public string FYPPA_Type { get; set; }
       

    }
}
