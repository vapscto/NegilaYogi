using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.Admission.Criteria7;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.Admission;
using PreadmissionDTOs.NAAC.Admission.Criteria7;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.Admission.Criteria7
{
    [Route("api/[controller]")]
    public class LocationalAdvtgController : Controller
    {
        public LocationalAdvtgDelegate del = new LocationalAdvtgDelegate();
        // GET: api/<controller>
        [Route("loaddata/{id:int}")]
        public LocationalAdvtgDTO loaddata(int id)
        {
            LocationalAdvtgDTO data = new LocationalAdvtgDTO();
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
         
            return del.loaddata(data);
        }
        [Route("getdata/{id:int}")]
        public LocationalAdvtgDTO getdata(int id)
        {
            LocationalAdvtgDTO data = new LocationalAdvtgDTO();
            data.MI_Id = id;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getdata(data);
        }
        [Route("savedatatab1")]
        public LocationalAdvtgDTO savedatatab1([FromBody]LocationalAdvtgDTO data)
        {
          
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savedatatab1(data);
        }
        [Route("EditData")]
        public LocationalAdvtgDTO edittab1([FromBody]LocationalAdvtgDTO data)
        {
          
            return del.edittab1(data);
        }
        [Route("deactivate")]
        public LocationalAdvtgDTO deactivYTab1([FromBody]LocationalAdvtgDTO data)
        {
         
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deactivYTab1(data);
        }

        [Route("deleteuploadfile")]
        public LocationalAdvtgDTO deleteuploadfile([FromBody] LocationalAdvtgDTO data)
        {
         
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deleteuploadfile(data);
        }
        [Route("viewuploadflies")]
        public LocationalAdvtgDTO viewuploadflies([FromBody] LocationalAdvtgDTO data)
        {
         
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.viewuploadflies(data);
        }
        [Route("getcomment")]
        public LocationalAdvtgDTO getcomment([FromBody] LocationalAdvtgDTO data)
        {
         
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getcomment(data);
        }
        [Route("getfilecomment")]
        public LocationalAdvtgDTO getfilecomment([FromBody] LocationalAdvtgDTO data)
        {
         
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getfilecomment(data);
        }
        [Route("savecomments")]
        public LocationalAdvtgDTO savecomments([FromBody] LocationalAdvtgDTO data)
        {
         
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savecomments(data);
        }
        [Route("savefilewisecomments")]
        public LocationalAdvtgDTO savefilewisecomments([FromBody] LocationalAdvtgDTO data)
        {
         
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savefilewisecomments(data);
        }
    }
}
