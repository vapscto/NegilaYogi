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
    [Route("api/[controller]")]
    [ValidateAntiForgeryToken]
    public class FeeClassCategoryController : Controller
    {

        FeeClassCategoryDelegates FGD = new FeeClassCategoryDelegates();

        // GET: api/values
        [HttpGet]

        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        // GET api/values/5
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public FeeClassCategoryDTO Get( int id)
        {
            FeeClassCategoryDTO data = new FeeClassCategoryDTO();
            data.MI_Id= Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_ID= Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return FGD.getdetails(data);
        }

        //for edit
        [Route("getdetails/{id:int}")]
        public FeeClassCategoryDTO getdetails(int id)
        {
            HttpContext.Session.SetString("pageid", id.ToString()); //Set
            // id = 12;
            return FGD.getpagedetails(id);

        }

        [Route("Editdetails/{id:int}")]
        public FeeClassCategoryDTO EditDetails(int id)
        {
            HttpContext.Session.SetString("sectionid", id.ToString());
            return FGD.EditDetails(id);
        }
        // POST api/values
        [HttpPost]
        public FeeClassCategoryDTO savedetail([FromBody] FeeClassCategoryDTO Grouppage)
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
  
        [Route("deletepages/{id:int}")]
        public FeeClassCategoryDTO Delete(int id)
        {
            return FGD.deleterec(id);
        }
        [HttpPost]
        [Route("deactivate")]
        public FeeClassCategoryDTO deactvate([FromBody] FeeClassCategoryDTO id)
        {
            return FGD.deactivateAcademicYear(id);
        }

        //for yearly group 


        [Route("getdpforyear")]
        public FeeClassCategoryDTO getDpData([FromBody] FeeClassCategoryDTO yrs)
        {
            //return sad.getIndependentDropDowns(ctry);
            return FGD.getIndependentDropDowns(yrs);
        }


        // POST api/values
        [HttpPost]
        [Route("savedetailY")]
        public FeeYearlyClassCategoryDTO savedetailY([FromBody] FeeYearlyClassCategoryDTO GrouppageY)
        {
            GrouppageY.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            GrouppageY.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return FGD.savedetailsY(GrouppageY);
        }

        //    GET api/values/5
        [HttpGet]
        [Route("getalldetailsY/{id:int}")]
        public FeeYearlyClassCategoryDTO getalldetailsY([FromQuery] int id)
        {

            return FGD.getdetailsY(id);
        }


        [HttpPost]
        [Route("deactivateY")]
        public FeeYearlyClassCategoryDTO deactvateY([FromBody] FeeYearlyClassCategoryDTO id)
        {
            return FGD.deactivateY(id);
        }


        [Route("getdetailsY/{id:int}")]
        public FeeYearlyClassCategoryDTO getdetailsY(int id)
        {

            HttpContext.Session.SetString("pageid", id.ToString()); //Set
            // id = 12;
            return FGD.getpagedetailsY(id);

        }

        // DELETE api/values/5
        
        [Route("deletepagesY/{id:int}")]
        public FeeYearlyClassCategoryDTO DeleteY(int id)
        {
            return FGD.deleterecY(id);
        }
      
        [Route("Loaddata")]
        public FeeYearlyClassCategoryDTO Loaddata([FromBody] FeeYearlyClassCategoryDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = data.ASMAY_Id;
            data.FMCC_Id = data.FMCC_Id;
            return FGD.loaddata(data);
        }

    }
}


