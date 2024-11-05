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
    public class MC_343_TechnologyTransferredController : Controller
    {
        MC_343_TechnologyTransferredDelegate del = new MC_343_TechnologyTransferredDelegate();
        [Route("loaddata/{id:int}")]
        public MC_343_TechnologyTransferredDTO loaddata(int id)
        {
            MC_343_TechnologyTransferredDTO data = new MC_343_TechnologyTransferredDTO();

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = id;
            return del.loaddata(data);
        }
        [Route("save")]
        public MC_343_TechnologyTransferredDTO save([FromBody] MC_343_TechnologyTransferredDTO data)
        {
            // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.save(data);
        }
        [Route("deactive")]
        public MC_343_TechnologyTransferredDTO deactive([FromBody] MC_343_TechnologyTransferredDTO data)
        {
            // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deactive(data);
        }
        [Route("EditData")]
        public MC_343_TechnologyTransferredDTO EditData([FromBody] MC_343_TechnologyTransferredDTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.EditData(data);
        }


        [Route("viewuploadflies")]
        public MC_343_TechnologyTransferredDTO viewuploadflies([FromBody] MC_343_TechnologyTransferredDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.viewuploadflies(data);
        }


        [Route("deleteuploadfile")]
        public MC_343_TechnologyTransferredDTO deleteuploadfile([FromBody] MC_343_TechnologyTransferredDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.deleteuploadfile(data);
        }
    }
}
