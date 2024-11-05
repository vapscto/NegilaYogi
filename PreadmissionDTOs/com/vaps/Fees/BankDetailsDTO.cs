using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class BankDetailsDTO
    {
        public long FBD_ID { get; set; }
        public string Class_Category { get; set; }
        public long Class { get; set; }
        public long ASMCL_Id { get; set; }
        public string classname { get; set; }
       
        public string Bank_Name { get; set; }
        public string Bank_Address { get; set; }
        public string Acc_No { get; set; }
        public long L_code { get; set; }
        public string IFSC { get; set; }
        public string ACC_name { get; set; }
        public string msg { get; set; }
        public long MI_Id { get; set; }
        public bool Active_Flag { get; set; }
        public bool duplicate { get; set; }
        public bool ret { get; set; }
        public int UserId { get; set; }
        public Array alldata { get; set; }
        public Array Editlist { get; set; }
        public Array classlist { get; set; }






    }    
}
