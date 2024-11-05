using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HostelServiceHub.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Hostel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HostelServiceHub.Facade
{
    [Route("api/[controller]")]
    public class StaffRequestConfirmFacade : Controller
    {

        public StaffRequestConfirmInterface _Interface;
        public StaffRequestConfirmFacade(StaffRequestConfirmInterface parameter)
        {
            _Interface = parameter;
        }

        [Route("loaddata")]
        public Task<StaffRequestConfirm_DTO> loaddata([FromBody] StaffRequestConfirm_DTO data)
        {
            return _Interface.loaddata(data);
        }

        [Route("requestApproved")]
        public StaffRequestConfirm_DTO requestApproved([FromBody] StaffRequestConfirm_DTO data)
        {
            return _Interface.requestApproved(data);
        }

        [Route("requestRejected")]
        public StaffRequestConfirm_DTO requestRejected([FromBody] StaffRequestConfirm_DTO data)
        {
            return _Interface.requestRejected(data);
        }

        
    }
}
