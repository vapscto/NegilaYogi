using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;


namespace WebApplication1.Interfaces
{
    public interface StudentMasterConfigurationInterface
    {
        Task<CommonDTO> getRecord(CommonDTO data);
        MasterConfigurationDTO saveMasterConfig(MasterConfigurationDTO mstConfigData);
        MasterConfigurationDTO getMasterEditData(int id);
        MasterConfigurationDTO deleteRecord(MasterConfigurationDTO data);
    }
}
