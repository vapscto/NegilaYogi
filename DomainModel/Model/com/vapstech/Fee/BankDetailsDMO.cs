using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Fee
{
    [Table("Fee_Bank_Details")]
    public class BankDetailsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FBD_ID { get; set; }
        public string Class_Category { get; set; }
        public long Class { get; set; }
        public string Bank_Name { get; set; }
   
    
        public string Bank_Address { get; set; }
        public string Acc_No { get; set; }
        public long L_code { get; set; }
        public string IFSC { get; set; }
        public string ACC_name { get; set; }
        public long MI_Id { get; set; }
        public bool Active_Flag { get; set; }
    }
}
