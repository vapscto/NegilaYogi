using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.ClubManagement
{
   public class CMS_TrasanctionTypeDTO
    {
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

        public long user_id { get; set; }

        public Array loadDetails { get; set; }
        public string returnval { get; set; }

        public Array cmsdetails { get; set; }
    }
}
