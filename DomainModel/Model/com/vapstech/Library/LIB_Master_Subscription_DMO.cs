using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_Master_Subscription", Schema = "LIB")]
    public class LIB_Master_Subscription_DMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LMSU_Id { get; set; }
        public long MI_Id { get; set; }
        public long LMPE_Id { get; set; }
        public long LMP_Id { get; set; }
        public long LMD_Id { get; set; }
        public long LMV_Id { get; set; }
        public long LMC_Id { get; set; }
        public long LML_Id { get; set; }
        public string LMSU_PeriodicalTitle { get; set; }
        public string LMSU_SubscriptionNo { get; set; }
        public string LMSU_PeriodicalTypeFlg { get; set; }
        public DateTime? LMSU_SubscriptionDate { get; set; }
        public DateTime? LMSU_ExpiryDate { get; set; }
        public DateTime? LMSU_PreTerminationDate { get; set; }
        public decimal LMSU_Price { get; set; }
        public decimal LMSU_Discount { get; set; }
        public string LMSU_DoscountTypeFlg { get; set; }
        public decimal LMSU_NetPrice { get; set; }
        public string LMSU_OrderNo { get; set; }
        public DateTime? LMSU_OrderDate { get; set; }
        public int LMSU_NoOfCopies { get; set; }
        public string LMSU_StartVolumeNo { get; set; }
        public string LMSU_StartIssueNo { get; set; }
        public DateTime? LMSU_ExpectedDate { get; set; }
        public bool LMSU_AutoAccnNoFlg { get; set; }
        public DateTime? LMSU_EntryDate { get; set; }
        public int LMSU_NoOfIssues { get; set; }
        public string LMSU_CurrencyType { get; set; }
        public bool LMSU_PreTerminateFlg { get; set; }
        public bool LMSU_ActiveFlg { get; set; }
        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }


    }
}
