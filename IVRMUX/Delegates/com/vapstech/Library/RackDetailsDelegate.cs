using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Library
{
    public class RackDetailsDelegate
    {
        CommonDelegate<RackDetailsDTO, RackDetailsDTO> _commbranch = new CommonDelegate<RackDetailsDTO, RackDetailsDTO>();

        public RackDetailsDTO getdetails(int id)
        {
            return _commbranch.GETLibrary(id, "RackDetailsFacade/getdetails/");
        }
        public RackDetailsDTO Savedata(RackDetailsDTO data)
        {
            return _commbranch.PostLibrary(data, "RackDetailsFacade/Savedata/");
        }
        public RackDetailsDTO EditData(RackDetailsDTO data)
        {
            return _commbranch.PostLibrary(data, "RackDetailsFacade/EditData/");
        }
        public RackDetailsDTO deactiveY(RackDetailsDTO data)
        {
            return _commbranch.PostLibrary(data, "RackDetailsFacade/deactiveY/");
        }
    }
}
