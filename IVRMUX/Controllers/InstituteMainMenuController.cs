using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using DomainModel.Model;
using PreadmissionDTOs;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class InstituteMainMenuController : Controller
    {
        // GET: /<controller>/
        InstituteMainMenuDelegates InstituteMainMenudelStr = new InstituteMainMenuDelegates();

        //// GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [Route("Getdetails/")]
        public InstituteMainMenuDTO Getdetails(InstituteMainMenuDTO InstituteMainMenuDTO)
        {
            int roleidd = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            InstituteMainMenuDTO.roleId = roleidd;
            int MIID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            InstituteMainMenuDTO.MI_Id= MIID;
            return InstituteMainMenudelStr.GetMasterMainMenuData(InstituteMainMenuDTO);
            
        }

        [Route("GetSelectedRowdetails/{id:int}")]
        public InstituteMainMenuDTO GetSelectedRowDetails(int ID)
        {
            return InstituteMainMenudelStr.GetSelectedRowDetails(ID);
        }
       

        
        [Route("getMenudetailsByModuleId")]
        public InstituteMainMenuDTO getMenudetailsByModuleId([FromBody]InstituteMainMenuDTO data)
        {
           // InstituteMainMenuDTO data = new PreadmissionDTOs.InstituteMainMenuDTO();

            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.IVRMM_Id = ID;
            return InstituteMainMenudelStr.getMenudetailsByModuleId(data);
        }

        [HttpPost]
        public InstituteMainMenuDTO InstituteMainMenuDTO([FromBody] InstituteMainMenuDTO MMD)
        {
           
            //int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //MMD.MI_Id = mid;

            return InstituteMainMenudelStr.MasterMainMenuDTO(MMD);
        }

        [Route("getmoduledetails/")]
        public InstituteMainMenuDTO getmoduledetails([FromBody] InstituteMainMenuDTO data)
        {
            //InstituteMainMenuDTO data = new PreadmissionDTOs.InstituteMainMenuDTO();

            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            data = InstituteMainMenudelStr.getmoduledetails(data);
            return data;
        }

        [HttpDelete]
        [Route("MasterDeleteMainMenuDTO/{id:int}")]
        public InstituteMainMenuDTO MasterDeleteMainMenuDTO(int ID)
        {
            return InstituteMainMenudelStr.MasterDeleteMainMenuDTO(ID);
        }

        [Route("validateordernumber")]
        public InstituteMainMenuDTO validateordernumber([FromBody]InstituteMainMenuDTO dto)
        {
            // HR_Master_GroupTypeDTO dto = new HR_Master_GroupTypeDTO();
            // dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return InstituteMainMenudelStr.Onchangedetails(dto);
        }


    }

}
