using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.College.Fees.Masters;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.College.Fee;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.College.Fees.Masters
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class FeeGroupClgController : Controller
    {
        public FeeGroupClgDelegate FGD =new FeeGroupClgDelegate();
       
        // GET: api/values
        [HttpGet]

        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        // GET api/values/5
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public FeeGroupClgDTO Get([FromQuery] int id)
        {
            FeeGroupClgDTO data = new FeeGroupClgDTO();
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.MI_Id = id;
            return FGD.getdetails(data);
        }

        //for edit
        [Route("getdetails/{id:int}")]
        public FeeGroupClgDTO getdetail(int id)
        {
            HttpContext.Session.SetString("pageid", id.ToString()); //Set
            // id = 12;
            return FGD.getpagedetails(id);

        }

        [Route("Editdetails/{id:int}")]
        public FeeGroupClgDTO EditDetails(int id)
        {
            HttpContext.Session.SetString("sectionid", id.ToString());
            return FGD.EditDetails(id);
        }
        // POST api/values
        [HttpPost]
        public FeeGroupClgDTO savedetail([FromBody] FeeGroupClgDTO Grouppage)
        {
            Grouppage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            Grouppage.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return FGD.savedetails(Grouppage);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("deletepages/{id:int}")]
        public FeeGroupClgDTO Delete(int id)
        {
            return FGD.deleterec(id);
        }
        [HttpPost]
        [Route("deactivate")]
        public FeeGroupClgDTO deactvate([FromBody] FeeGroupClgDTO id)
        {
            return FGD.deactivateAcademicYear(id);
        }

        //for yearly group 


        [Route("getdpforyear")]
        public FeeGroupClgDTO getDpData([FromBody] FeeGroupClgDTO yrs)
        {
            //return sad.getIndependentDropDowns(ctry);
            return FGD.getIndependentDropDowns(yrs);
        }


        // POST api/values
        [HttpPost]
        [Route("savedetailY")]
        public FeeYearlyGroupClgDTO savedetailY([FromBody] FeeYearlyGroupClgDTO GrouppageY)
        {
            GrouppageY.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            GrouppageY.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return FGD.savedetailsY(GrouppageY);
        }

        //    GET api/values/5
        [HttpGet]
        [Route("getalldetailsY/{id:int}")]
        public FeeYearlyGroupClgDTO getalldetailsY([FromQuery] int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return FGD.getdetailsY(id);
        }


        [HttpPost]
        [Route("deactivateY")]
        public FeeYearlyGroupClgDTO deactvateY([FromBody] FeeYearlyGroupClgDTO id)
        {
            return FGD.deactivateY(id);
        }


        [Route("getdetailsY/{id:int}")]
        public FeeYearlyGroupClgDTO getdetailY(int id)
        {
            HttpContext.Session.SetString("pageid", id.ToString()); //Set
            // id = 12;
            return FGD.getpagedetailsY(id);

        }


        // DELETE api/values/5
        [HttpDelete]
        [Route("deletepagesY/{id:int}")]
        public FeeYearlyGroupClgDTO DeleteY(int id)
        {
            return FGD.deleterecY(id);
        }

        [HttpPost]
        [Route("selectacademicyear")]
        public FeeYearlyGroupClgDTO selectacademicyear([FromBody] FeeYearlyGroupClgDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return FGD.selectacade(data);
        }

    }
}
