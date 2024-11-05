using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.VMS.HRMS
{
    public class masterPriorityDelegates
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Master_PriorityDTO, HR_Master_PriorityDTO> COMMM = new CommonDelegate<HR_Master_PriorityDTO, HR_Master_PriorityDTO>();

        public HR_Master_PriorityDTO onloadgetdetails(HR_Master_PriorityDTO dto)
        {
            return COMMM.POSTVMS(dto, "masterPriorityFacade/onloadgetdetails");
        }

        public HR_Master_PriorityDTO savedetails(HR_Master_PriorityDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "masterPriorityFacade/");
        }
        public HR_Master_PriorityDTO getdata(HR_Master_PriorityDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "masterPriorityFacade/getdata");
        }
        public HR_Master_PriorityDTO getRecorddetailsById(int id)
        {
            return COMMM.GetVMS(id, "masterPriorityFacade/getRecordById/");
        }
        public HR_Master_PriorityDTO deleterec(HR_Master_PriorityDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "masterPriorityFacade/deactivateRecordById/");
        }


    }
}
