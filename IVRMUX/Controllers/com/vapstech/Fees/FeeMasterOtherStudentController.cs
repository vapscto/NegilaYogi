using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.Fees;
using PreadmissionDTOs.com.vaps.Fees;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Fees
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class FeeMasterOtherStudentController : Controller
    {
        FeeMasterOtherStudentDelegate del = new FeeMasterOtherStudentDelegate();
        [Route("getdetails/{id:int}")]
        public FeeMasterOtherStudentDTO getdetails(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getdata(id);
        }
        [HttpPost]
        public FeeMasterOtherStudentDTO save([FromBody]FeeMasterOtherStudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.save(data);
        }
        [Route("edit/{id:int}")]
        public FeeMasterOtherStudentDTO edit(int id)
        {
            return del.edit(id);
        }
        [Route("delete/{id:int}")]
        public FeeMasterOtherStudentDTO delete(int id)
        {
            return del.delete(id);
        }

    }
}
