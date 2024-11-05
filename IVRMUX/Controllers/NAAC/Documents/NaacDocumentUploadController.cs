using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.Documents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.Documents;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.Documents
{
    [Route("api/[controller]")]
    public class NaacDocumentUploadController : Controller
    {
        public NaacDocumentUploadDelegate _delg = new NaacDocumentUploadDelegate();

        // GET: api/<controller>
        [HttpGet]
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
        [Route("onload/{id:int}")]
        public NaacDocumentUploadDTO onload(int id)
        {
            NaacDocumentUploadDTO data = new NaacDocumentUploadDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onload(data);
        }      

        [Route("save")]
        public NaacDocumentUploadDTO save([FromBody] NaacDocumentUploadDTO data)
        {           
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.NAACMSL_CreatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.NAACMSL_UpdatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.NAACSL_CreatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.NAACSL_UpdatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.save(data);
        }

        [Route("saveapproved")]
        public NaacDocumentUploadDTO saveapproved([FromBody] NaacDocumentUploadDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.NAACMSL_CreatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.NAACMSL_UpdatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.NAACSL_CreatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.NAACSL_UpdatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.saveapproved(data);
        }


        // Get Upload Documents
        [Route("getuploaddoc")]
        public NaacDocumentUploadDTO getuploaddoc([FromBody] NaacDocumentUploadDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.NAACMSL_CreatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.NAACMSL_UpdatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.NAACSL_CreatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.NAACSL_UpdatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.getuploaddoc(data);
        }         

        // File Wise Comments Saving And View
        [Route("savecomments")]
        public NaacDocumentUploadDTO savecomments([FromBody] NaacDocumentUploadDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.NAACMSL_CreatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.NAACMSL_UpdatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.NAACSL_CreatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.NAACSL_UpdatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.savecomments(data);
        }
        [Route("viewcomments")]
        public NaacDocumentUploadDTO viewcomments([FromBody] NaacDocumentUploadDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.NAACMSL_CreatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.NAACMSL_UpdatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.NAACSL_CreatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.NAACSL_UpdatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.viewcomments(data);
        }
        // End Of File Wise Comments Saving And View

        // Get General Comments
        [Route("getcomments")]
        public NaacDocumentUploadDTO getcomments([FromBody] NaacDocumentUploadDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.NAACMSL_CreatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.NAACMSL_UpdatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.NAACSL_CreatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.NAACSL_UpdatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.getcomments(data);
        }
        // Save General Comments 
        [Route("savegeneralcommetns")]
        public NaacDocumentUploadDTO savegeneralcommetns([FromBody] NaacDocumentUploadDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.NAACMSL_CreatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.NAACMSL_UpdatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.NAACSL_CreatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.NAACSL_UpdatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.savegeneralcommetns(data);
        }

        [Route("savecommentscons")]
        public NaacDocumentUploadDTO savecommentscons([FromBody] NaacDocumentUploadDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.NAACMSL_CreatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.NAACMSL_UpdatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.NAACSL_CreatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.NAACSL_UpdatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.savecommentscons(data);
        }

        // Save Hyperlinks 
        [Route("savehyperlinks")]
        public NaacDocumentUploadDTO savehyperlinks([FromBody] NaacDocumentUploadDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.NAACMSL_CreatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.NAACMSL_UpdatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.NAACSL_CreatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.NAACSL_UpdatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.savehyperlinks(data);
        }

        [Route("viewaddedhyperlink")]
        public NaacDocumentUploadDTO viewaddedhyperlink([FromBody] NaacDocumentUploadDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.NAACMSL_CreatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.NAACMSL_UpdatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.NAACSL_CreatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.NAACSL_UpdatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.viewaddedhyperlink(data);
        }
        [Route("deletehyperlink")]
        public NaacDocumentUploadDTO deletehyperlink([FromBody] NaacDocumentUploadDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.NAACMSL_CreatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.NAACMSL_UpdatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.NAACSL_CreatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.NAACSL_UpdatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.deletehyperlink(data);
        }

        [Route("deleteuploadfile")]
        public NaacDocumentUploadDTO deleteuploadfile([FromBody] NaacDocumentUploadDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.NAACMSL_CreatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.NAACMSL_UpdatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.NAACSL_CreatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.NAACSL_UpdatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.deleteuploadfile(data);
        }

        [Route("saveCGPA")]
        public NaacDocumentUploadDTO saveCGPA([FromBody] NaacDocumentUploadDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.NAACMSL_CreatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.NAACMSL_UpdatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.NAACSL_CreatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.NAACSL_UpdatedBy = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.saveCGPA(data);
        }


    }
}
