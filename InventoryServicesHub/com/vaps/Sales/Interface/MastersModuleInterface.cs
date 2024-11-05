using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Sales.Interface
{
    public interface MastersModuleInterface
    {
        MastersModule_DTO getdetails(MastersModule_DTO dTO);
        MastersModule_DTO saverecord(MastersModule_DTO data);
        MastersModule_DTO deactiveY(MastersModule_DTO data);
        MastersModule_DTO get_emplist(MastersModule_DTO data);
        MastersModule_DTO editlist(MastersModule_DTO data);
        MastersModule_DTO get_MappedDeveloperlist(MastersModule_DTO data);
        MastersModule_DTO deactiveDevpMappingdata(MastersModule_DTO data);

        //==================master task group mapping
        ISM_Master_TaskGroup_DTO getdetails_taskgroup(ISM_Master_TaskGroup_DTO data);
        ISM_Master_TaskGroup_DTO getdept(ISM_Master_TaskGroup_DTO data);
        ISM_Master_TaskGroup_DTO show_tasklist(ISM_Master_TaskGroup_DTO data);
        ISM_Master_TaskGroup_DTO save_taskgrpdata(ISM_Master_TaskGroup_DTO data);
        ISM_Master_TaskGroup_DTO task_view(ISM_Master_TaskGroup_DTO data);
        ISM_Master_TaskGroup_DTO task_edit(ISM_Master_TaskGroup_DTO data);
    }
}
