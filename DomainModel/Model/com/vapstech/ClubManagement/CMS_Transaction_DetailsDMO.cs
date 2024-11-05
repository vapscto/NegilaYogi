using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.ClubManagement
{
    [Table("CMS_Transaction_Details", Schema = "CMS")]
    public class CMS_Transaction_DetailsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CMSTRANSDET_Id { get; set; }
        public long CMSTRANS_Id { get; set; }
        public long CMSTRANSMEMTYINT_Id { get; set; }
        public decimal CMSTRANSDET_Qty { get; set; }
        public decimal CMSTRANSDET_Amount { get; set; }
        public decimal CMSTRANSDET_Tax { get; set; }
        public decimal CMSTRANSDET_NetAmount { get; set; }
        public bool CMSTRANSDET_ActiveFlg { get; set; }
        public DateTime? CMSTRANSDET_CreatedDate { get; set; }
        public long CMSTRANSDET_CreatedBy { get; set; }
        public DateTime? CMSTRANSDET_UpdatedDate { get; set; }
        public long CMSTRANSDET_UpdatedBy { get; set; }


    }
}
