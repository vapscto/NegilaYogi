using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Exam;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College;
using PreadmissionDTOs.com.vaps.College.Exam;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Exam
{
    [Route("api/[controller]")]
    public class ClgMasterSubjectGroupController : Controller
    {
        ClgMasterSubjectGroupDelegate delobj = new ClgMasterSubjectGroupDelegate();

        [Route("getalldetails")]
        public MasterSubjectGroupDTO Get([FromQuery] int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.getdetails(id);
        }

        [Route("savedetail")]
        public MasterSubjectGroupDTO savedetail([FromBody] MasterSubjectGroupDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.savedetail(categorypage);
        }

        [Route("deactivate")]
        public MasterSubjectGroupDTO deactivate([FromBody] MasterSubjectGroupDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.deactivate(categorypage);
        }

        [Route("getalldetailsviewrecords/{id:int}")]
        public Exm_Col_Master_Group_SubjectsDTO getalldetailsviewrecords(int id)
        {
            return delobj.getalldetailsviewrecords(id);
        }

        [Route("getdetails/{id:int}")]
        public MasterSubjectGroupDTO getdetail(int id)
        {
            return delobj.getpagedetails(id);
        }

        [Route("deletepages/{id:int}")]
        public MasterSubjectGroupDTO deletepages(int id)
        {
            return delobj.deleterec(id);
        }
    }
}
