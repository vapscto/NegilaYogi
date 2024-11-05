using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.VisitorsManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VisitorsManagement;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.VisitorsManagement
{
    [Route("api/[controller]")]
    public class StaffGatePassController : Controller
    {
        // GET: api/<controller>
        StaffGatePassDelegate _objdel = new StaffGatePassDelegate();

        [Route("Getdetails/{id:int}")]
        public StaffGatePass_DTO Getdetails(int id)
        {
            StaffGatePass_DTO dto = new StaffGatePass_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return _objdel.Getdetails(dto);
        }
        [Route("getdepchange")]
        public StaffGatePass_DTO getdepchange([FromBody] StaffGatePass_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.getdepchange(dto);
        }
        [Route("get_staff1")]
        public StaffGatePass_DTO get_staff1([FromBody] StaffGatePass_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.get_staff1(dto);
        }
        [Route("saverecord")]
        public StaffGatePass_DTO saverecord([FromBody] StaffGatePass_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id= Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _objdel.saverecord(dto);
        }

        [Route("editrecord")]
        public StaffGatePass_DTO editrecord([FromBody] StaffGatePass_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.editrecord(dto);
        }
        
        [Route("deactive")]
        public StaffGatePass_DTO deactive([FromBody] StaffGatePass_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.deactive(dto);
        }
        
        [Route("PrintGatePass")]
        public StaffGatePass_DTO PrintGatePass([FromBody] StaffGatePass_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.PrintGatePass(dto);
        }
    }
}