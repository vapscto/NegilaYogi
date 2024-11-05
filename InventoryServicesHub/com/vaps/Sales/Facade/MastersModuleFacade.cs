using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryServicesHub.com.vaps.Sales.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InventoryServicesHub.com.vaps.Sales.Facade
{
    [Route("api/[controller]")]
    public class MastersModuleFacade : Controller
    {
        public MastersModuleInterface _objinter;

        public MastersModuleFacade(MastersModuleInterface parameter)
        {
            _objinter = parameter;
        }

        [Route("getdetails")]
        public MastersModule_DTO getdetails([FromBody] MastersModule_DTO dTO)
        {
            return _objinter.getdetails(dTO);
        }

        [Route("saverecord")]
        public MastersModule_DTO saverecord([FromBody]MastersModule_DTO value)
        {
            return _objinter.saverecord(value);
        }

        [Route("deactiveY")]
        public MastersModule_DTO deactiveY([FromBody]MastersModule_DTO value)
        {
            return _objinter.deactiveY(value);
        }

        [Route("get_emplist")]
        public MastersModule_DTO get_emplist([FromBody]MastersModule_DTO value)
        {
            return _objinter.get_emplist(value);
        }
        [Route("editlist")]
        public MastersModule_DTO editlist([FromBody]MastersModule_DTO value)
        {
            return _objinter.editlist(value);
        }

        [Route("get_MappedDeveloperlist")]
        public MastersModule_DTO get_MappedDeveloperlist([FromBody]MastersModule_DTO value)
        {
            return _objinter.get_MappedDeveloperlist(value);
        }

        [Route("deactiveDevpMappingdata")]
        public MastersModule_DTO deactiveDevpMappingdata([FromBody]MastersModule_DTO value)
        {
            return _objinter.deactiveDevpMappingdata(value);
        }

        //==================master task group mapping
        [Route("getdetails_taskgroup")]
        public ISM_Master_TaskGroup_DTO getdetails_taskgroup([FromBody]ISM_Master_TaskGroup_DTO value)
        {
            return _objinter.getdetails_taskgroup(value);
        }
        [Route("getdept")]
        public ISM_Master_TaskGroup_DTO getdept([FromBody]ISM_Master_TaskGroup_DTO value)
        {
            return _objinter.getdept(value);
        }

        [Route("show_tasklist")]
        public ISM_Master_TaskGroup_DTO show_tasklist([FromBody]ISM_Master_TaskGroup_DTO value)
        {
            return _objinter.show_tasklist(value);
        }
        [Route("save_taskgrpdata")]
        public ISM_Master_TaskGroup_DTO save_taskgrpdata([FromBody]ISM_Master_TaskGroup_DTO value)
        {
            return _objinter.save_taskgrpdata(value);
        }
        [Route("task_view")]
        public ISM_Master_TaskGroup_DTO task_view([FromBody]ISM_Master_TaskGroup_DTO value)
        {
            return _objinter.task_view(value);
        }
        [Route("task_edit")]
        public ISM_Master_TaskGroup_DTO task_edit([FromBody]ISM_Master_TaskGroup_DTO value)
        {
            return _objinter.task_edit(value);
        }
    }
}
