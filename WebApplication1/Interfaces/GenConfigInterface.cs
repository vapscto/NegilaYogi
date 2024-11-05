using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interfaces
{
    public interface GenConfigInterface
    {
        GeneralConfigDTO Configurationget(GeneralConfigDTO data);
        GeneralConfigDTO geteditdata(GeneralConfigDTO data);
        GeneralConfigDTO saveMasterConfig(GeneralConfigDTO mstConfigData);
        GeneralConfigDTO getcontent(GeneralConfigDTO mstConfigData);
        GeneralConfigDTO deleteUserNameconfig(GeneralConfigDTO id);
    }
}
