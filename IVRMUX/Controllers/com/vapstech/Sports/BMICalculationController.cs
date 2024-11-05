using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Sports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Sports;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Sports
{
    [Route("api/[controller]")]
    public class BMICalculationController : Controller
    {
        BMICalculationDelegate delegat = new BMICalculationDelegate();

        [Route("loadgrid/{id:int}")]
        public BMICalculationDTO getDetails(int id)
        {
            BMICalculationDTO dto = new BMICalculationDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return delegat.getDetails(dto);
        }
        [Route("get_section")]
        public BMICalculationDTO get_section([FromBody]BMICalculationDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return delegat.get_section(data);
        }
        [Route("getStudents")]
        public BMICalculationDTO getStudents([FromBody]BMICalculationDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return delegat.getStudents(data);
        }
        [Route("saveRecord")]
        public BMICalculationDTO saveRecord([FromBody]BMICalculationDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
             data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return delegat.save(data);
        }
        [Route("deactivate")]
        public BMICalculationDTO deactivate([FromBody]BMICalculationDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return delegat.deactivate(data);
        }
        [Route("editdata")]
        public BMICalculationDTO editdata([FromBody]BMICalculationDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return delegat.editdata(data);
        }
        [Route("get_classes")]
        public BMICalculationDTO get_classes([FromBody]BMICalculationDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return delegat.get_classes(data);
        }
        [Route("filterStudeDateWise")]
        public BMICalculationDTO filterStudeDateWise([FromBody]BMICalculationDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return delegat.filterStudeDateWise(data);
        }
        
    }
}
