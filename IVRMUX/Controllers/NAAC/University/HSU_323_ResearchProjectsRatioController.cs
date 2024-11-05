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
    public class HSU_323_ResearchProjectsRatioController : Controller
    {
        HSU_323_ResearchProjectsRatioDelegate del = new HSU_323_ResearchProjectsRatioDelegate();
        [Route("loaddata/{id:int}")]
        public HSU_323_ResearchProjectsRatioDTO loaddata(int id)
        {
            HSU_323_ResearchProjectsRatioDTO data = new HSU_323_ResearchProjectsRatioDTO();

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = id;
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.loaddata(data);
        }
        [Route("save")]
        public HSU_323_ResearchProjectsRatioDTO save([FromBody] HSU_323_ResearchProjectsRatioDTO data)
        {
            // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.save(data);
        }
        [Route("deactive")]
        public HSU_323_ResearchProjectsRatioDTO deactive([FromBody] HSU_323_ResearchProjectsRatioDTO data)
        {
            // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deactive(data);
        }
        [Route("EditData")]
        public HSU_323_ResearchProjectsRatioDTO EditData([FromBody] HSU_323_ResearchProjectsRatioDTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.EditData(data);
        }
        
        [Route("viewuploadflies")]
        public HSU_323_ResearchProjectsRatioDTO viewuploadflies([FromBody] HSU_323_ResearchProjectsRatioDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.viewuploadflies(data);
        }

        [Route("deleteuploadfile")]
        public HSU_323_ResearchProjectsRatioDTO deleteuploadfile([FromBody] HSU_323_ResearchProjectsRatioDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.deleteuploadfile(data);
        }
    }
}
