using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.College.Fees;
using PreadmissionDTOs.com.vaps.College.Fees;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.College.Fees
{
    [Route("api/[controller]")]
    public class MasterClgFeePrevilegeController : Controller
    {
        MasterClgFeePrevilegeDelegate FID = new MasterClgFeePrevilegeDelegate();
        // FID = new FeeInstallmentDelegate();
        // GET: api/values
        [HttpGet]

        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        // GET api/values/5
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public MasterClgFeePrevilegeDTO getalldetails(int id)
        {
            MasterClgFeePrevilegeDTO data = new MasterClgFeePrevilegeDTO();

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            //data.ASMAY_Id = ASMAY_Id;

            return FID.getdetails(data);
        }


        [Route("getusername/{id:int}")]
        public MasterClgFeePrevilegeDTO getusername(int id)
        {
            MasterClgFeePrevilegeDTO data = new MasterClgFeePrevilegeDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMRT_Id = id;
            return FID.getusername(data);

        }

        [Route("delete/{id:int}")]
        public MasterClgFeePrevilegeDTO delete(int id)
        {
            return FID.delete(id);
        }

        [HttpPost]
        [Route("savedetail")]
        public MasterClgFeePrevilegeDTO savedetail([FromBody] MasterClgFeePrevilegeDTO categorypage)
        {
            // categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return FID.savedetail(categorypage);
        }


        [Route("edit/{id:int}")]
        public MasterClgFeePrevilegeDTO edit(int id)
        {
            MasterClgFeePrevilegeDTO data = new MasterClgFeePrevilegeDTO();
           int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data= FID.edit(id);
            data.ASMAY_Id=ASMAY_Id;
            return data;
        }

        [Route("Deletedetails")]
        public MasterClgFeePrevilegeDTO Deletedetails([FromBody]MasterClgFeePrevilegeDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return FID.deleterec(data);
        }

    }
}
