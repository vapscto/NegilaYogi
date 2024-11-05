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
    public class NAAC_MC_423_StuLearningResourceController : Controller
    {
        NAAC_MC_423_StuLearningResourceDelegate del = new NAAC_MC_423_StuLearningResourceDelegate();
        [Route("loaddata/{id:int}")]
        public NAAC_MC_423_StuLearningResource_DTO loaddata(int id)
        {
            NAAC_MC_423_StuLearningResource_DTO data = new NAAC_MC_423_StuLearningResource_DTO();
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = id;
            return del.loaddata(data);
        }
        [Route("save")]
        public NAAC_MC_423_StuLearningResource_DTO save([FromBody]NAAC_MC_423_StuLearningResource_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.save(data);
        }
       
        [Route("EditData")]
        public NAAC_MC_423_StuLearningResource_DTO EditData([FromBody] NAAC_MC_423_StuLearningResource_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.EditData(data);
        }
        [Route("viewuploadflies")]
        public NAAC_MC_423_StuLearningResource_DTO viewuploadflies([FromBody] NAAC_MC_423_StuLearningResource_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.viewuploadflies(data);
        }

        [Route("deleteuploadfile")]
        public NAAC_MC_423_StuLearningResource_DTO deleteuploadfile([FromBody] NAAC_MC_423_StuLearningResource_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.deleteuploadfile(data);
        }

        [Route("loaddatainfra/{id:int}")]
        public NAAC_MC_423_StuLearningResource_DTO loaddatainfra(int id)
        {
            NAAC_MC_423_StuLearningResource_DTO data = new NAAC_MC_423_StuLearningResource_DTO();
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = id;
            return del.loaddatainfra(data);
        }
        [Route("saveinfra")]
        public NAAC_MC_423_StuLearningResource_DTO saveinfra([FromBody]NAAC_MC_423_StuLearningResource_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.saveinfra(data);
        }

        [Route("EditDatainfra")]
        public NAAC_MC_423_StuLearningResource_DTO EditDatainfra([FromBody] NAAC_MC_423_StuLearningResource_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.EditDatainfra(data);
        }
    }
}
