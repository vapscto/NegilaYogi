using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Fees.FinancialAccounting
{
   public class FACompanyFYMappingDTO
    {
        public long FACFYM_Id { get; set; }

        public long MI_Id { get; set; }
        public long FAMCOMP_Id { get; set; }

        public long IMFY_Id { get; set; }

        public string FACFYM_RefNo { get; set; }
        public bool FACFYM_FinancialYearCloseFlg { get; set; }
        public bool FACFYM_ActiveFlg { get; set; }
        public decimal? FACFYM_Budget { get; set; }

        public DateTime? FACFYM_BBDate { get; set; }
        public DateTime? FACFYM_CreatedDate { get; set; }
        public DateTime? FACFYM_UpdatedDate { get; set; }

        public long FACFYM_UpdatedBY { get; set; }
        public long FACFYM_CreatedBy { get; set; }

        public string returnval { get; set; }

        public long user_id { get; set; }
    

        public Array FYDetails { get; set; }

        public Array fillfinacialyear { get; set; }

        public string FAMCOMP_CompanyName { get; set; }
        public Array fillcompany { get; set; }
        public Array fillfinacialUser { get; set; }
        public string IMFY_FinancialYear { get; set; }

      
      
    }
}
