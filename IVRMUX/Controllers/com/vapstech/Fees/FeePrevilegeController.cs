using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.Fees;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class FeePrevilegeController : Controller
    {
        FeePrevilegeDelegate FID = new FeePrevilegeDelegate();
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
        public FeePrevilegeDTO Get(int id)
        {
            FeePrevilegeDTO data = new FeePrevilegeDTO();

           data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.ASMAY_Id = ASMAY_Id;

            return FID.getdetails(data);
        }


        [Route("getusername/{id:int}")]
        public FeePrevilegeDTO getusername(int id)
        {
            FeePrevilegeDTO data = new FeePrevilegeDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMRT_Id = id;
            return FID.getusername(data);

        }

        [Route("delete/{id:int}")]
        public FeePrevilegeDTO delete(int id)
        {
            return FID.delete(id);
        }

        [HttpPost]
        [Route("savedetail")]
        public FeePrevilegeDTO savedetail([FromBody] FeePrevilegeDTO categorypage)
        {
            // categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return FID.savedetail(categorypage);
        }

        [Route("getheads")]
        public FeePrevilegeDTO fillheadss([FromBody] FeePrevilegeDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //categorypage.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return FID.fillheads(categorypage);
        }


        [Route("edit/{id:int}")]
        public FeePrevilegeDTO edit(int id)
        {
            FeePrevilegeDTO data = new FeePrevilegeDTO();
            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data= FID.edit(id);
            data.ASMAY_Id = ASMAY_Id;
            return data;
        }

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete]
        //[Route("deletepages/{id:int}")]
        //public FeePrevilegeDTO Delete(int id)
        //{
        //    return FID.deleterec(id);
        //}
        //[Route("deletepagesY/{id:int}")]
        //public FeePrevilegeDTO DeleteY(int id)
        //{
        //    return FID.deleterecY(id);
        //}
        //[HttpPost]
        //[Route("deactivate")]
        //public FeePrevilegeDTO deactvate([FromBody] FeePrevilegeDTO id)
        //{
        //    return FID.deactivateAcademicYear(id);
        //}


        //[Route("GetWrittenTestMarks/")]
        //public FeePrevilegeDTO[] GetWrittenTestMarks([FromBody] FeePrevilegeDTO MMD)
        //{
        //    return FID.GetWrittenTestMarks(MMD);
        //}

        //[Route("getdpforyear")]
        //public FeePrevilegeDTO getDpData([FromBody] FeePrevilegeDTO yrs)
        //{
        //    //return sad.getIndependentDropDowns(ctry);
        //    return FID.getIndependentDropDowns(yrs);
        //}

        //[Route("Getduedates/")]
        //public FeePrevilegeDTO[] Getduedates([FromBody] FeePrevilegeDTO MMD)
        //{
        //    return FID.Getduedates(MMD);
        //}
        //// POST api/values
        //[HttpPost]
        //[Route("savedetailDDD/")]
        //public FeePrevilegeDTO savedetailDDD([FromBody] FeePrevilegeDTO Grouppage)
        //{
        //    Grouppage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    return FID.savedetailDDD(Grouppage);
        //}


    }
}
