using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class ECRReportDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ECRDTO, ECRDTO> COMMM = new CommonDelegate<ECRDTO, ECRDTO>();


        public ECRDTO onloadgetdetails(ECRDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "ECRFacade/onloadgetdetails");
        }

        public ECRDTO SaveData(ECRDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "ECRFacade/SaveData");
        }

        public ECRDTO GetEmpDetails(ECRDTO data)
        {
            return COMMM.POSTDataHRMS(data, "ECRFacade/GetEmpDetails");
        }
        public ECRDTO getEmployeedetailsBySelection(ECRDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "ECRFacade/getEmployeedetailsBySelection/");
        }

        public ECRDTO get_depts(ECRDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "ECRFacade/get_depts/");
        }
        public ECRDTO get_desig(ECRDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "ECRFacade/get_desig/");
        }
    }
}
