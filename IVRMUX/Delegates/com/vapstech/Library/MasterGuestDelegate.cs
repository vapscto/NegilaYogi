using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Library
{
    public class MasterGuestDelegate
    {
        CommonDelegate<MasterGuestDTO, MasterGuestDTO> _commbranch = new CommonDelegate<MasterGuestDTO, MasterGuestDTO>();
        public MasterGuestDTO Savedata(MasterGuestDTO data)
        {
            return _commbranch.PostLibrary(data, "MasterGuestFacade/Savedata");
        }

        public MasterGuestDTO getdetails(int id)
        {
            return _commbranch.GETLibrary(id, "MasterGuestFacade/getdetails/");
        }

        public MasterGuestDTO deactiveY(MasterGuestDTO data)
        {
            return _commbranch.PostLibrary(data, "MasterGuestFacade/deactiveY");
        }
    }
}
