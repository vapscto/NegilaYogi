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
    public class FeeMasterTermsController : Controller 
    {


        FeeMasterTermsDelegate FMTD = new FeeMasterTermsDelegate();

        // GET: api/values
        [HttpGet]

        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        // GET api/values/5
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public FeeTermDTO Get([FromQuery] int id)
        {
           
            FeeTermDTO data = new FeeTermDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.USER_ID = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_ID = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return FMTD.getdetails(data);
        }

        //for edit
        [Route("getdetails/{id:int}")]
        public FeeTermDTO getdetail(int id)
        {     
                                                                 
       
            return FMTD.getpagedetails(id);

        }

        [Route("Editdetails/{id:int}")]
        public FeeTermDTO EditDetails(int id)
        {
            HttpContext.Session.SetString("sectionid", id.ToString());
            return FMTD.EditDetails(id);
        }
        // POST api/values
        [HttpPost]
        public FeeTermDTO savedetail([FromBody] FeeTermDTO Grouppage)
        {
            Grouppage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return FMTD.savedetails(Grouppage);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("deletepages/{id:int}")]
        public FeeTermDTO Delete(int id)
        {
            return FMTD.deleterec(id);
        }
        [HttpPost]
        [Route("deactivate")]
        public FeeTermDTO deactvate([FromBody] FeeTermDTO id)
        {
            id.ASMAY_ID = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return FMTD.deactivateAcademicYear(id);
        }
        // POST api/values
        //extra
        [Route("savedetailfourth")]
        public FeeMasterTermHeadsDTO savedetailfourth([FromBody] FeeMasterTermHeadsDTO GrouppageY)
        {
            GrouppageY.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            GrouppageY.USER_ID = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return FMTD.savedetailfourth(GrouppageY);
        }
        [HttpPost]
        [Route("savedetailY")]
        public FeeMasterTermHeadsDTO savedetailY([FromBody] FeeMasterTermHeadsDTO GrouppageY)
        {
            GrouppageY.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return FMTD.savedetailsY(GrouppageY);
        }

        //    GET api/values/5
        [HttpGet]
        [Route("getalldetailsY/{id:int}")]
        public FeeMasterTermHeadsDTO getalldetailsY([FromQuery] int id)
        {
            return FMTD.getdetailsY(id);
        }


        [HttpPost]
        [Route("deactivateY")]
        public FeeMasterTermHeadsDTO deactvateY([FromBody] FeeMasterTermHeadsDTO id)
        {
            return FMTD.deactivateY(id);
        }


        [Route("getdetailsY/{id:int}")]
        public FeeMasterTermHeadsDTO getdetailY(int id)
        {
            HttpContext.Session.SetString("pageid", id.ToString()); //Set
            // id = 12;
            return FMTD.getpagedetailsY(id);

        }
        [Route("getdetailsDYthird/{id:int}")]
        public FeeMasterTermFeeHeadsDueDateDTO getdetailsDY(int id)
        {
            HttpContext.Session.SetString("pageid", id.ToString()); //Set
           
            return FMTD.getdetailsDY(id);

        }
        [Route("getdetailsDYfourth/{id:int}")]
        public FeeMasterTermFeeHeadsDueDateDTO getdetailsDYfourth(int id)
        {
            HttpContext.Session.SetString("pageid", id.ToString()); //Set

            return FMTD.getdetailsDYfourth(id);

        }

       
        //third tab
        //fourth
        [Route("DeleteYss/{id:int}")]
        public FeeMasterTermHeadsDTO DeleteYss(int id)
        {
            FeeMasterTermHeadsDTO data = new FeeMasterTermHeadsDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.FMTP_Id = id;
            return FMTD.DeleteYss(data);
        }
        [Route("Getduedates/")]
        public FeeMasterTermHeadsDTO[] Getduedates([FromBody] FeeMasterTermHeadsDTO MMD) // for head and term installments getting
        {

            MMD.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return FMTD.Getduedates(MMD);
        }
        // POST api/values
        [HttpPost]
        [Route("savedetailDDD/")]
        public FeeMasterTermHeadsDTO savedetailDDD([FromBody] FeeMasterTermHeadsDTO Grouppage)
        {
            Grouppage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return FMTD.savedetailDDD(Grouppage);
        }
        // DELETE api/values/5
        [HttpPost]
        [Route("deletepagesY")]
        public FeeMasterTermHeadsDTO DeleteY([FromBody] FeeMasterTermHeadsDTO data)
        {
           
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
           

            return FMTD.deleterecY(data);
        }
        [HttpDelete]
        [Route("deletepagesthird/{id:int}")]
        public FeeMasterTermFeeHeadsDueDateDTO deletepagesthird(int id)
        {
            return FMTD.deletepagesthird(id);
        }
    }
}
