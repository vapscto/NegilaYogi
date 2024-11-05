using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.VMS.HRMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.HRMS;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.VMS.HRMS
{
    [Route("api/[controller]")]
    public class VMS_AcknowledgementController : Controller
    {
        VMS_AcknowledgementDelegate del = new VMS_AcknowledgementDelegate();


       [Route("loaddata/{id:int}")]
        public HR_VMS_AcknowledgementDTO loaddata(int id)
        {
            HR_VMS_AcknowledgementDTO data = new HR_VMS_AcknowledgementDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddata(data);
        }

    }
}
