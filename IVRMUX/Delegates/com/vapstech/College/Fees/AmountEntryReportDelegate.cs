using System;
using PreadmissionDTOs.com.vaps.College.Fees;
using CommonLibrary;
namespace IVRMUX.Delegates.com.vapstech.College.Fees
{
    public class AmountEntryReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<CollegeConcessionDTO, CollegeConcessionDTO> COMMM = new CommonDelegate<CollegeConcessionDTO, CollegeConcessionDTO>();
        public CollegeConcessionDTO getdetails(CollegeConcessionDTO data)
        {

            return COMMM.POSTDataCollfee(data, "AmountEntryReportFacade/getdetails/");

        }

        public CollegeConcessionDTO get_courses(CollegeConcessionDTO data)
        {
            return COMMM.POSTDataCollfee(data, "AmountEntryReportFacade/get_courses/");

        }


        public CollegeConcessionDTO get_branches(CollegeConcessionDTO data)
        {
            return COMMM.POSTDataCollfee(data, "AmountEntryReportFacade/get_branches/");

        }
        public CollegeConcessionDTO radiobtndata(CollegeConcessionDTO data)
        {
            return COMMM.POSTDataCollfee(data, "AmountEntryReportFacade/radiobtndata/");

        }
    }
}
