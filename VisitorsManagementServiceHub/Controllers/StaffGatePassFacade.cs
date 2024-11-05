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
    public class StaffGatePassFacade : Controller
    {
        // GET: api/<controller>
        StaffGatePassInterface _objinter;
        public StaffGatePassFacade(StaffGatePassInterface obj)
        {
            _objinter = obj;
        }

        // POST api/<controller>
        [HttpPost]
        [Route("Getdetails")]
        public StaffGatePass_DTO Getdetails([FromBody]StaffGatePass_DTO data)
        {
            return _objinter.Getdetails(data);
        }
        [Route("getdepchange")]
        public StaffGatePass_DTO getdepchange([FromBody]StaffGatePass_DTO data)
        {
            return _objinter.getdepchange(data);
        }
        [Route("get_staff1")]
        public StaffGatePass_DTO get_staff1([FromBody]StaffGatePass_DTO value)
        {
            return _objinter.get_staff1(value);
        }
        [Route("saverecord")]
        public StaffGatePass_DTO saverecord([FromBody]StaffGatePass_DTO data)
        {
            return _objinter.saverecord(data);
        }
        [Route("editrecord")]
        public StaffGatePass_DTO editrecord([FromBody] StaffGatePass_DTO id)
        {
            return _objinter.editrecord(id);
        }
       
        [Route("deactive")]
        public StaffGatePass_DTO deactive([FromBody]StaffGatePass_DTO data)
        {
            return _objinter.deactive(data);
        }
       
        [Route("PrintGatePass")]
        public StaffGatePass_DTO PrintGatePass([FromBody]StaffGatePass_DTO data)
        {
            return _objinter.PrintGatePass(data);
        }
    }
}
