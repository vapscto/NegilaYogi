using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Fee
{
    [Table("Fee_Payment_Settlement_Details")]
    public class Fee_Payment_Settlement_DetailsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FYPPSD_Id { get; set; }
        public string FYPPSD_PAYU_Id { get; set; }
        public long FYPPSD_Transaction_amount { get; set; }
        public string FYPPSD_Payment_Id { get; set; }
        public string FYPPSD_Payment_Mode { get; set; }
        public string FYPPSD_Payment_Status { get; set; }
        public DateTime FYPPSD_Transaction_Date { get; set; }
        public long FYPPSD_Payment_Amount { get; set; }      
        public long FYPPST_Id { get; set; }
        public long MI_Id { get; set; }
    }
}
