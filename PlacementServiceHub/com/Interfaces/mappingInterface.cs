using PreadmissionDTOs.com.vaps.Placement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlacementServiceHub.com.Interfaces
{
    public interface mappingInterface
    {
        mappingDTO loaddata(int dto);
        mappingDTO savedata(mappingDTO data);
        mappingDTO edit(mappingDTO data);
        mappingDTO deactive(mappingDTO data);
    }
}
