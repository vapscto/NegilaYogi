
using System;
using PreadmissionDTOs.com.vaps.Portals.Principal;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.Employee;

namespace corewebapi18072016.Delegates
{
    public class CareerReportDelegates
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<CareerReportDTO, CareerReportDTO> COMMM = new CommonDelegate<CareerReportDTO, CareerReportDTO>();
        CommonDelegate<IVRM_Homework_DTO, IVRM_Homework_DTO> COMHC = new CommonDelegate<IVRM_Homework_DTO, IVRM_Homework_DTO>();
        public CareerReportDTO getalldetails(CareerReportDTO data)
        {
            return COMMM.POSTPORTALData(data, "CareerReportFacade/getalldetails/");
        }

        //==========================home/class work upload
        public IVRM_Homework_DTO get_home_classwork(IVRM_Homework_DTO data)
        {
            return COMHC.POSTPORTALData(data, "CareerReportFacade/get_home_classwork/");
        }

//          
  }
}
