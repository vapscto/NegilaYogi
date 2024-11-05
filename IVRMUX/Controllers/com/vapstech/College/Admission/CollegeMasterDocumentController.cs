using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Admission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Admission
{
    [Route("api/[controller]")]
    public class CollegeMasterDocumentController : Controller
    {

        public CollegeMasterDocumentDelegate _delgate = new CollegeMasterDocumentDelegate();
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // First Tab

        [Route("Getdetails/")]
        public CollegeDocumentMasterDTO Getdetails(CollegeDocumentMasterDTO id)
        {
            CollegeDocumentMasterDTO data = new CollegeDocumentMasterDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delgate.Getdetails(data);
        }
        [Route("savedata")]
        public CollegeDocumentMasterDTO savedata([FromBody]CollegeDocumentMasterDTO data)
        {           
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delgate.savedata(data);
        }
        [Route("Editdata")]
        public CollegeDocumentMasterDTO Editdata([FromBody]CollegeDocumentMasterDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delgate.Editdata(data);
        }
        [Route("DeleteData")]
        public CollegeDocumentMasterDTO DeleteData([FromBody]CollegeDocumentMasterDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delgate.DeleteData(data);
        }

        // End Of First Tab


        [Route("onchangecourse")]
        public CollegeDocumentMasterDTO onchangecourse([FromBody]CollegeDocumentMasterDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delgate.onchangecourse(data);
        }
        [Route("savedata1")]
        public CollegeDocumentMasterDTO savedata1([FromBody]CollegeDocumentMasterDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delgate.savedata1(data);
        }
        [Route("getdoc")]
        public CollegeDocumentMasterDTO getdoc([FromBody]CollegeDocumentMasterDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delgate.getdoc(data);
        }
        [Route("deactive_sub")]
        public CollegeDocumentMasterDTO deactive_sub([FromBody]CollegeDocumentMasterDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delgate.deactive_sub(data);
        }       

    }
}
