using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Library;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Library;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Library
{
    [Route("api/[controller]")]
    public class ImportLibrarydataController : Controller
    {
        ImportLibrarydataDelegate _delobj = new ImportLibrarydataDelegate();
      
        //[Route("Savedata")]
        //public ImportLibrarydataDTO Savedata([FromBody] ImportLibrarydataDTO data)
        //{
        //    data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    return _delobj.Savedata(data);
        //}



        public ImportLibrarydataDTO Savedata([FromBody] ImportLibrarydataDTO data)
        {
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.Savedata(data);
        }
        [Route("getdetails/{id:int}")]
        public ImportLibrarydataDTO getdetails(int id)
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            id = mi_id;
            return _delobj.getdetails(id);
        }

        [Route("deactiveY")]
        public ImportLibrarydataDTO deactiveY([FromBody] ImportLibrarydataDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.deactiveY(data);
        }
    }
}
