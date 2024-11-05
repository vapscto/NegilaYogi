using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Library
{
    public class MasterDonorDelegate
    {
        CommonDelegate<MasterDonorDTO, MasterDonorDTO> _commbranch = new CommonDelegate<MasterDonorDTO, MasterDonorDTO>();
        public MasterDonorDTO Savedata(MasterDonorDTO data)
        {
            return _commbranch.PostLibrary(data, "MasterDonorFacade/Savedata");
        }

        public MasterDonorDTO getdetails(int id)
        {
            return _commbranch.GETLibrary(id, "MasterDonorFacade/getdetails/");
        }

        public MasterDonorDTO deactiveY(MasterDonorDTO data)
        {
            return _commbranch.PostLibrary(data, "MasterDonorFacade/deactiveY");
        }
    }
}
