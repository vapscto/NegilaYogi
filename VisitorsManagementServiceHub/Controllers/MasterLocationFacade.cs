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
    public class MasterLocationFacade : Controller
    {
        // GET: api/<controller>
        MasterLocationInterface _objInter;

        public MasterLocationFacade(MasterLocationInterface parameter)
        {
            _objInter = parameter;
        }

        [Route("getdetails")]
        public Visitor_Management_Master_Location_DTO getdetails([FromBody] Visitor_Management_Master_Location_DTO data)
        {
            return _objInter.getdetails(data);
        }

        [Route("saveRecorddata")]
        public Visitor_Management_Master_Location_DTO saveRecorddata([FromBody] Visitor_Management_Master_Location_DTO data)
        {
            return _objInter.saveRecorddata(data);
        }

        [Route("editrecord")]
        public Visitor_Management_Master_Location_DTO editrecord([FromBody] Visitor_Management_Master_Location_DTO data)
        {
            return _objInter.editrecord(data);
        }
        [Route("deactiveY")]
        public Visitor_Management_Master_Location_DTO deactiveY([FromBody] Visitor_Management_Master_Location_DTO data)
        {
            return _objInter.deactiveY(data);
        }
        
    }
}
