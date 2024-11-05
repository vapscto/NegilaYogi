using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Fees;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Fees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Fees
{
    [Route("api/[controller]")]
    public class ClgQuotaFeeGroupController : Controller
    {
        public ClgQuotaFeeGroupDelegate FGD = new ClgQuotaFeeGroupDelegate();

        // GET: api/values
        [HttpGet]

        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        // GET api/values/5
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public ClgQuotaFeeGroupDTO Get([FromQuery] int id)
        {
            ClgQuotaFeeGroupDTO data = new ClgQuotaFeeGroupDTO();
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.MI_Id = id;
            return FGD.getdetails(data);
        }

        //for edit
        [Route("getdetails/{id:int}")]
        public ClgQuotaFeeGroupDTO getdetail(int id)
        {
            HttpContext.Session.SetString("pageid", id.ToString()); //Set
            // id = 12;
            return FGD.getpagedetails(id);

        }

        [Route("Editdetails/{id:int}")]
        public ClgQuotaFeeGroupDTO EditDetails(int id)
        {
            HttpContext.Session.SetString("sectionid", id.ToString());
            return FGD.EditDetails(id);
        }
        // POST api/values
        [HttpPost]
        public ClgQuotaFeeGroupDTO savedetail([FromBody] ClgQuotaFeeGroupDTO Grouppage)
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
        public ClgQuotaFeeGroupDTO Delete(int id)
        {
            return FGD.deleterec(id);
        }
        [HttpPost]
        [Route("deactivate")]
        public ClgQuotaFeeGroupDTO deactvate([FromBody] ClgQuotaFeeGroupDTO id)
        {
            id.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return FGD.deactivateAcademicYear(id);
        }

        //for yearly group 


        [Route("getdpforyear")]
        public ClgQuotaFeeGroupDTO getDpData([FromBody] ClgQuotaFeeGroupDTO yrs)
        {
            //return sad.getIndependentDropDowns(ctry);
            return FGD.getIndependentDropDowns(yrs);
        }


    

    }
}
