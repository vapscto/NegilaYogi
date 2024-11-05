using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using VisitorsManagementServiceHub.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VisitorsManagementServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class VisitorAppointmentFacade : Controller
    {
        // GET: api/<controller>
        VisitorAppointmentInterface interobj;
        public VisitorAppointmentFacade(VisitorAppointmentInterface par)
        {
            interobj = par;
        }

        [HttpPost]
        [Route("getDetails")]
        public Visitor_Management_Appointment_DTO getDetails([FromBody]Visitor_Management_Appointment_DTO data)
        {
            return interobj.getDetails(data);
        }
        [Route("EditDetails")]
        public Visitor_Management_Appointment_DTO EditDetails([FromBody] Visitor_Management_Appointment_DTO id)
        {
            return interobj.EditDetails(id);
        }
        [Route("saveData")]
        public Visitor_Management_Appointment_DTO saveData([FromBody]Visitor_Management_Appointment_DTO data)
        {
            return interobj.saveData(data);
        }
        [Route("deactivate")]
        public Visitor_Management_Appointment_DTO deactivate([FromBody]Visitor_Management_Appointment_DTO data)
        {
            return interobj.deactivate(data);
        }
        [Route("viewuploadflies")]
        public Visitor_Management_Appointment_DTO viewuploadflies([FromBody]Visitor_Management_Appointment_DTO data)
        {
            return interobj.viewuploadflies(data);
        }
        [Route("deleteuploadfile")]
        public Visitor_Management_Appointment_DTO deleteuploadfile([FromBody]Visitor_Management_Appointment_DTO data)
        {
            return interobj.deleteuploadfile(data);
        }
        [Route("todayappointments")]
        public Visitor_Management_Appointment_DTO todayappointments([FromBody]Visitor_Management_Appointment_DTO data)
        {
            return interobj.todayappointments(data);
        }

        [Route("visitormailtrigger/{id:int}")]
        public Visitor_Management_Appointment_DTO visitormailtrigger(int id)
        {
            Visitor_Management_Appointment_DTO data = new Visitor_Management_Appointment_DTO();
            return interobj.visitormailtrigger(data);
        }

        [Route("visitorappoinmentstatus/{id:int}")]
        public Visitor_Management_Appointment_DTO visitorappoinmentstatus(int id)
        {
            Visitor_Management_Appointment_DTO data = new Visitor_Management_Appointment_DTO();
            return interobj.visitorappoinmentstatus(data);
        }

    }
}
