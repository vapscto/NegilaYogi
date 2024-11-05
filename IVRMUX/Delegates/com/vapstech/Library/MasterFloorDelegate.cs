using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Library
{
    public class MasterFloorDelegate
    {
        CommonDelegate<MasterFloorDTO, MasterFloorDTO> _commbranch = new CommonDelegate<MasterFloorDTO, MasterFloorDTO>();
        public MasterFloorDTO Savedata(MasterFloorDTO data)
        {
            return _commbranch.PostLibrary(data, "MasterFloorFacade/Savedata/");
        }
        public MasterFloorDTO getdetails(int id)
        {
            return _commbranch.GETLibrary(id, "MasterFloorFacade/getdetails/");
        }
        public MasterFloorDTO deactiveY(MasterFloorDTO data)
        {
            return _commbranch.PostLibrary(data, "MasterFloorFacade/deactiveY/");
        }
    }
}
