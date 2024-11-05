using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Portals.Student;
using CommonLibrary;

namespace corewebapi18072016.Delegates.com.vapstech.Portals.Student
{
    public class ParentsSmartCardDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ParentSmartCardDTO, ParentSmartCardDTO> COMMM = new CommonDelegate<ParentSmartCardDTO, ParentSmartCardDTO>();
        public ParentSmartCardDTO getloaddata(ParentSmartCardDTO data)
        {     
            return COMMM.POSTPORTALData(data, "ParentsSmartCardFacade/getloaddata/");
        }

        public ParentSmartCardDTO getstudata(ParentSmartCardDTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "ParentsSmartCardFacade/getstudata/");
        }
        public ParentSmartCardDTO savedata(ParentSmartCardDTO data)
        {
            return COMMM.POSTPORTALData(data, "ParentsSmartCardFacade/savedata/");
        }

        public ParentSmartCardDTO savedataadmin(ParentSmartCardDTO data)
        {
            return COMMM.POSTPORTALData(data, "ParentsSmartCardFacade/savedataadmin/");
        }
        public ParentSmartCardDTO searchfilter(ParentSmartCardDTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "ParentsSmartCardFacade/searchfilter/");
        }

        public ParentSmartCardDTO guardianDetails(ParentSmartCardDTO data)
        {
            return COMMM.POSTPORTALData(data, "ParentsSmartCardFacade/guardianDetails/");
        }
        public ParentSmartCardDTO getreport(ParentSmartCardDTO data)
        {
            return COMMM.POSTPORTALData(data, "ParentsSmartCardFacade/getreport/");
        }

        public ParentSmartCardDTO getStateByCountry(int id)
        {
            return COMMM.GETPORTALData(id, "ParentsSmartCardFacade/getdpstate/");
        }

    }
}
