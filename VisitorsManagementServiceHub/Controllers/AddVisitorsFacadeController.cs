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
    public class AddVisitorsFacadeController : Controller
    {
        AddVisitorsInterface interobj;
        public AddVisitorsFacadeController(AddVisitorsInterface obj)
        {
            interobj = obj;
        }

        // POST api/<controller>
        [HttpPost]
        [Route("getDetails")]
        public AddVisitorsDTO getDetails([FromBody]AddVisitorsDTO data)
        {
            return interobj.getDetails(data);
        }

        [Route("EditDetails")]
        public AddVisitorsDTO EditDetails([FromBody] AddVisitorsDTO id)
        {
            return interobj.EditDetails(id);
        }

        [Route("saveData")]
        public Task<AddVisitorsDTO> saveData([FromBody]AddVisitorsDTO data)
        {
            return interobj.saveDataAsync(data);
        }

        [Route("deactivate")]
        public AddVisitorsDTO deactivate([FromBody]AddVisitorsDTO data)
        {
            return interobj.deactivate(data);
        }

        [Route("GetMultiVisitorDetails")]
        public AddVisitorsDTO GetMultiVisitorDetails([FromBody]AddVisitorsDTO data)
        {
            return interobj.GetMultiVisitorDetails(data);
        } 

        [Route("GetVisitorDetails")]
        public AddVisitorsDTO GetVisitorDetails([FromBody]AddVisitorsDTO data)
        {
            return interobj.GetVisitorDetails(data);
        } 

        [Route("UpdateStatus")]
        public Task<AddVisitorsDTO> UpdateStatus([FromBody]AddVisitorsDTO data)
        {
            return interobj.UpdateStatus(data);
        }   

        [Route("BlockOrUblockVisitor")]
        public AddVisitorsDTO BlockOrUblockVisitor([FromBody]AddVisitorsDTO data)
        {
            return interobj.BlockOrUblockVisitor(data);
        }     

        [Route("GetVisitorMultiDocuments")]
        public AddVisitorsDTO GetVisitorMultiDocuments([FromBody]AddVisitorsDTO data)
        {
            return interobj.GetVisitorMultiDocuments(data);
        }      

        [Route("GetVisitorIdCardDetails")]
        public AddVisitorsDTO GetVisitorIdCardDetails([FromBody]AddVisitorsDTO data)
        {
            return interobj.GetVisitorIdCardDetails(data);
        }     

        [Route("UpdateIDCardDetails")]
        public AddVisitorsDTO UpdateIDCardDetails([FromBody]AddVisitorsDTO data)
        {
            return interobj.UpdateIDCardDetails(data);
        }       

        [Route("SearchPreviousVisitor")]
        public AddVisitorsDTO SearchPreviousVisitor([FromBody]AddVisitorsDTO data)
        {
            return interobj.SearchPreviousVisitor(data);
        }        

        [Route("AddPreviousVisitorDetails")]
        public AddVisitorsDTO AddPreviousVisitorDetails([FromBody]AddVisitorsDTO data)
        {
            return interobj.AddPreviousVisitorDetails(data);
        }

        // Assign Details


        [Route("getAssignDetails")]
        public AddVisitorsDTO getAssignDetails([FromBody]AddVisitorsDTO data)
        {
            return interobj.getAssignDetails(data);
        }

        [Route("getVisitorAssignDetails")]
        public AddVisitorsDTO getVisitorAssignDetails([FromBody]AddVisitorsDTO data)
        {
            return interobj.getVisitorAssignDetails(data);
        }

        [Route("saveAssignedData")]
        public Task<AddVisitorsDTO> saveAssignedData([FromBody]AddVisitorsDTO data)
        {
            return interobj.saveAssignedData(data);
        }

        [Route("GetVisitorAssginDetails")]
        public AddVisitorsDTO GetVisitorAssginDetails([FromBody]AddVisitorsDTO data)
        {
            return interobj.GetVisitorAssginDetails(data);
        }

        // Appointment Visitor
        [Route("SearchAppVisitors")]
        public AddVisitorsDTO SearchAppVisitors([FromBody]AddVisitorsDTO data)
        {
            return interobj.SearchAppVisitors(data);
        }

        [Route("GetAppointmentVisitorDetails")]
        public AddVisitorsDTO GetAppointmentVisitorDetails([FromBody]AddVisitorsDTO data)
        {
            return interobj.GetAppointmentVisitorDetails(data);
        }
    }
}