using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using corewebapi18072016.Delegates.com.vapstech.Fees;
using IVRMUX.Delegates.com.vapstech.College.Fees;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Fees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Fees
{
    [Route("api/[controller]")]
    public class OtherCollegeStudentEntryController : Controller
    {
        OtherCollegeStudentEntryDelegate del = new OtherCollegeStudentEntryDelegate();
        [Route("getdetails/{id:int}")]
        public Fee_Master_College_OtherStudentsDTO getdetails(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            
            return del.getdata(id);
        }
        [HttpPost]
        public Fee_Master_College_OtherStudentsDTO save([FromBody]Fee_Master_College_OtherStudentsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("User_Id"));
            return del.save(data);
        }
        [Route("edit/{id:int}")]
        public Fee_Master_College_OtherStudentsDTO edit(int id)
        {
            return del.edit(id);
        }
        [Route("delete/{id:int}")]
        public Fee_Master_College_OtherStudentsDTO delete(int id)
        {
            return del.delete(id);
        }
    }
}
