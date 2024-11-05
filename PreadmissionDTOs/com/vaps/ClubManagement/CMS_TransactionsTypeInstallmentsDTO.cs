using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.ClubManagement
{
    public class CMS_TransactionsTypeInstallmentsDTO
    {
        public long CMSTRANSTYINT_Id { get; set; }
        public long CMSTRANSTY_Id { get; set; }
        public long CMSMINST_Id { get; set; }
        public decimal CMSTRANSTYINT_Amount { get; set; }
        public int CMSTRANSTYINT_InstlOrder { get; set; }
        public bool CMSTRANSTYINT_ActiveFlag { get; set; }
        public DateTime? CMSTRANSTYINT_CreatedDate { get; set; }
        public long CMSTRANSTYINT_CreatedBy { get; set; }
        public DateTime? CMSTRANSTYINT_UpdatedDate { get; set; }
        public long CMSTRANSTYINT_UpdatedBy { get; set; }

        public long user_id { get; set; }
        public string returnval { get; set; }

        public Array fill_Transaction { get; set; }
        public Array fill_Installment { get; set; }

        public Array fill_details { get; set; }

        public string CMSMINST_InstallmentName { get; set; }

        public string CMSTRANSTY_TransactionsName { get; set; }

    }
}
