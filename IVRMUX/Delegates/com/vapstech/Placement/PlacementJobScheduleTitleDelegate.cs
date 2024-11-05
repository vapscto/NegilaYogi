using CommonLibrary;
using PreadmissionDTOs.com.vaps.Placement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Placement
{
    public class PlacementJobScheduleTitleDelegate
    {
        CommonDelegate<PlacementJobScheduleTitleDTO, PlacementJobScheduleTitleDTO> _comm = new CommonDelegate<PlacementJobScheduleTitleDTO, PlacementJobScheduleTitleDTO>();
        public PlacementJobScheduleTitleDTO loaddata(PlacementJobScheduleTitleDTO data)
        {
            return _comm.POSTDataPlacement(data, "PlacementJobScheduleTitleFacade/loaddata/");
        }

        public PlacementJobScheduleTitleDTO savedetails(PlacementJobScheduleTitleDTO data)
        {
            return _comm.POSTDataPlacement(data, "PlacementJobScheduleTitleFacade/savedetails/");
        }

        public PlacementJobScheduleTitleDTO editdetails(PlacementJobScheduleTitleDTO data)
        {
            return _comm.POSTDataPlacement(data, "PlacementJobScheduleTitleFacade/editdetails");
        }
        public PlacementJobScheduleTitleDTO deactive(PlacementJobScheduleTitleDTO data)
        {
            return _comm.POSTDataPlacement(data, "PlacementJobScheduleTitleFacade/deactive/");
        }
        public PlacementJobScheduleTitleDTO report(PlacementJobScheduleTitleDTO data)
        {
            return _comm.POSTDataPlacement(data, "PlacementJobScheduleTitleFacade/report/");
        }
    }
}
