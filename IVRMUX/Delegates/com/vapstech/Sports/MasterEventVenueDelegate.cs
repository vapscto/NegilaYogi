using CommonLibrary;
using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Sports
{
    public class MasterEventVenueDelegate
    {
        CommonDelegate<MasterEventVenueDTO, MasterEventVenueDTO> COMSPRT = new CommonDelegate<MasterEventVenueDTO, MasterEventVenueDTO>();

        public MasterEventVenueDTO getDetails(MasterEventVenueDTO data)
        {
            return COMSPRT.POSTDataSports(data, "MasterEventVenueFacade/getDetails/");
        }
        public MasterEventVenueDTO save(MasterEventVenueDTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "MasterEventVenueFacade/save/");
        }
        public MasterEventVenueDTO EditDetails(int id)
        {
            return COMSPRT.GetDataByIdSports(id, "MasterEventVenueFacade/EditDetails/");
        }
        public MasterEventVenueDTO deactivate(MasterEventVenueDTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "MasterEventVenueFacade/deactivate/");
        }
    }
}
