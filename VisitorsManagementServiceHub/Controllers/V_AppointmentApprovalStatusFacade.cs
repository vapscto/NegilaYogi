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
    public class V_AppointmentApprovalStatusFacade : Controller
    {
        public V_AppointmentApprovalStatusInterface _Interfae;
        // GET: api/<controller>
        public V_AppointmentApprovalStatusFacade(V_AppointmentApprovalStatusInterface para)
        {
            _Interfae = para;
        }

        [Route("getDetails")]
        public AppointmentApprovalStatus_DTO getDetails([FromBody]AppointmentApprovalStatus_DTO data)
        {
            return _Interfae.getDetails(data);
        }

        [Route("EditDetails")]
        public AppointmentApprovalStatus_DTO EditDetails([FromBody]AppointmentApprovalStatus_DTO id)
        {
            return _Interfae.EditDetails(id);
        }
        [Route("Editnew")]
        public AppointmentApprovalStatus_DTO Editnew([FromBody]AppointmentApprovalStatus_DTO id)
        {
            return _Interfae.Editnew(id);
        }

        [Route("saveData")]
        public Task<AppointmentApprovalStatus_DTO> saveDataAsync([FromBody]AppointmentApprovalStatus_DTO data)
        {
            return _Interfae.saveDataAsync(data);
        }
          [Route("viewuploadflies")]
        public AppointmentApprovalStatus_DTO viewuploadflies([FromBody]AppointmentApprovalStatus_DTO data)
        {
            return _Interfae.viewuploadflies(data);
        }
        [Route("sendMOM")]
        public AppointmentApprovalStatus_DTO sendMOM([FromBody]AppointmentApprovalStatus_DTO data)
        {
            return _Interfae.sendMOM(data);
        }

        [Route("ApprovalReminder")]
        public AppointmentApprovalStatus_DTO ApprovalReminder([FromBody]AppointmentApprovalStatus_DTO data)
        {
            return _Interfae.ApprovalReminder(data);
        }
        [Route("savefeedback")]
        public AppointmentApprovalStatus_DTO savefeedback([FromBody]AppointmentApprovalStatus_DTO data)
        {
            return _Interfae.savefeedback(data);
        }
        [Route("getfeedback")]
        public AppointmentApprovalStatus_DTO getfeedback([FromBody]AppointmentApprovalStatus_DTO data)
        {
            return _Interfae.getfeedback(data);
        }

    }
}
