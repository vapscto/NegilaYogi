using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;

//PreadmissionDTOs.com.vaps.admission


namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface mastercasteInterface
    {
        mastercasteDTO mastercasteData(mastercasteDTO mas);

        mastercasteDTO MasterDeleteModulesData(int ID);

        mastercasteDTO GetSelectedRowDetails(int ID);

        mastercasteDTO GetmastercasteData(mastercasteDTO mastercasteDTO);
    }
}
