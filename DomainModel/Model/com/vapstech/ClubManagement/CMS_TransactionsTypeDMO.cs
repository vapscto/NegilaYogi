using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.ClubManagement
{
    [Table("CMS_TransactionsType", Schema = "CMS")]

    public class CMS_TransactionsTypeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long CMSTRANSTY_Id { get; set; }
        public long MI_Id { get; set; }
        public string CMSTRANSTY_TransactionsName { get; set; }
        public string CMSTRANSTY_AliasName { get; set; }
        public bool CMSTRANSTY_AllowCreditTransFlg { get; set; }
        public string CMSTRANSTY_ConsiderForMinTransFlg { get; set; }
        public bool CMSTRANSTY_CompulsoryFlg { get; set; }
        public bool CMSTRANSTY_MemberCanChooseFlg { get; set; }
        public bool CMSTRANSTY_CoverChargeFlg { get; set; }
        public bool CMSTRANSTY_BarTransactionFlg { get; set; }
        public bool CMSTRANSTY_FoodTransactionFlg { get; set; }
        public bool CMSTRANSTY_CardTransactionFlg { get; set; }
        public decimal CMSTRANSTY_Amount { get; set; }
        public bool CMSTRANSTY_ActiveFlag { get; set; }
        public DateTime? CMSTRANSTY_CreatedDate { get; set; }
       public long CMSTRANSTY_CreatedBy { get; set; }
       public DateTime? CMSTRANSTY_UpdatedDate { get; set; }
       public long CMSTRANSTY_UpdatedBy { get; set; }
      
    }
}
