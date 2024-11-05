using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class Student_SettlementDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long TRMR_Id { get; set; }
        public DateTime Selected_Date { get; set; }
        public DateTime From_Date { get; set; }
        public DateTime To_Date { get; set; }
        public long FPGD_Id { get; set; }
        public Array yearlist { get; set; }
        public Array merchantlist { get; set; }
        public bool returnval { get; set; }
        public bool settled_flag { get; set; }
        public Array savedlist { get; set; }
        public DateTime[] datelist { get; set; }
        public long FYPPST_Id { get; set; }
        public Array viewlist { get; set; }
        public Array routelist { get; set; }
        public Array classlist { get; set; }
        public Array sectionlist { get; set; }
        public Array reportlist { get; set; }

        public long AMST_Id { get; set; }
        public string AMST_FirstName { get; set; }        
        public string AMST_RegistrationNo { get; set; }
        public string AMST_AdmNo { get; set; }
        public long AMAY_RollNo { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public long FYP_Id { get; set; }
        public long FYPPSD_Id { get; set; }
        public string FYPPSD_Payment_Mode { get; set; }
        public string FYPPSD_Payment_Id { get; set; }
        public string FYPPSD_PAYU_Id { get; set; }
        public long FYPPSD_Payment_Amount { get; set; }
        public DateTime FYPPSD_Transaction_Date { get; set; }        
        public DateTime FYPPST_Settlement_Date { get; set; }
        public string FYPPST_UTR_No { get; set; }
        public string FYPPST_Settlement_Id { get; set; }

        public string allclass { get; set; }

        public string allsection { get; set; }
        public long user_id { get; set; }
        public Array alldata { get; set; }
        public Array studentlist { get; set; }
        public Array allgroupheaddata { get; set; }

        public Array paymentgatewaydet { get; set; }

        public long IMPG_Id { get; set; }

        public long FMG_Id { get; set; }

        public string IMPG_PGFlag { get; set; }
        public string IMPG_SettlementURL { get; set; }

        public string MID { get; set; }
        public string FPGD_SubMerchantKey { get; set; }
        public string FPGD_AuthorizationHeader { get; set; }
        public string FPGD_AuthorisationKey { get; set; }

        public string ORDER_ID { get; set; }
        public string TRANSFER_ID { get; set; }

        public string PAYMENT_ID { get; set; }

        public Array settsumm { get; set; }
        public long overallsettamt { get; set; }

        public long SettmentAmount { get; set; }

        public Array paymentlogs { get; set; }
    }
}
