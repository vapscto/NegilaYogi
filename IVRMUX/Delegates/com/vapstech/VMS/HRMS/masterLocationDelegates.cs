using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.VMS.HRMS
{
    public class masterLocationDelegates
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Master_LocationDTO, HR_Master_LocationDTO> COMMM = new CommonDelegate<HR_Master_LocationDTO, HR_Master_LocationDTO>();

        public HR_Master_LocationDTO onloadgetdetails(HR_Master_LocationDTO dto)
        {
            return COMMM.POSTVMS(dto, "MasterLocationFacade/onloadgetdetails");
        }

        public HR_Master_LocationDTO savedetails(HR_Master_LocationDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "MasterLocationFacade/");

        }
        public HR_Master_LocationDTO getdata(HR_Master_LocationDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "MasterLocationFacade/getdata");
        }
        public HR_Master_LocationDTO getRecorddetailsById(int id)
        {
            return COMMM.GetVMS(id, "MasterLocationFacade/getRecordById/");
        }
        public HR_Master_LocationDTO deleterec(HR_Master_LocationDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "MasterLocationFacade/deactivateRecordById/");
        }


    }
}
