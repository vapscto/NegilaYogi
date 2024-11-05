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
    public class VisitorAppointmentController : Controller
    {
        VisitorAppointmentDelegate delobj = new VisitorAppointmentDelegate();

        [Route("getDetails/{id:int}")]
        public Visitor_Management_Appointment_DTO getDetails(int id)
        {
            Visitor_Management_Appointment_DTO dto = new Visitor_Management_Appointment_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.getDetails(dto);
        }

        [Route("EditDetails")]
        public Visitor_Management_Appointment_DTO EditDetails([FromBody]Visitor_Management_Appointment_DTO dto)
        {
            // Visitor_Management_Appointment_DTO dto = new Visitor_Management_Appointment_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.EditDetails(dto);
        }

        [HttpPost]
        [Route("saveData")]
        public Visitor_Management_Appointment_DTO saveData([FromBody]Visitor_Management_Appointment_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.saveData(data);
        }

        [Route("deactivate")]
        public Visitor_Management_Appointment_DTO deactivate([FromBody] Visitor_Management_Appointment_DTO d)
        {
            d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.deactivate(d);
        }
        [Route("viewuploadflies")]
        public Visitor_Management_Appointment_DTO viewuploadflies([FromBody] Visitor_Management_Appointment_DTO d)
        {
            d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.viewuploadflies(d);
        }
        [Route("deleteuploadfile")]
        public Visitor_Management_Appointment_DTO deleteuploadfile([FromBody] Visitor_Management_Appointment_DTO d)
        {
            d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.deleteuploadfile(d);
        }

    }
}
