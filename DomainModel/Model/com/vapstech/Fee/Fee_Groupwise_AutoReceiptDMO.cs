using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_Groupwise_AutoReceipt")]
    public class Fee_Groupwise_AutoReceiptDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FGAR_Id { get; set; }
        public long MI_Id { get; set; }     
        public long ASMAY_Id { get; set; }
        public string FGAR_PrefixFlag { get; set; }
        public string FGAR_PrefixName { get; set; }
        public string FGAR_SuffixFlag { get; set; }
        public string FGAR_SuffixName { get; set; }
        public string FGAR_Name { get; set; }
        public string FGAR_Address { get; set; }
       
       public long FGAR_Starting_No { get; set; }

        public string FGAR_Template_Name { get; set; }

        public long? FGAR_CreatedBy { get; set; }
        public long? FGAR_UpdatedBy { get; set; }

        public DateTime? FGAR_CreatedDate { get; set; }
        public DateTime? FGAR_UpdatedDate { get; set; }
    }
}
