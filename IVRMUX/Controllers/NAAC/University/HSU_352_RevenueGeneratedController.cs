using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.University;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.University;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.University
{
    [Route("api/[controller]")]
    public class HSU_352_RevenueGeneratedController : Controller
    {
        HSU_352_RevenueGeneratedDelegate del = new HSU_352_RevenueGeneratedDelegate();
        [Route("loaddata/{id:int}")]
        public HSU_352_RevenueGeneratedDTO loaddata(int id)
        {
            HSU_352_RevenueGeneratedDTO data = new HSU_352_RevenueGeneratedDTO();

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.loaddata(data);
        }
        [Route("save")]
        public HSU_352_RevenueGeneratedDTO save([FromBody] HSU_352_RevenueGeneratedDTO data)
        {
            // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.save(data);
        }
        [Route("deactive")]
        public HSU_352_RevenueGeneratedDTO deactive([FromBody] HSU_352_RevenueGeneratedDTO data)
        {
            // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deactive(data);
        }
        [Route("EditData")]
        public HSU_352_RevenueGeneratedDTO EditData([FromBody] HSU_352_RevenueGeneratedDTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.EditData(data);
        }


        [Route("viewuploadflies")]
        public HSU_352_RevenueGeneratedDTO viewuploadflies([FromBody] HSU_352_RevenueGeneratedDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.viewuploadflies(data);
        }

        [Route("deleteuploadfile")]
        public HSU_352_RevenueGeneratedDTO deleteuploadfile([FromBody] HSU_352_RevenueGeneratedDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.deleteuploadfile(data);
        }
    }
}
