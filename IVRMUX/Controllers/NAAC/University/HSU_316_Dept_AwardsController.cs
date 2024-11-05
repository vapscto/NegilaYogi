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
    public class HSU_316_Dept_AwardsController : Controller
    {
        HSU_316_Dept_AwardsDelegate del = new HSU_316_Dept_AwardsDelegate();
        [Route("loaddata/{id:int}")]
        public HSU_316_Dept_AwardsDTO loaddata(int id)
        {
            HSU_316_Dept_AwardsDTO data = new HSU_316_Dept_AwardsDTO();

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = id;
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.loaddata(data);
        }
        [Route("save")]
        public HSU_316_Dept_AwardsDTO save([FromBody] HSU_316_Dept_AwardsDTO data)
        {
            // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.save(data);
        }
        [Route("deactive")]
        public HSU_316_Dept_AwardsDTO deactive([FromBody] HSU_316_Dept_AwardsDTO data)
        {
            // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deactive(data);
        }
        [Route("EditData")]
        public HSU_316_Dept_AwardsDTO EditData([FromBody] HSU_316_Dept_AwardsDTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.EditData(data);
        }


        [Route("viewuploadflies")]
        public HSU_316_Dept_AwardsDTO viewuploadflies([FromBody] HSU_316_Dept_AwardsDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.viewuploadflies(data);
        }


        [Route("deleteuploadfile")]
        public HSU_316_Dept_AwardsDTO deleteuploadfile([FromBody] HSU_316_Dept_AwardsDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.deleteuploadfile(data);
        }
    }
}
