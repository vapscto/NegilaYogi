using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;

//PreadmissionDTOs.com.vaps.admission


namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface MasterActivityInterface
    {
        MasterActivityDTO MasterActivityData(MasterActivityDTO mas);     

        MasterActivityDTO MasterDeleteModulesData(int ID);

        MasterActivityDTO GetSelectedRowDetails(int ID);

        MasterActivityDTO GetMasterActivityData(MasterActivityDTO MasterActivityDTO);
    }
}
