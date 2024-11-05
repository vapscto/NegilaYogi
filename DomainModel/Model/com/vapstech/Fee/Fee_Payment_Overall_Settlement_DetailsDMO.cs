using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Fee
{
    [Table("Fee_Payment_Overall_Settlement_Details")]
    public class Fee_Payment_Overall_Settlement_DetailsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FYPPST_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string FYPPST_Settlement_Id { get; set; }
        public DateTime FYPPST_Settlement_Date { get; set; }
        public long FYPPST_Settlement_Amount { get; set; }
        public string FYPPST_UTR_No { get; set; }
        public List<Fee_Payment_Settlement_DetailsDMO> Fee_Payment_Settlement_Details { get; set; }

        public long User_id { get; set; }
    }
}
