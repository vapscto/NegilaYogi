using PreadmissionDTOs.com.vaps.Placement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlacementServiceHub.com.Interfaces
{
    public interface PL_CI_StudentStatusInterface
    {
        PL_CI_StudentStatusDTO loaddata(PL_CI_StudentStatusDTO data);
        PL_CI_StudentStatusDTO savedetails(PL_CI_StudentStatusDTO data);
        PL_CI_StudentStatusDTO editdetails(PL_CI_StudentStatusDTO data);
        PL_CI_StudentStatusDTO deactive(PL_CI_StudentStatusDTO data);
    }
}
