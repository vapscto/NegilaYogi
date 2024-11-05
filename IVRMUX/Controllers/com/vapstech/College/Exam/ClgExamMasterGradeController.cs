using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Exam;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Exam
{
    [Route("api/[controller]")]
    public class ClgExamMasterGradeController : Controller
    {
        ClgExamMasterGradeDelegate delobj = new ClgExamMasterGradeDelegate();

        [Route("getalldetails")]
        public MasterExamGradeDTO Get([FromQuery] int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.getdetails(id);
        }

        [Route("savedetail")]
        public MasterExamGradeDTO savedetail([FromBody] MasterExamGradeDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.savedetail(categorypage);
        }

        [Route("deactivate")]
        public MasterExamGradeDTO deactivate([FromBody] MasterExamGradeDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.deactivate(categorypage);
        }

        [Route("getalldetailsviewrecords/{id:int}")]
        public MasterExamGradeDTO getalldetailsviewrecords(int id)
        {
            return delobj.getalldetailsviewrecords(id);
        }

        [Route("getdetails/{id:int}")]
        public MasterExamGradeDTO getdetail(int id)
        {
            return delobj.getpagedetails(id);
        }

        [Route("deletepages/{id:int}")]
        public MasterExamGradeDTO deletepages(int id)
        {
            return delobj.deleterec(id);
        }
    }
}
