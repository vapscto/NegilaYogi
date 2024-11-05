using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Preadmission_School_Enquiry")]
    public class Enquiry : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PASE_Id { get; set; }

       
        public long MI_Id { get; set; }
      
        public long ASMAY_Id { get; set; }
       
        public long AMC_Id { get; set; }
       
        public long ASMCL_Id { get; set; }
        public string PASE_FirstName { get; set; }
        public string PASE_MiddleName { get; set; }
        public string PASE_LastName { get; set; }
        public DateTime PASE_Date { get; set; }
        public string PASE_EnquiryNo { get; set; }
        public string PASE_Address1 { get; set; }
        public string PASE_Address2 { get; set; }
        public string PASE_Address3 { get; set; }
      
      
        public string PASE_City { get; set; }
        public string PASE_State { get; set; }
        public long IVRMMC_Id { get; set; }
        public long PASE_Pincode { get; set; }
        public long PASE_MobileNo { get; set; }
        public string PASE_emailid { get; set; }
        public int PASE_ActiveFlag { get; set; }
        public long PASE_Phone { get; set; }

        public long Id { get; set; }

        public string PASE_EnquiryDetails { get; set; }

    }


}
