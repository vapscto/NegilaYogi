using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_NonBook_Transaction", Schema = "LIB")]
    public class LIB_NonBook_Transaction_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LNBTR_Id { get; set; }
        public long MI_Id { get; set; }
        public long LMNBKANO_Id { get; set; }
        public string LNBTR_MemberFlg { get; set; }
        public DateTime LNBTR_IssuedDate { get; set; }
        public string LNBTR_GuestName { get; set; }
        public long LNBTR_GuestContactNo { get; set; }
        public string LNBTR_GuestEmailId { get; set; }
        public DateTime LNBTR_DueDate { get; set; }
        public string LNBTR_Status { get; set; }
        public DateTime LNBTR_ReturnedDate { get; set; }
        public DateTime? LNBTR_RenewedDate { get; set; }
        public int LNBTR_RenewalCount { get; set; }
        public decimal LNBTR_TotalFine { get; set; }
        public decimal LNBTR_FineCollecetd { get; set; }
        public decimal LNBTR_FineWaived { get; set; }
        public bool LNBTR_ActiveFlg { get; set; }
        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }


        public List<LIB_NonBook_Transaction_Student_DMO> LIB_NonBook_Transaction_Student_DMO { get;set; }
        public List<LIB_NonBook_Transaction_Staff_DMO> LIB_NonBook_Transaction_Staff_DMO { get;set; }
        public List<LIB_NonBook_Transaction_Department_DMO> LIB_NonBook_Transaction_Department_DMO { get;set; }
        public List<LIB_NonBook_Transaction_Student_College_DMO> LIB_NonBook_Transaction_Student_College_DMO { get; set; }

    }
}
