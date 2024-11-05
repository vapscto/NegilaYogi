using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.admission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.admission
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class studenttccustomreportController : Controller
    {
        studenttccustomreportDelegate studtc = new studenttccustomreportDelegate();
        [Route("getdetails/{id:int}")]
        public studenttccustomreportDTO getinitialdata(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return studtc.getinitialdata(id);
        }
        [Route("changeyear")]
        public studenttccustomreportDTO changeyear([FromBody]studenttccustomreportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return studtc.changeyear(data);
        }
        [Route("changeclass")]
        public studenttccustomreportDTO changeclass([FromBody]studenttccustomreportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return studtc.changeclass(data);
        }

        [Route("changesection")]
        public studenttccustomreportDTO changesection([FromBody]studenttccustomreportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return studtc.changesection(data);
        }

        [Route("getTcdetails")]
        public studenttccustomreportDTO getTCdata([FromBody] studenttccustomreportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return studtc.getTCdata(data);
        }
    }
}
