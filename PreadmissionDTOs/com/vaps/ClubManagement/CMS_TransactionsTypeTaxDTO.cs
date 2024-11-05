using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.ClubManagement
{
   public  class CMS_TransactionsTypeTaxDTO
    {
        public long CMSTRANSTYTAX_Id { get; set; }
        public long CMSTRANSTY_Id { get; set; }
        public string CMSTRANSTY_TaxNo { get; set; }
        public decimal CMSTRANSTYTAX_TaxPercent { get; set; }
        public bool CMSTRANSTYTAX_ActiveFlag { get; set; }
        public DateTime? CMSTRANSTYTAX_CreatedDate { get; set; }
        public long CMSTRANSTYTAX_CreatedBy { get; set; }
        public DateTime? CMSTRANSTYTAX_UpdatedDate { get; set; }
        public long CMSTRANSTYTAX_UpdatedBy { get; set; }

        public long user_id { get; set; }

        public Array fill_TaxTransaction { get; set; }

        public Array fill_Taxdetails { get; set; }

        public string CMSTRANSTY_TransactionsName { get; set; }
        public string returnval { get; set; }



    }
}
