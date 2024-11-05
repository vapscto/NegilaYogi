using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class HRProcessConfigurationDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_ProcessDTO, HR_ProcessDTO> COMMM = new CommonDelegate<HR_ProcessDTO, HR_ProcessDTO>();

        public HR_ProcessDTO onloadgetdetails(HR_ProcessDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "HRProcessConfigurationFacade/getalldetails");
        }


        public HR_ProcessDTO savedetails(HR_ProcessDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "HRProcessConfigurationFacade/savedata");

        }


        public HR_ProcessDTO getRecorddetailsById(int id)
        {
            return COMMM.GetDataByIdHRMS(id, "HRProcessConfigurationFacade/editRecord/");
        }



        public HR_ProcessDTO deleterec(int id)
        {
            return COMMM.GetDataByIdHRMS(id, "HRProcessConfigurationFacade/ActiveDeactiveRecord");
        }

        public HR_ProcessDTO deleteauth(HR_ProcessDTO data)
        {
            return COMMM.POSTDataHRMS(data, "HRProcessConfigurationFacade/deleteauth");

        }
    }
}
