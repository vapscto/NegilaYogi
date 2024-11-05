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
    public class AddVisitorsController : Controller
    {
        AddVisitorsDelegate delobj = new AddVisitorsDelegate();

        // GET: api/<controller>
        [HttpGet]

        [Route("getDetails/{id:int}")]
        public AddVisitorsDTO getDetails(int id)
        {
            AddVisitorsDTO dto = new AddVisitorsDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.getDetails(dto);
        }

        [Route("Edit")]
        public AddVisitorsDTO Edit([FromBody] AddVisitorsDTO dto)
        {
            //AddVisitorsDTO dto = new AddVisitorsDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.EditDetails(dto);
        }

        [HttpPost]
        [Route("saveData")]
        public AddVisitorsDTO saveData([FromBody]AddVisitorsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.saveData(data);
        }

        [Route("deactivate")]
        public AddVisitorsDTO deactivate([FromBody] AddVisitorsDTO d)
        {
            d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            d.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.deactivate(d);
        }

        [Route("GetMultiVisitorDetails")]
        public AddVisitorsDTO GetMultiVisitorDetails([FromBody] AddVisitorsDTO d)
        {
            d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            d.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.GetMultiVisitorDetails(d);
        }

        [Route("GetVisitorDetails")]
        public AddVisitorsDTO GetVisitorDetails([FromBody] AddVisitorsDTO d)
        {
            d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            d.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.GetVisitorDetails(d);
        }

        [Route("UpdateStatus")]
        public AddVisitorsDTO UpdateStatus([FromBody] AddVisitorsDTO d)
        {
            d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            d.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.UpdateStatus(d);
        }

        [Route("BlockOrUblockVisitor")]
        public AddVisitorsDTO BlockOrUblockVisitor([FromBody] AddVisitorsDTO d)
        {
            d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            d.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.BlockOrUblockVisitor(d);
        }

        [Route("GetVisitorMultiDocuments")]
        public AddVisitorsDTO GetVisitorMultiDocuments([FromBody] AddVisitorsDTO d)
        {
            d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            d.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.GetVisitorMultiDocuments(d);
        }

        [Route("GetVisitorIdCardDetails")]
        public AddVisitorsDTO GetVisitorIdCardDetails([FromBody] AddVisitorsDTO d)
        {
            d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            d.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.GetVisitorIdCardDetails(d);
        }

        [Route("UpdateIDCardDetails")]
        public AddVisitorsDTO UpdateIDCardDetails([FromBody] AddVisitorsDTO d)
        {
            d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            d.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.UpdateIDCardDetails(d);
        }

        [Route("SearchPreviousVisitor/{id:int}")]
        public AddVisitorsDTO SearchPreviousVisitor(int id)
        {
            AddVisitorsDTO dto = new AddVisitorsDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.SearchPreviousVisitor(dto);
        }

        [Route("AddPreviousVisitorDetails")]
        public AddVisitorsDTO AddPreviousVisitorDetails([FromBody] AddVisitorsDTO dto)
        {            
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.AddPreviousVisitorDetails(dto);
        }


        // Assign Details
        [Route("getAssignDetails/{id:int}")]
        public AddVisitorsDTO getAssignDetails(int id)
        {
            AddVisitorsDTO dto = new AddVisitorsDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.getAssignDetails(dto);
        }

        [Route("getVisitorAssignDetails")]
        public AddVisitorsDTO getVisitorAssignDetails([FromBody] AddVisitorsDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.getVisitorAssignDetails(dto);
        }

        [Route("saveAssignedData")]
        public AddVisitorsDTO saveAssignedData([FromBody] AddVisitorsDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.saveAssignedData(dto);
        }

        [Route("GetVisitorAssginDetails")]
        public AddVisitorsDTO GetVisitorAssginDetails([FromBody] AddVisitorsDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.GetVisitorAssginDetails(dto);
        }

        // Appointment Visitor 
        [Route("SearchAppVisitors/{id:int}")]
        public AddVisitorsDTO SearchAppVisitors(int id)
        {
            AddVisitorsDTO dto = new AddVisitorsDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.SearchAppVisitors(dto);
        }

        [Route("GetAppointmentVisitorDetails")]
        public AddVisitorsDTO GetAppointmentVisitorDetails([FromBody] AddVisitorsDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.GetAppointmentVisitorDetails(dto);
        }
         

        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
