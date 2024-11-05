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
    public class V_AppointmentApprovalStatusController : Controller
    {
        V_AppointmentApprovalStatusDelegate delobj = new V_AppointmentApprovalStatusDelegate();
        // GET: api/<controller>       

        [Route("getDetails/{id:int}")]
        public AppointmentApprovalStatus_DTO getDetails(int id)
        {
            AppointmentApprovalStatus_DTO dto = new AppointmentApprovalStatus_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.getDetails(dto);
        }

        [Route("Edit")]
        public AppointmentApprovalStatus_DTO Edit([FromBody] AppointmentApprovalStatus_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.EditDetails(dto);
        }
        [Route("Editnew")]
        public AppointmentApprovalStatus_DTO Editnew([FromBody] AppointmentApprovalStatus_DTO dto)
        {
            
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.Editnew(dto);
        }

        [Route("saveData")]
        public AppointmentApprovalStatus_DTO saveData([FromBody]AppointmentApprovalStatus_DTO data)
        {
           data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.saveData(data);
        }
        [Route("viewuploadflies")]
        public AppointmentApprovalStatus_DTO viewuploadflies([FromBody]AppointmentApprovalStatus_DTO data)
        {
           data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.viewuploadflies(data);
        }
        [Route("sendMOM")]
        public AppointmentApprovalStatus_DTO sendMOM([FromBody]AppointmentApprovalStatus_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.sendMOM(data);
        }

        [Route("getfeedback")]
        public AppointmentApprovalStatus_DTO getfeedback([FromBody]AppointmentApprovalStatus_DTO data)
        {
           data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.getfeedback(data);
        }

        [Route("savefeedback")]
        public AppointmentApprovalStatus_DTO savefeedback([FromBody]AppointmentApprovalStatus_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.savefeedback(data);
        }

    }
}
