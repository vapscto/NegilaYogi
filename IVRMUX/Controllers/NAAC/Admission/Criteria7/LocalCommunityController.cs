using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.Admission.Criteria7;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.Admission.Criteria7;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.Admission.Criteria7
{
    [Route("api/[controller]")]
    public class LocalCommunityController : Controller
    {
        public LocalCommunityDelegate del = new LocalCommunityDelegate();
        [Route("loaddata/{id:int}")]
        public LocalCommunityDTO loaddata(int id)
        {
            LocalCommunityDTO data = new LocalCommunityDTO();
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));            
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.loaddata(data);
        }

        [Route("getdata/{id:int}")]
        public LocalCommunityDTO getdata(int id)
        {
            LocalCommunityDTO data = new LocalCommunityDTO();
            data.MI_Id = id;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getdata(data);
        }
        [Route("savedatatab1")]
        public LocalCommunityDTO savedatatab1([FromBody]LocalCommunityDTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savedatatab1(data);
        }
        [Route("EditData")]
        public LocalCommunityDTO edittab1([FromBody]LocalCommunityDTO data)
        {
           // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.edittab1(data);
        }
        [Route("deactivate")]
        public LocalCommunityDTO deactivYTab1([FromBody]LocalCommunityDTO data)
        {
          //  data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deactivYTab1(data);
        }

        [Route("deleteuploadfile")]
        public LocalCommunityDTO deleteuploadfile([FromBody] LocalCommunityDTO data)
        {
           // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deleteuploadfile(data);
        }
        [Route("viewuploadflies")]
        public LocalCommunityDTO viewuploadflies([FromBody] LocalCommunityDTO data)
        {
           // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.viewuploadflies(data);
        }
        [Route("getcomment")]
        public LocalCommunityDTO getcomment([FromBody] LocalCommunityDTO data)
        {         
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getcomment(data);
        }
        [Route("getfilecomment")]
        public LocalCommunityDTO getfilecomment([FromBody] LocalCommunityDTO data)
        {         
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getfilecomment(data);
        }
        [Route("savecomments")]
        public LocalCommunityDTO savecomments([FromBody] LocalCommunityDTO data)
        {         
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savecomments(data);
        }
        [Route("savefilewisecomments")]
        public LocalCommunityDTO savefilewisecomments([FromBody] LocalCommunityDTO data)
        {         
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savefilewisecomments(data);
        }
    }
}
