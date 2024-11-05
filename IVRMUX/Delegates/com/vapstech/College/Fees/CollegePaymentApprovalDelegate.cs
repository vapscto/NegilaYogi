using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.College.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Fees
{
    public class CollegePaymentApprovalDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<CollegePaymentApprovalDTO, CollegePaymentApprovalDTO> COMMM = new CommonDelegate<CollegePaymentApprovalDTO, CollegePaymentApprovalDTO>();


        public CollegePaymentApprovalDTO getdetails(CollegePaymentApprovalDTO data)
        {
            return COMMM.POSTDataCollfee(data, "CollegePaymentApprovalFacade/getdetails/");
        }

        public CollegePaymentApprovalDTO Getreportdetails(CollegePaymentApprovalDTO data)
        {
            return COMMM.POSTDataCollfee(data, "CollegePaymentApprovalFacade/Getreportdetails/");


        }
        public CollegePaymentApprovalDTO savedetails(CollegePaymentApprovalDTO data)
        {
            return COMMM.POSTDataCollfee(data, "CollegePaymentApprovalFacade/savedetails/");


        }

    }
}
