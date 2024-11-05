using CommonLibrary;
using PreadmissionDTOs.com.vaps.FrontOffice;
using PreadmissionDTOs.com.vaps.Portals.HOD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Portals.HOD
{
    public class HOD_PrincyDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HOD_DTO, HOD_DTO> COMFRNT = new CommonDelegate<HOD_DTO, HOD_DTO>();

        public HOD_DTO getalldetails(HOD_DTO data)
        {
            return COMFRNT.POSTPORTALData(data, "HOD_PRINCFacade/getalldetails/");
        }
        public HOD_DTO save(HOD_DTO data)
        {
            return COMFRNT.POSTPORTALData(data, "HOD_PRINCFacade/save/");
        }
        public HOD_DTO mappHOD(HOD_DTO data)
        {
            return COMFRNT.POSTPORTALData(data, "HOD_PRINCFacade/mappHOD/");
        }
        public HOD_DTO updateHOD(HOD_DTO data)
        {
            return COMFRNT.POSTPORTALData(data, "HOD_PRINCFacade/updateHOD/");
        }
        public HOD_DTO deactiveY(HOD_DTO data)
        {
            return COMFRNT.POSTPORTALData(data, "HOD_PRINCFacade/deactiveY/");
        }
    }
}
