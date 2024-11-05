using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Fee.FinancialAccounting
{
    [Table("FA_M_Voucher")]
    public  class FA_M_VoucherDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FAMVOU_Id { get; set; }
        public long MI_Id { get; set; }
        public long FAMCOMP_Id { get; set; }
        public long IMFY_Id { get; set; }
        public string FAMVOU_VoucherType { get; set; }  
       public string FAMVOU_VoucherNo { get; set; }
        public DateTime? FAMVOU_VoucherDate { get; set; }
        public string FAMVOU_Narration { get; set; }
        public string FAMVOU_Suffix { get; set; }
        public string FAMVOU_Prefix { get; set; }
        //public string FAMVOU_VNo { get; set; }     
        public string FAMVOU_UserVoucherType { get; set; }
        public string FAMVOU_APIReferenceNo { get; set; }
        public bool FAMVOU_BillwiseFlg { get; set; }
        public string FAMVOU_Description { get; set; }
        public bool FAMVOU_ActiveFlg { get; set; }
        public DateTime? FAMVOU_CreatedDate { get; set; }
        public DateTime? FAMVOU_UpdatedDate { get; set; }
        public long FAMVOU_CreatedBy { get; set; }
        public long FAMVOU_UpdatedBy { get; set; }
       public List<FA_T_VoucherDMO> FA_T_VoucherDMO { get; set; }

    }
}
