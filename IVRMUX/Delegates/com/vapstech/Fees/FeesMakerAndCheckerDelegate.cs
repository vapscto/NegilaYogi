using CommonLibrary;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Fees
{
    public class FeesMakerAndCheckerDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<FeesMakerAndCheckerDTO, FeesMakerAndCheckerDTO> COMMM = new CommonDelegate<FeesMakerAndCheckerDTO, FeesMakerAndCheckerDTO>();


        public FeesMakerAndCheckerDTO getdetails(FeesMakerAndCheckerDTO data)
        {
            return COMMM.POSTDatafee(data, "FeesMakerAndCheckerFacade/getdetails/");
        }

        public FeesMakerAndCheckerDTO Getreportdetails(FeesMakerAndCheckerDTO data)
        {
            return COMMM.POSTDatafee(data, "FeesMakerAndCheckerFacade/Getreportdetails/");


        }
        public FeesMakerAndCheckerDTO savedetails(FeesMakerAndCheckerDTO data)
        {
            return COMMM.POSTDatafee(data, "FeesMakerAndCheckerFacade/savedetails/");


        }
    }
}
