using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Library
{
    public class MasterTimeSlabDelegate
    {
        CommonDelegate<MasterTimeSlabDTO, MasterTimeSlabDTO> _commbranch = new CommonDelegate<MasterTimeSlabDTO, MasterTimeSlabDTO>();
        public MasterTimeSlabDTO getdetails(int id)
        {
            return _commbranch.GETLibrary(id, "MasterTimeSlabFacade/getdetails/");
        }
        public MasterTimeSlabDTO Savedata(MasterTimeSlabDTO data)
        {
            return _commbranch.PostLibrary(data, "MasterTimeSlabFacade/Savedata/");
        }
        public MasterTimeSlabDTO deactiveY(MasterTimeSlabDTO data)
        {
            return _commbranch.PostLibrary(data, "MasterTimeSlabFacade/deactiveY/");
        }
    }
}
