using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Canteen
{
    [Table("CM_Transaction_Items")]
    public class CM_Transaction_ItemsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long CMTRANSI_Id { get; set; }
        public long CMTRANS_Id { get; set; }
        public long CMMFI_Id { get; set; }
        public string CMTRANSI_name { get; set; }
        public decimal CMTRANS_Qty { get; set; }
        public decimal CMTRANSI_UnitRate { get; set; }
        public decimal CMTRANSI_Amount { get; set; }
        public bool CMTRANSI_TaxApplicableFlg { get; set; }
        public bool CMTRANSI_ActiveFlg { get; set; }
        public long? CMTRANSI_CreatedBy { get; set; }
        public long? CMTRANSI_UpdatedBy { get; set; }
        public DateTime? CMTRANSI_CreatedDate { get; set; }
        public DateTime? CMTRANSI_Updateddate { get; set; }

    }
}
