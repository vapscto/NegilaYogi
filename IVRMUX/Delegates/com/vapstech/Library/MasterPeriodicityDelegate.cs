using System;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Library
{
    public class MasterPeriodicityDelegate
    {
        CommonDelegate<MasterPeriodicityDTO, MasterPeriodicityDTO> _commbranch = new CommonDelegate<MasterPeriodicityDTO, MasterPeriodicityDTO>();
        public MasterPeriodicityDTO Savedata(MasterPeriodicityDTO data)
        {
            return _commbranch.PostLibrary(data, "MasterPeriodicityFacade/Savedata/");
        }
        public MasterPeriodicityDTO getdetails(int id)
        {
            return _commbranch.GETLibrary(id, "MasterPeriodicityFacade/getdetails/");
        }
        public MasterPeriodicityDTO deactiveY(MasterPeriodicityDTO data)
        {
            return _commbranch.PostLibrary(data, "MasterPeriodicityFacade/deactiveY/");
        }
    }
}
