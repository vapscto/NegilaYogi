using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_Payment_Settlement_Details_College", Schema = "CLG")]
    public class Fee_Payment_Settlement_Details_CollegeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FYPPSDC_Id { get; set; }
        public string FYPPSDC_PAYU_Id { get; set; }
        public long FYPPSDC_Transaction_amount { get; set; }
        public string FYPPSDC_Payment_Id { get; set; }
        public string FYPPSDC_Payment_Mode { get; set; }
        public string FYPPSDC_Payment_Status { get; set; }
        public DateTime FYPPSDC_Transaction_Date { get; set; }
        public long FYPPSDC_Payment_Amount { get; set; }
        public long FYPPSTC_Id { get; set; }
        public long MI_Id { get; set; }
    }
}
