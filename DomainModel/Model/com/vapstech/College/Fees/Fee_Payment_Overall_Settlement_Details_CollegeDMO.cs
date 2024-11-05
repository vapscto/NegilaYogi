using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_Payment_Overall_Settlement_Details_College", Schema = "CLG")]
    public class Fee_Payment_Overall_Settlement_Details_CollegeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FYPPSTC_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string FYPPSTC_Settlement_Id { get; set; }
        public DateTime FYPPSTC_Settlement_Date { get; set; }
        public long FYPPSTC_Settlement_Amount { get; set; }
        public string FYPPSTC_UTR_No { get; set; }
        public List<Fee_Payment_Settlement_Details_CollegeDMO> Fee_Payment_Settlement_Details_College { get; set; }

        public long User_Id { get; set; }

        public DateTime FYPPSTC_CreatedDate { get; set; }
          public DateTime FYPPSTC_UpdatedDate { get; set; }
        public long FYPPSTC_CreatedBy { get; set; }
         public long FYPPSTC_UpdatedBy { get; set; }
    }
}
