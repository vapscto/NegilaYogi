using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("fee_bank_details")]
    public class FeeBankDetailsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 FBD_ID { get; set; }
        public string Class_Category { get; set; }
        public string Class { get; set; }
        public string Bank_Name { get; set; }
        public string Bank_Address { get; set; }
        public string Acc_No { get; set; }
        public Int64 L_code { get; set; }
        public string IFSC { get; set; }
        public string ACC_name { get; set; }

        public long MI_Id { get; set; }
    }
}
