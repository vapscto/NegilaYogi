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
    public class NAAC_HSU_StudentComplaints252PageController : Controller
    {
        NAAC_HSU_StudentComplaints252Delegate del = new NAAC_HSU_StudentComplaints252Delegate();
        [Route("loaddata/{id:int}")]
        public NAAC_HSU_StudentComplaints252_DTO loaddata(int id)
        {
            NAAC_HSU_StudentComplaints252_DTO data = new NAAC_HSU_StudentComplaints252_DTO();
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = id;
            return del.loaddata(data);
        }
        [Route("save")]
        public NAAC_HSU_StudentComplaints252_DTO save([FromBody]NAAC_HSU_StudentComplaints252_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.save(data);
        }
        [Route("deactive")]
        public NAAC_HSU_StudentComplaints252_DTO deactive([FromBody] NAAC_HSU_StudentComplaints252_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.deactive(data);
        }
        [Route("EditData")]
        public NAAC_HSU_StudentComplaints252_DTO EditData([FromBody] NAAC_HSU_StudentComplaints252_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.EditData(data);
        }



        [Route("viewuploadflies")]
        public NAAC_HSU_StudentComplaints252_DTO viewuploadflies([FromBody] NAAC_HSU_StudentComplaints252_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.viewuploadflies(data);
        }


        [Route("deleteuploadfile")]
        public NAAC_HSU_StudentComplaints252_DTO deleteuploadfile([FromBody] NAAC_HSU_StudentComplaints252_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.deleteuploadfile(data);
        }
    }
}
