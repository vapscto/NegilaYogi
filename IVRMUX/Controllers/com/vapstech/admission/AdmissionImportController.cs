using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;
using corewebapi18072016.Delegates.com.vaps.admission;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vaps.admission
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class AdmissionImportController : Controller
    {
        AdmissionImportDelegate adsd = new AdmissionImportDelegate();
        // GET: api/values

        //[HttpPost]
        //[Route("saveimporteddata")]
        //public Adm_M_StudentDTO getinitialdata([FromBody] Adm_M_StudentDTO data)
        //{
        //    return adsd.savedata(data);
        //} 
        [HttpPost]
   
        public ImportStudentWrapperDTO getinitialdata([FromBody] ImportStudentWrapperDTO data)
        {
            data.ASMAY_Id= Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return adsd.savedata(data);
        }
        [Route("checkvalidation")]
        public ImportStudentWrapperDTO checkvalidation([FromBody] ImportStudentWrapperDTO data)
        {
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return adsd.checkvalidation(data);
        }


    }
}
