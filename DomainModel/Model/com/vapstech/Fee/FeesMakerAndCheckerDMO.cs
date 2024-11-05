using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Fee
{
    [Table("Fee_Y_Payment_Approval ")]
    public class FeesMakerAndCheckerDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FYPAPP_Id { get; set; }
        public long FYP_Id { get; set; }
        public long HRME_Id { get; set; }
        public bool FYPAPP_ApprovedFlg { get; set; }
        public string FYPAPP_Remarks { get; set; }
        public DateTime FYPAPP_DateTime { get; set; }
        public bool FYPAPP_ActiveFlg { get; set; }
        public DateTime FYPAPP_CreatedDate { get; set; }
        public DateTime FYPAPP_UpdatedDate { get; set; }
        public long FYPAPP_CreatedBy { get; set; }
        public long FYPAPP_UpdatedBy { get; set; }
    }
}
