using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class Fee_PaymentGateway_DetailsDTO
    {
        public long FPGD_Id { get; set; }
        public long MI_Id { get; set; }
        public long user_id { get; set; }       
        public string FPGD_MerchantId { get; set; }
        public string FPGD_SaltKey { get; set; }
        public string FPGD_AuthorisationKey { get; set; }
        public string FPGD_URL { get; set; }
        public string FPGD_SubMerchantId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Array paymentGatewayDetailList { get; set; }
        public Array institutionlist { get; set; }
        public Array gateway_list { get; set; }
        public string IMPG_PGName { get; set; }
        public string FPGD_PGName { get; set; }
        public long IMPG_Id { get; set; }
        public string FPGD_Image { get; set; }
        public bool FPGD_MobileActiveFlag { get; set; }
        public string FPGD_AccNo { get; set; }
        public Fee_PaymentGateway_DetailsDTO[] SubmerchantIds { get; set;}
        public string institutionName { get; set; }
        public string gatewayName { get; set; }
        public string returnval { get; set; }
        public int count { get; set; }
        public string Isduplicate { get; set; }        
    }
}
