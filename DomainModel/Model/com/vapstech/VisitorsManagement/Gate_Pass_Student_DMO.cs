using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.VisitorsManagement
{
    [Table("Gate_Pass_Student", Schema = "VM")]
    public class Gate_Pass_Student_DMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long GPHS_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public string GPHS_GatePassNo { get; set; }
        public string GPHS_IDCardNo { get; set; }
        public DateTime? GPHS_DateTime { get; set; }
        public string GPHS_Remarks { get; set; }
        public bool GPHS_ActiveFlg { get; set; }
        public long? GPHS_CreatedBy { get; set; }
        public long? GPHS_UpdatedBy { get; set; }
        public string GPHS_ReceiverName { get; set; }
        public string GPHS_ReceiverPhoneNo { get; set; }
        public string GPHS_ReceiverIdProof { get; set; }
        public string GPHS_ReceiverIdProofNo { get; set; }
        public string GPHS_SecretCode { get; set; }
        public string GPHS_OTP { get; set; }
        public bool? GPHS_SentFlg { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }


    }
}
