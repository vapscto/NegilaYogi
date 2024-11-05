using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;


namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface MasterHeaderDetailsInterface
    {
        MasterHeaderDetailsDTO GetMasterHeaderDetailsData(MasterHeaderDetailsDTO data);

        MasterHeaderDetailsDTO SaveData(MasterHeaderDetailsDTO mas);

        MasterHeaderDetailsDTO DeleteEntry(int ID);

        MasterHeaderDetailsDTO GetSelectedRowDetails(MasterHeaderDetailsDTO dto);
        MasterHeaderDetailsDTO getmodulePage(MasterHeaderDetailsDTO dto);
    }
}
