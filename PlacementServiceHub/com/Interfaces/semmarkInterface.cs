using PreadmissionDTOs.com.vaps.Placement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlacementServiceHub.com.Interfaces
{
    public interface semmarkInterface
    {
        semmarkDTO loaddata(int dto);
        semmarkDTO savedata(semmarkDTO data);
        semmarkDTO edit(semmarkDTO data);
        semmarkDTO deactive(semmarkDTO data);

    }
}
