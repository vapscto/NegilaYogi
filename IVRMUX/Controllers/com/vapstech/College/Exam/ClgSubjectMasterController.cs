using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Exam;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Exam
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class ClgSubjectMasterController : Controller
    {
        ClgSubjectMasterDelegate delobj = new ClgSubjectMasterDelegate();

        [Route("getalldetails/{id:int}")]
        public MasterSubjectAllMDTO getalldetails(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.getdetails(id);
        }

        [Route("Editdetails/{id:int}")]
        public MasterSubjectAllMDTO EditDetails(int id)
        {
            return delobj.EditDetails(id);
        }

        [Route("savedetail")]
        public MasterSubjectAllMDTO Savedetails([FromBody] MasterSubjectAllMDTO maste)
        {
            maste.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return delobj.savedetails(maste);
        }

        [Route("validateordernumber")]
        public MasterSubjectAllMDTO validateordernumber([FromBody] MasterSubjectAllMDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.validateordernumber(data);
        }

        [Route("getdetails/{id:int}")]
        public MasterSubjectAllMDTO Get(int id)
        {
            MasterSubjectAllMDTO maste = new MasterSubjectAllMDTO();
            maste.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.GetmasterSubdetails(maste);
        }

        
       // [HttpDelete("{id}")]
        [Route("Deletedetails/{id:int}")]
        public MasterSubjectAllMDTO Delete(int id)
        {
            return delobj.DeleteMasterRecord(id);
        }

        [Route("savedata2")]
        public MasterSubjectAllMDTO savedata2([FromBody]MasterSubjectAllMDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.savedata2(data);
        }
    }
}
