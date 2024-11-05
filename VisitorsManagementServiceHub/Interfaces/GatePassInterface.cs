using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VisitorsManagementServiceHub.Interfaces
{
    public interface GatePassInterface
    {
        GatePassDTO getDetails(GatePassDTO data);
        GatePassDTO saveData(GatePassDTO data);
        GatePassDTO getStudentDetails(GatePassDTO data);
        GatePassDTO sendmail(GatePassDTO data);
    }
}
