using System;
using PreadmissionDTOs.com.vaps.College.Fees;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class CLGFeeGroupwiseRecieptDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<CLGFeeGroupwiseRecieptDTO, CLGFeeGroupwiseRecieptDTO> COMMM = new CommonDelegate<CLGFeeGroupwiseRecieptDTO, CLGFeeGroupwiseRecieptDTO>();

        public CLGFeeGroupwiseRecieptDTO getdata(CLGFeeGroupwiseRecieptDTO enqdto)
        {
            return COMMM.POSTDataCollfee(enqdto, "CLGFeeGroupwiseRecieptFacade/getalldetails/");
        }

        public CLGFeeGroupwiseRecieptDTO getcourse(CLGFeeGroupwiseRecieptDTO enqdto)
        {
            return COMMM.POSTDataCollfee(enqdto, "CLGFeeGroupwiseRecieptFacade/getcoursedetails");
        }
        public CLGFeeGroupwiseRecieptDTO onsemselection(CLGFeeGroupwiseRecieptDTO enqdto)
        {
            return COMMM.POSTDataCollfee(enqdto, "CLGFeeGroupwiseRecieptFacade/onsemselection");
        }
        public CLGFeeGroupwiseRecieptDTO onselectsec(CLGFeeGroupwiseRecieptDTO enqdto)
        {
            return COMMM.POSTDataCollfee(enqdto, "CLGFeeGroupwiseRecieptFacade/onselectsec");
        }


        public CLGFeeGroupwiseRecieptDTO getbran(CLGFeeGroupwiseRecieptDTO enqdto)
        {
            return COMMM.POSTDataCollfee(enqdto, "CLGFeeGroupwiseRecieptFacade/getbranchdetails");
        }

        public CLGFeeGroupwiseRecieptDTO getcoubransem(CLGFeeGroupwiseRecieptDTO enqdto)
        {
            return COMMM.POSTDataCollfee(enqdto, "CLGFeeGroupwiseRecieptFacade/getsemesterdetails");
        }

   
        public CLGFeeGroupwiseRecieptDTO getreceiptreport(CLGFeeGroupwiseRecieptDTO enqdto)
        {
            return COMMM.POSTDataCollfee(enqdto, "CLGFeeGroupwiseRecieptFacade/getreceiptreport");
        }

        public CLGFeeGroupwiseRecieptDTO getreceipt(CLGFeeGroupwiseRecieptDTO enqdto)
        {
            return COMMM.POSTDataCollfee(enqdto, "CLGFeeGroupwiseRecieptFacade/getreceipt");
        }

        
    }
}
