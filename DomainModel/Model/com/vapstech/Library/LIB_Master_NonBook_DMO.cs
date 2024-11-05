using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_Master_NonBook", Schema = "LIB")]
    public class LIB_Master_NonBook_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LMNBK_Id { get; set; }
        public long MI_Id { get; set; }
        public long LMPE_Id { get; set; }
        public long LMP_Id { get; set; }
        public long LMD_Id { get; set; }
        public long LMV_Id { get; set; }
        public long LMC_Id { get; set; }
        public long LML_Id { get; set; }
        public long LMSU_Id { get; set; }
        public string LMNBK_NonBookTitle { get; set; }
        public string LMNBK_VolumeNo { get; set; }
        public string LMNBK_IssueNo { get; set; }
        public string LMNBK_PeriodicalTypeFlg { get; set; }
        public string LMNBK_ISBN { get; set; }
        public DateTime LMNBK_PublishDate { get; set; }
        public DateTime LMNBK_PurchaseDate { get; set; }
        public string LMNBK_BindStatus { get; set; }
        public decimal LMNBK_Price { get; set; }        
        public bool LMNBK_WithAccessoriesFlg { get; set; }
        public decimal LMNBK_Discount { get; set; }
        public string LMNBK_DiscountTypeFlg { get; set; }
        public decimal LMNBK_NetPrice { get; set; }
        public string LMNBK_BindingType { get; set; }
        public string LMNBK_BillNo { get; set; }
        public string LMNBK_VoucherNo { get; set; }
        public string LMNBK_NoOfPages { get; set; }
        public string LMNBK_SourceType { get; set; }
        public string LMNBK_DonarName { get; set; }
        public string LMNBK_DonarAddress { get; set; }
        public string LMNBK_Keywords { get; set; }
        public DateTime LMNBK_BillDate { get; set; }
        public string LMNBK_NoOfCopies { get; set; }
        public string LMNBK_ReferenceNo { get; set; }
        public string LMNBK_Description { get; set; }
        public string LMNBK_CurrencyType { get; set; }
        public bool LMNBK_ActiveFlg { get; set; }
        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }

        public List<LIB_Master_NonBook_AccnNo_DMO> LIB_Master_NonBook_AccnNo_DMO { get; set; }       
        public List<LIB_Master_NonBook_KeyFactor_DMO> LIB_Master_NonBook_KeyFactor_DMO { get; set; }
        public List<LIB_Master_NonBook_Library_DMO> LIB_Master_NonBook_Library_DMO { get; set; }
        public List<LIB_Master_NonBook_Accessories_DMO> LIB_Master_NonBook_Accessories_DMO { get; set; }


    }
}
