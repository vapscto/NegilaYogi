using PreadmissionDTOs.com.vaps.Placement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlacementServiceHub.com.Interfaces
{
    public interface PlacementJobScheduleTitleInterface
    {
        PlacementJobScheduleTitleDTO loaddata(PlacementJobScheduleTitleDTO data);
        PlacementJobScheduleTitleDTO savedetails(PlacementJobScheduleTitleDTO data);
        PlacementJobScheduleTitleDTO editdetails(PlacementJobScheduleTitleDTO data);
        PlacementJobScheduleTitleDTO deactive(PlacementJobScheduleTitleDTO data);
        PlacementJobScheduleTitleDTO report(PlacementJobScheduleTitleDTO data);

    }
}
