using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Sales.Interface
{
    public interface ISM_ClientProject_MappingInterface
    {
        ISM_ClientProject_MappingDTO loaddata(ISM_ClientProject_MappingDTO data);
        ISM_ClientProject_MappingDTO savedata(ISM_ClientProject_MappingDTO data);
        ISM_ClientProject_MappingDTO Editdata(ISM_ClientProject_MappingDTO data);
        ISM_ClientProject_MappingDTO getproject(ISM_ClientProject_MappingDTO data);
        ISM_ClientProject_MappingDTO getmodule(ISM_ClientProject_MappingDTO data);
        ISM_ClientProject_MappingDTO clientDecative(ISM_ClientProject_MappingDTO data);
    }
}
