using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface GovernmentBondInterface
    {
        
        GovernmentBondDTO GovernmentBondData(GovernmentBondDTO mas);

        GovernmentBondDTO MasterDeleteModulesData(GovernmentBondDTO ID);

        GovernmentBondDTO GetSelectedRowDetails(int ID);

        GovernmentBondDTO GetGovernmentBondData(GovernmentBondDTO GovernmentBondDTO);

    }
}
