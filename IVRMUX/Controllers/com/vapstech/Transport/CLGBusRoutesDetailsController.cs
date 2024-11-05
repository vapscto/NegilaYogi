using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Transport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Transport;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Transport
{
    [Route("api/[controller]")]
    public class CLGBusRoutesDetailsController : Controller
    {

        CLGBusRoutesDetailsDelegate del = new CLGBusRoutesDetailsDelegate();

        [Route("loaddata/{id:int}")]
       public CLGBusRoutesDetailsDTO loaddata(int id)
        {
            CLGBusRoutesDetailsDTO data = new CLGBusRoutesDetailsDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.loaddata(data);
        }
        [Route("getbranch")]
        public CLGBusRoutesDetailsDTO getbranch([FromBody] CLGBusRoutesDetailsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getbranch(data);
        }

        [Route("getsemester")]
        public CLGBusRoutesDetailsDTO getsemester([FromBody] CLGBusRoutesDetailsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getsemester(data);
        }

        
        
         [Route("getreport")]
        public CLGBusRoutesDetailsDTO getreport([FromBody] CLGBusRoutesDetailsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getreport(data);
        }


    }
}
