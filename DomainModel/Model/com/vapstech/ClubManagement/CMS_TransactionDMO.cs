using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.ClubManagement
{
    [Table("CMS_Transaction", Schema = "CMS")]
    public class CMS_TransactionDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CMSTRANS_Id { get; set; }
        public long MI_Id { get; set; }        
        public long CMSMMEM_Id { get; set; }
        public long IMFY_Id { get; set; }
        
        public string CMSTRANS_TransactionNo { get; set; }
        public DateTime? CMSTRANS_Date { get; set; }
        public decimal CMSTRANS_TotalAmount { get; set; }
        public decimal CMSTRANS_TotalTax { get; set; }
        public decimal CMSTRANS_TotalNetAmount { get; set; }
        public bool CMSTRANS_CreditTransFlg { get; set; }
        public long CMSTRANS_NoOFGuests { get; set; }
        public string CMSTRANS_GuestsName { get; set; }
        public string CMSTRANS_GuestContactNo { get; set; }
        public string CMSTRANS_Remarks { get; set; }
        public bool CMSTRANS_ActiveFlg { get; set; }
        public DateTime? CMSTRANS_CreatedDate { get; set; }
        public long CMSTRANS_CreatedBy { get; set; }
        public DateTime? CMSTRANS_UpdatedDate { get; set; }
        public long CMSTRANS_UpdatedBy { get; set; }
        public List<CMS_Transaction_MemberDMO> CMS_Transaction_MemberDMO { get; set; }
        public List<CMS_Transaction_NonMemberDMO> CMS_Transaction_NonMemberDMO { get; set; }

    }
}
