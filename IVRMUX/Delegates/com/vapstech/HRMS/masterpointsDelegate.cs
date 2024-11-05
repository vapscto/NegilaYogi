using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class masterpointsDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Employee_AssesmentpointsDTO, HR_Employee_AssesmentpointsDTO> COMMM = new CommonDelegate<HR_Employee_AssesmentpointsDTO, HR_Employee_AssesmentpointsDTO>();

        public HR_Employee_AssesmentpointsDTO onloadgetdetails(HR_Employee_AssesmentpointsDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "masterpointsFacade/onloadgetdetails");
        }

        public HR_Employee_AssesmentpointsDTO savedetails(HR_Employee_AssesmentpointsDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "masterpointsFacade/");
        }
        public HR_Employee_AssesmentpointsDTO getRecorddetailsById(int id)
        {
            return COMMM.GetDataByIdHRMS(id, "masterpointsFacade/getRecordById/");
        }
        public HR_Employee_AssesmentpointsDTO deleterec(HR_Employee_AssesmentpointsDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "masterpointsFacade/deactivateRecordById/");
        }

    }
}
