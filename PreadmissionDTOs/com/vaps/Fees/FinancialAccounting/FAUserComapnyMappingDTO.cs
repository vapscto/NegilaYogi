using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Fees.FinancialAccounting
{
   public class FAUserComapnyMappingDTO
    {
        public long FAUCM_Id { get; set; }
        public long FAMCOMP_Id { get; set; }
        public long MI_Id { get; set; }
        public long user_id { get; set; }
        public long muser_Id { get; set; }
        public bool FAUCM_ActiveFlg { get; set; }
        public string FAUCM_Password { get; set; }
        public DateTime FAUCM_CreatedDate { get; set; }
        public DateTime FAUCM_UpdatedDate { get; set; }
      
        public long FAUCM_CreatedBy { get; set; }
        public long FAUCM_UpdatedBY { get; set; }
        public string returnval { get; set; }

        public Array UserCompanyDetails { get; set; }
        public Array fillcompany { get; set; }
        
        public string FAMCOMP_CompanyName { get; set; }

        public Array fillfinacialUser { get; set; }

        public string UserName { get; set; }
        public long Id { get; set; }

    }
}
