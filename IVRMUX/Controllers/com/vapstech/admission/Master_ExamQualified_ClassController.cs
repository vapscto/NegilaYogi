using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.admission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.admission
{
    [Route("api/[controller]")]
    public class Master_ExamQualified_ClassController : Controller
    {

        [HttpGet]
        [Route("getalldata/{id:int}")]
        public Master_ExamQualified_ClassDTO getalldata(int id)
        {
            Master_ExamQualified_ClassDTO dto = new Master_ExamQualified_ClassDTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.getalldata(dto);


        }

        Master_ExamQualified_ClassDelegate _delegate = new Master_ExamQualified_ClassDelegate();
        [Route("SaveClass")]
        public Master_ExamQualified_ClassDTO SaveClass([FromBody]Master_ExamQualified_ClassDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.SaveClass(data);
        }
        [Route("Editdetails/{id:int}")]
        public Master_ExamQualified_ClassDTO Editdetails(int id)
        {
            return _delegate.Editdetails(id);
        }
        [Route("deactiveCat")]
        public Master_ExamQualified_ClassDTO deactiveCat([FromBody]Master_ExamQualified_ClassDTO user)
        {
            user.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            user.User_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.deactiveCat(user);

        }


    }
}
