using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class MasterQuarterDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Master_QuarterDTO, HR_Master_QuarterDTO> COMMM = new CommonDelegate<HR_Master_QuarterDTO, HR_Master_QuarterDTO>();

        public HR_Master_QuarterDTO onloadgetdetails(HR_Master_QuarterDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "MasterQuarterFacade/onloadgetdetails");
        }

        public HR_Master_QuarterDTO savedetails(HR_Master_QuarterDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "MasterQuarterFacade/");
        }
        public HR_Master_QuarterDTO getRecorddetailsById(int id)
        {
            return COMMM.GetDataByIdHRMS(id, "MasterQuarterFacade/getRecordById/");
        }
        public HR_Master_QuarterDTO deleterec(HR_Master_QuarterDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "MasterQuarterFacade/deactivateRecordById/");
        }

    }
}
