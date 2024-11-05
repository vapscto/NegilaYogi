using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeChequeBounceDTO
    {
        public long FCB_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMAY_ID { get; set; }
        public long MI_ID { get; set; }
        public long FYP_ID { get; set; }
        public decimal FCB_Amount { get; set; }
        public string FCB_Remarks { get; set; }
        public bool FCB_ActiveFlag { get; set; }
        public DateTime FCB_DATE { get; set; }
        public Array fillyear { get; set; }
        public Array fillstudent { get; set; }
        public Array fillreceipt { get; set; }

        public Array alldata { get; set; }

        public string fyP_Receipt_No { get; set; }
        public bool returnval { get; set; }

        public string validationvalue { get; set; }

        public string ASMAY_Year { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }
        public string searchType { get; set; }
        public string searchtext { get; set; }
       // public string searchnumber { get; set; }
        public DateTime searchdate { get; set; }

        //MB
        public Array classlist { get; set; }
        public long ASMCL_Id { get; set; }
        public int user_id { get; set; }

        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }

        public Array fillsection { get; set; }
        public long ASMS_Id { get; set; }
        //MB
    }
}
