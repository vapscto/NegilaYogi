using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.VidyaBharathi;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VidyaBharathi;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.VidyaBharathi
{
    [Route("api/[controller]")]
    public class VBSC_MasterCompetition_Category : Controller
    {
        VBSC_MasterCompetition_CategoryDelegate cms = new VBSC_MasterCompetition_CategoryDelegate();

        [HttpGet]
        [Route("loaddata/{id:int}")]
        public VBSC_MasterCompetition_CategoryDTO loaddata(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return cms.loaddata(id);

        }
        //
        
        [HttpPost]
        [Route("savedata")]
        public VBSC_MasterCompetition_CategoryDTO savedata([FromBody]VBSC_MasterCompetition_CategoryDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.savedata(data);
        }
        //Deactivate
        [Route("Deactivate")]
        public VBSC_MasterCompetition_CategoryDTO Deactivate([FromBody]VBSC_MasterCompetition_CategoryDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.Deactivate(data);
        }
        //Organsation
        [Route("Organsation")]
        public VBSC_MasterCompetition_CategoryDTO Organsation([FromBody]VBSC_MasterCompetition_CategoryDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.Organsation(data);
        }
        //Master_Competition_Category_ClassesDTO
        [Route("savedataCl")]
        public Master_Competition_Category_ClassesDTO savedataCl([FromBody]Master_Competition_Category_ClassesDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.savedataCl(data);
        }
        //Deactivate
        [Route("DeactivateCl")]
        public Master_Competition_Category_ClassesDTO DeactivateCl([FromBody]Master_Competition_Category_ClassesDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.DeactivateCl(data);
        }
        [HttpGet]
        [Route("getdata/{id:int}")]
        public VBSC_Master_Competition_Category_LevelsDTO getdata(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return cms.getdata(id);

        }
        // 
        [HttpPost]
        [Route("savedataVCl")]
        public VBSC_Master_Competition_Category_LevelsDTO savedataVCl([FromBody]VBSC_Master_Competition_Category_LevelsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.savedataVCl(data);
        }
        //Deactivate
        [Route("DeactivateVCl")]
        public VBSC_Master_Competition_Category_LevelsDTO DeactivateCl([FromBody]VBSC_Master_Competition_Category_LevelsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return cms.DeactivateVCl(data);
        }
    }
}
