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
    public class StudentRequestConfirmFacade : Controller
    {
        public StudentRequestConfirmInterface _Interface;
        public StudentRequestConfirmFacade(StudentRequestConfirmInterface parameter)
        {
            _Interface = parameter;
        }

        [Route("loaddata")]
        public Task<StudentRequestConfirm_DTO> loaddata([FromBody] StudentRequestConfirm_DTO data)
        {
            return _Interface.loaddata(data);
        }

        [Route("requestApproved")]
        public StudentRequestConfirm_DTO requestApproved([FromBody] StudentRequestConfirm_DTO data)
        {
            return _Interface.requestApproved(data);
        }

        [Route("requestRejected")]
        public StudentRequestConfirm_DTO requestRejected([FromBody] StudentRequestConfirm_DTO data)
        {
            return _Interface.requestRejected(data);
        }

        [Route("Ydeactive")]
        public StudentRequestConfirm_DTO Ydeactive([FromBody]StudentRequestConfirm_DTO data)
        {
            return _Interface.Ydeactive(data);
        }
    }
}
