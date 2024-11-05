using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Exam;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Exam
{
    [Route("api/[controller]")]
    public class ClgExamMasterController : Controller
    {
        ClgExamMasterDelegate delobj = new ClgExamMasterDelegate();
        [Route("Getdetails")]
        public exammasterDTO Getdetails(exammasterDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.Getdetails(data);
        }

        [Route("savedetails")]
        public exammasterDTO savedetails([FromBody] exammasterDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.savedetails(data);
        }

        [Route("editdetails/{id:int}")]
        public exammasterDTO editdetails(int ID)
        {
            return delobj.editdetails(ID);
        }

        [Route("validateordernumber")]
        public exammasterDTO validateordernumber([FromBody] exammasterDTO data)
        {
            return delobj.validateordernumber(data);
        }

        [Route("deactivate")]
        public exammasterDTO deactivate([FromBody] exammasterDTO data)
        {
            return delobj.deactivate(data);
        }
    }
}
