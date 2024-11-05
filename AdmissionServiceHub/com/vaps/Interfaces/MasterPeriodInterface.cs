using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;


namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface MasterPeriodInterface
    {
        MasterPeriodDTO GetMasterPeriodData(MasterPeriodDTO MasterPeriodDTO);

        MasterPeriodDTO SaveData(MasterPeriodDTO mas);

        MasterPeriodDTO DeleteEntry(int ID);

        MasterPeriodDTO GetSelectedRowDetails(MasterPeriodDTO dto);
    }
}
