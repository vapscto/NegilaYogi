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
    public class NAAC_MC_351MasterController : Controller
    {
        public NAAC_MC_351MasterDelegate _delobj = new NAAC_MC_351MasterDelegate();

        [Route("loaddata/{id:int}")]
        public NAAC_MC_351_CollaborationActivities_DTO loaddata(int id)
        {
            NAAC_MC_351_CollaborationActivities_DTO data = new NAAC_MC_351_CollaborationActivities_DTO();
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = id;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delobj.loaddata(data);
        }


        [Route("savedata")]
        public NAAC_MC_351_CollaborationActivities_DTO savedata([FromBody] NAAC_MC_351_CollaborationActivities_DTO data)
        {

            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delobj.savedata(data);
        }

        [Route("editdata")]
        public NAAC_MC_351_CollaborationActivities_DTO editdata([FromBody] NAAC_MC_351_CollaborationActivities_DTO data)
        {

            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delobj.editdata(data);
        }

        [Route("deactivY")]
        public NAAC_MC_351_CollaborationActivities_DTO deactivY([FromBody] NAAC_MC_351_CollaborationActivities_DTO data)
        {

            // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delobj.deactivY(data);
        }

        [Route("viewuploadflies")]
        public NAAC_MC_351_CollaborationActivities_DTO viewuploadflies([FromBody] NAAC_MC_351_CollaborationActivities_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.viewuploadflies(data);
        }

        [Route("deleteuploadfile")]
        public NAAC_MC_351_CollaborationActivities_DTO deleteuploadfile([FromBody] NAAC_MC_351_CollaborationActivities_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.deleteuploadfile(data);
        }
    }
}
