using CommonLibrary;
using IVRMUX.Controllers.com.vapstech.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Library;

namespace IVRMUX.Delegates.com.vapstech.Library
{
    public class MasterPublisherDelegate
    {
        CommonDelegate<MasterPublisherDTO, MasterPublisherDTO> _commbranch = new CommonDelegate<MasterPublisherDTO, MasterPublisherDTO>();
        public MasterPublisherDTO Savedata(MasterPublisherDTO data)
        {
            return _commbranch.PostLibrary(data, "MasterPublisherFacade/Savedata");
        }

        public MasterPublisherDTO getdetails(int id)
        {
           return _commbranch.GETLibrary(id, "MasterPublisherFacade/getdetails/");
        }
        
        public MasterPublisherDTO deactiveY(MasterPublisherDTO data)
        {
            return _commbranch.PostLibrary(data, "MasterPublisherFacade/deactiveY");
        }
    }
}
