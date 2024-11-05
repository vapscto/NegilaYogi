using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Inventory.Sales;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;

namespace IVRMUX.Controllers.com.vapstech.Inventory.Sales
{
    [Produces("application/json")]
    [Route("api/MastersModule")]
    public class MastersModuleController : Controller
    {
        public MastersModuleDelegate _objdel = new MastersModuleDelegate();

        [Route("getdetails/{id:int}")]
        public MastersModule_DTO getdetails(int id)
        {
            MastersModule_DTO dTO = new MastersModule_DTO();
            dTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return _objdel.getdetails(dTO);
        }

        [Route("saverecord")]
        public MastersModule_DTO saverecord([FromBody]MastersModule_DTO value)
        {
            value.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.saverecord(value);
        }

        [Route("deactiveY")]
        public MastersModule_DTO deactiveY([FromBody]MastersModule_DTO value)
        {
            value.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.deactiveY(value);
        }
        [Route("get_emplist")]
        public MastersModule_DTO get_emplist([FromBody]MastersModule_DTO value)
        {
            value.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.get_emplist(value);
        }

        [Route("editlist")]
        public MastersModule_DTO editlist([FromBody]MastersModule_DTO value)
        {
            value.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.editlist(value);
        }

        [Route("get_MappedDeveloperlist")]
        public MastersModule_DTO get_MappedDeveloperlist([FromBody]MastersModule_DTO value)
        {
            value.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.get_MappedDeveloperlist(value);
        }

        [Route("deactiveDevpMappingdata")]
        public MastersModule_DTO deactiveDevpMappingdata([FromBody]MastersModule_DTO value)
        {
            value.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.deactiveDevpMappingdata(value);
        }

        //==================master task group mapping
        [Route("getdetails_taskgroup/{id:int}")]
        public ISM_Master_TaskGroup_DTO getdetails_taskgroup(int id)
        {
            ISM_Master_TaskGroup_DTO dto = new ISM_Master_TaskGroup_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getdetails_taskgroup(dto);
        }

        [Route("getdept")]
        public ISM_Master_TaskGroup_DTO getdept([FromBody] ISM_Master_TaskGroup_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getdept(dto);
        }
        [Route("get_task")]
        public ISM_Master_TaskGroup_DTO get_task([FromBody] ISM_Master_TaskGroup_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.get_task(dto);
        }
        [Route("show_tasklist")]
        public ISM_Master_TaskGroup_DTO show_tasklist([FromBody] ISM_Master_TaskGroup_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.show_tasklist(dto);
        }
        [Route("save_taskgrpdata")]
        public ISM_Master_TaskGroup_DTO save_taskgrpdata([FromBody] ISM_Master_TaskGroup_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.save_taskgrpdata(dto);
        }
        [Route("task_view")]
        public ISM_Master_TaskGroup_DTO task_view([FromBody] ISM_Master_TaskGroup_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.task_view(dto);
        }
        [Route("task_edit")]
        public ISM_Master_TaskGroup_DTO task_edit([FromBody] ISM_Master_TaskGroup_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.task_edit(dto);
        }
    }
}