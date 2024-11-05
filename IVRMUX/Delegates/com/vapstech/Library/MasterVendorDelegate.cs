using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Library
{
    public class MasterVendorDelegate
    {

        CommonDelegate<MasterVendorDTO, MasterVendorDTO> _commbranch = new CommonDelegate<MasterVendorDTO, MasterVendorDTO>();
        public MasterVendorDTO Savedata(MasterVendorDTO data)
        {
            return _commbranch.PostLibrary(data, "MasterVendorFacade/Savedata");
        }

        public MasterVendorDTO getdetails(int id)
        {
            return _commbranch.GETLibrary(id, "MasterVendorFacade/getdetails/");
        }

        public MasterVendorDTO deactiveY(MasterVendorDTO data)
        {
            return _commbranch.PostLibrary(data, "MasterVendorFacade/deactiveY");
        }

    }
}
