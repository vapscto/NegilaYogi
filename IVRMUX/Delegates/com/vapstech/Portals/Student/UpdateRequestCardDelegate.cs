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
    public class UpdateRequestDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<UpdateRequestDTO, UpdateRequestDTO> COMMM = new CommonDelegate<UpdateRequestDTO, UpdateRequestDTO>();
        public UpdateRequestDTO getloaddata(UpdateRequestDTO data)
        {     
            return COMMM.POSTPORTALData(data, "UpdateRequestFacade/getloaddata/");
        }
        public UpdateRequestDTO getreploaddata(UpdateRequestDTO data)
        {     
            return COMMM.POSTPORTALData(data, "UpdateRequestFacade/getreploaddata/");
        }

        public UpdateRequestDTO getstudata(UpdateRequestDTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "UpdateRequestFacade/getstudata/");
        }
        public UpdateRequestDTO getreport(UpdateRequestDTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "UpdateRequestFacade/getreport/");
        }
        public UpdateRequestDTO saverequest(UpdateRequestDTO data)
        {
            return COMMM.POSTPORTALData(data, "UpdateRequestFacade/saverequest/");
        }

        public UpdateRequestDTO savedataadmin(UpdateRequestDTO data)
        {
            return COMMM.POSTPORTALData(data, "UpdateRequestFacade/savedataadmin/");
        }
        public UpdateRequestDTO savereject(UpdateRequestDTO data)
        {
            return COMMM.POSTPORTALData(data, "UpdateRequestFacade/savereject/");
        }
        public UpdateRequestDTO searchfilter(UpdateRequestDTO sddto)
        {
            return COMMM.POSTPORTALData(sddto, "UpdateRequestFacade/searchfilter/");
        }

        public UpdateRequestDTO guardianDetails(UpdateRequestDTO data)
        {
            return COMMM.POSTPORTALData(data, "UpdateRequestFacade/guardianDetails/");
        }

        public UpdateRequestDTO getStateByCountry(int id)
        {
            return COMMM.GETPORTALData(id, "UpdateRequestFacade/getdpstate/");
        }

    }
}
