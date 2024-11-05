using LeaveManagementServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagementServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class MasterLeaveFacadeController : Controller
    {
        public MasterLeaveInterface _masterleaveform;

        public MasterLeaveFacadeController(MasterLeaveInterface maspag)
        {
            _masterleaveform = maspag;
        }

        [HttpPost]
        [Route("GetLeave")]
        public MasterLeaveDTO GetLeave([FromBody]MasterLeaveDTO data)
        {
            return _masterleaveform.GetLeave(data);
        }

        
        [HttpPost]
        [Route("validateordernumber")]
        public MasterLeaveDTO validateordernumber([FromBody]MasterLeaveDTO data)
        {
            return _masterleaveform.validateordernumber(data);
        }


        // POST api/values
        [HttpPost]
        [Route("savedetail")]
        public MasterLeaveDTO savedetail([FromBody]MasterLeaveDTO data)
        {
            return _masterleaveform.saveData(data);
        }
        [Route("Edit/{id:int}")]
        public MasterLeaveDTO Edit(int id)
        {
            return _masterleaveform.Edit(id);
        }


        [Route("deletepages")]
        public MasterLeaveDTO deletepages([FromBody]MasterLeaveDTO data)
        {
            return _masterleaveform.deletepages(data);
        }
        [Route("getpagedetails/{id:int}")]     
        public MasterLeaveDTO getpagedetails(int id)
        {            
            return _masterleaveform.getpageedit(id);
        }    
      
        [Route("deactivate")]
        public MasterLeaveDTO deactivate([FromBody]MasterLeaveDTO data)
        {
            return _masterleaveform.deactivate(data);
        }
        [Route("SearchByColumn")]
        public MasterLeaveDTO SearchByColumn([FromBody]MasterLeaveDTO data)
        {
            return _masterleaveform.searchByColumn(data);
        }

    }
}
