using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRMSServicesHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.HRMS;
//using HRMSServiceHub.com.vaps.Interfaces;

namespace HRMSServicesHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class HRProcessConfigurationFacadeController : Controller
    {


        // GET: api/values
        public HRProcessConfigurationInterface _ads;

        public HRProcessConfigurationFacadeController(HRProcessConfigurationInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("getalldetails")]
        public HR_ProcessDTO getalldetails([FromBody]HR_ProcessDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        //[Route("getalldetails/{id:int}")]
        //public HR_ProcessDTO getalldetails(int id)
        //{
        //    HR_ProcessDTO dto = new HR_ProcessDTO();
        //    dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
        //    return hrdel.onloadgetdetails(dto);
        //}
        // POST api/values
        [HttpPost]
        [Route("savedata")]
        public HR_ProcessDTO savedata([FromBody]HR_ProcessDTO dto)
        {
            return _ads.SaveUpdate(dto);
        }
        [HttpGet]
        [Route("editRecord/{id:int}")]

        public HR_ProcessDTO getcatgrydet(int id)
        {
            // id = 12;
            return _ads.editData(id);
        }
        [Route("deactivateRecordById")]
        public HR_ProcessDTO deactivateRecordById(int id)
        {
            return _ads.deactivate(id);
        }

        [Route("deleteauth")]
        public HR_ProcessDTO deleteauth([FromBody]HR_ProcessDTO data)
        {
            return _ads.deleteauth(data);
        }

    }
}
