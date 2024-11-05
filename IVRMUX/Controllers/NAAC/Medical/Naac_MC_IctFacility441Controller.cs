using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.Medical;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.Medical;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.Medical
{
    [Route("api/[controller]")]
    public class Naac_MC_IctFacility441Controller : Controller
    {
        Naac_MC_IctFacility441Delegate del = new Naac_MC_IctFacility441Delegate();
        [Route("loaddata/{id:int}")]
        public Naac_MC_IctFacility441_DTO loaddata(int id)
        {
            Naac_MC_IctFacility441_DTO data = new Naac_MC_IctFacility441_DTO();
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = id;
            return del.loaddata(data);
        }
        [Route("save")]
        public Naac_MC_IctFacility441_DTO save([FromBody]Naac_MC_IctFacility441_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.save(data);
        }
        [Route("EditData")]
        public Naac_MC_IctFacility441_DTO EditData([FromBody] Naac_MC_IctFacility441_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.EditData(data);
        }
        [Route("viewuploadflies")]
        public Naac_MC_IctFacility441_DTO viewuploadflies([FromBody] Naac_MC_IctFacility441_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.viewuploadflies(data);
        }
        [Route("deleteuploadfile")]
        public Naac_MC_IctFacility441_DTO deleteuploadfile([FromBody] Naac_MC_IctFacility441_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.deleteuploadfile(data);
        }
    }
}
