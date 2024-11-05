using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using Recruitment.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Recruitment.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class VMS_AcknowledgementFacadeController : Controller
    {
        public VMS_AcknowledgementInterface inter;
        public VMS_AcknowledgementFacadeController(VMS_AcknowledgementInterface i)
        {
            inter = i;
        }
        [HttpPost]
        [Route("loaddata")]
        public HR_VMS_AcknowledgementDTO loaddata([FromBody] HR_VMS_AcknowledgementDTO data)
        {
            return inter.loaddata(data);
        }
    }
}
