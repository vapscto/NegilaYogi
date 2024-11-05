using DomainModel.Model.com.vapstech.admission;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.admission
{
    [Table("IVRM_StudentDetails_Update")]
    public class IVRM_StudentDetails_UpdateDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISSU_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public DateTime? AMST_Date { get; set; }
        public string AMST_Street { get; set; }
        public string AMST_FatherName { get; set; }
        public string AMST_MotherName { get; set; }
        public string AMST_Area { get; set; }
        public string AMST_City { get; set; }
        public string AMST_State { get; set; }
        public string AMST_Country { get; set; }
        public long? AMST_Pincode { get; set; }
        public long AMST_FMobileNo { get; set; }
        public string AMST_FEmailId { get; set; }
        public long AMST_MMobileNo { get; set; }
        public string AMST_MEmailId { get; set; }
        public long AMST_FMobileNo_SMS { get; set; }
        public long AMST_MMobileNo_SMS { get; set; }
        public string AMST_FEmail_Mail { get; set; }
        public string AMST_MEmail_Mail { get; set; }
        public string AMST_Fimage { get; set; }
        public string AMST_Mimage { get; set; }
        public string AMST_AddressProof { get; set; }
        public string AMST_AddressAppr_Status { get; set; }
 

    }
}

