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
    public class FeeGroupController : Controller
    {

        FeeGroupDelegate FGD = new FeeGroupDelegate();

        // GET: api/values
        [HttpGet]

        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        // GET api/values/5
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public FeeGroupDTO Get([FromQuery] int id)
        {
            FeeGroupDTO data = new FeeGroupDTO();
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.MI_Id = id;
            return FGD.getdetails(data);
        }

        //for edit
        [Route("getdetails/{id:int}")]
        public FeeGroupDTO getdetail(int id)
        {
            HttpContext.Session.SetString("pageid", id.ToString()); //Set
            // id = 12;
            return FGD.getpagedetails(id);

        }

        [Route("Editdetails/{id:int}")]
        public FeeGroupDTO EditDetails(int id)
        {
            HttpContext.Session.SetString("sectionid", id.ToString());
            return FGD.EditDetails(id);
        }
        // POST api/values
        [HttpPost]
        public FeeGroupDTO savedetail([FromBody] FeeGroupDTO Grouppage)
        {
            Grouppage.MI_Id=Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
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
        public FeeGroupDTO Delete(int id)
        {
            return FGD.deleterec(id);
        }
        [HttpPost]
        [Route("deactivate")]
        public FeeGroupDTO deactvate([FromBody] FeeGroupDTO id)
        {
            id.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return FGD.deactivateAcademicYear(id);
        }

        //for yearly group 


        [Route("getdpforyear")]
        public FeeGroupDTO getDpData([FromBody] FeeGroupDTO yrs)
        {
            //return sad.getIndependentDropDowns(ctry);
            return FGD.getIndependentDropDowns(yrs);
        }


        // POST api/values
        [HttpPost]
        [Route("savedetailY")]
        public FeeYearlyGroupDTO savedetailY([FromBody] FeeYearlyGroupDTO GrouppageY)
        {
            GrouppageY.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            GrouppageY.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return FGD.savedetailsY(GrouppageY);
        }

        //    GET api/values/5
        [HttpGet]
        [Route("getalldetailsY/{id:int}")]
        public FeeYearlyGroupDTO getalldetailsY([FromQuery] int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return FGD.getdetailsY(id);
        }


        [HttpPost]
        [Route("deactivateY")]
        public FeeYearlyGroupDTO deactvateY([FromBody] FeeYearlyGroupDTO id)
        {
            id.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return FGD.deactivateY(id);
        }


        [Route("getdetailsY/{id:int}")]
        public FeeYearlyGroupDTO getdetailY(int id)
        {
            HttpContext.Session.SetString("pageid", id.ToString()); //Set
            // id = 12;
            return FGD.getpagedetailsY(id);

        }


        // DELETE api/values/5
        [HttpDelete]
        [Route("deletepagesY/{id:int}")]
        public FeeYearlyGroupDTO DeleteY(int id)
        {
            return FGD.deleterecY(id);
        }

        [HttpPost]
        [Route("selectacademicyear")]
        public FeeYearlyGroupDTO selectacademicyear([FromBody] FeeYearlyGroupDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return FGD.selectacade(data);
        }
        [HttpPost]
        [Route("savedataFTally")]
        public Fee_FeeGroup_CompanyMappingDTO savedataFTally([FromBody] Fee_FeeGroup_CompanyMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return FGD.savedataFTally(data);
        }
        [HttpPost]
        [Route("deletedataYYY")]
        public Fee_FeeGroup_CompanyMappingDTO deletedataYYY([FromBody] Fee_FeeGroup_CompanyMappingDTO id)
        {
            id.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return FGD.deletedataYYY(id);
        }
    }
}
