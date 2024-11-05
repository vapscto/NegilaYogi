using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VisitorsManagementServiceHub.Interfaces
{
    public interface OutwardInterface
    {
        OutwardDTO getDetails(OutwardDTO data);
        OutwardDTO saveData(OutwardDTO data);
        OutwardDTO EditDetails(OutwardDTO id);
        OutwardDTO deactivate(OutwardDTO data);
    }
}
