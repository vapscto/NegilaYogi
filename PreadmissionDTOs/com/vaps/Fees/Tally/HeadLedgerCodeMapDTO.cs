using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Fees.Tally
{
    public class HeadLedgerCodeMapDTO
    {
        public long FYGHLM_Id { get; set; }
        public long FYGHM_Id { get; set; }
        public string FYGHM_RVRegLedgerUnder { get; set; }
        public string FYGHM_RVRegLedgerId { get; set; }
        public string FYGHM_RVAdvanceLegderId { get; set; }
        public string FYGHM_RVAdvanceLegderUnder { get; set; }
        public string FYGHM_RVArrearLedgerId { get; set; }
        public string FYGHM_RVArrearLedgerUnder { get; set; }
        public string FYGHM_JVRegLedgerId { get; set; }
        public string FYGHM_JVRegLedgerUnder { get; set; }
        public string FYGHM_JVAdvanceLegderId { get; set; }
        public string FYGHM_JVAdvanceLegderUnder { get; set; }
        public string FYGHM_JVArrearLedgerId { get; set; }
        public string FYGHM_JVArrearLedgerUnder { get; set; }

        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public Array academicdrp { get; set; }
        public Array totaldata { get; set; }

        public string FMH_FeeName { get; set; }
        public Array fillmastergroup { get; set; }
        public long FMH_Id { get; set; }

        public Array fillheaddata { get; set; }

        public long FMG_Id { get; set; }

        public Array fillconfig { get; set; }

        public HeadLedgerCodeMapDTO[] savetmpdata { get; set; }

        public string returnval { get; set; }
        public long FTMCOM_Id { get; set; }
        public string FMG_GroupName { get; set; }
        public Array FTMCOM_companyname { get; set; }

        public Array savednotsavedlist { get; set; }

    }
}
