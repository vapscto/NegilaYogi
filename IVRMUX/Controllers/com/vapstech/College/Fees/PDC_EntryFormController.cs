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
    public class PDC_EntryFormController : Controller
    {
        public PDC_EntryFormDelegate FGD = new PDC_EntryFormDelegate();

        // GET: api/values
        [HttpGet]

        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        // GET api/values/5
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public PDC_EntryFormDTO Get([FromQuery] int id)
        {
            PDC_EntryFormDTO data = new PDC_EntryFormDTO();
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.MI_Id = id;
            return FGD.getdetails(data);
        }

        //for edit
        [Route("getdetails/{id:int}")]
        public PDC_EntryFormDTO getdetail(int id)
        {
            HttpContext.Session.SetString("pageid", id.ToString()); //Set
            // id = 12;
            return FGD.getpagedetails(id);

        }

        [Route("Editdetails/{id:int}")]
        public PDC_EntryFormDTO EditDetails(int id)
        {
            HttpContext.Session.SetString("sectionid", id.ToString());
            return FGD.EditDetails(id);
        }
        // POST api/values
        [HttpPost]
        public PDC_EntryFormDTO savedetail([FromBody] PDC_EntryFormDTO Grouppage)
        {
            Grouppage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            Grouppage.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return FGD.savedetails(Grouppage);
        }
        [Route("showdata")]
        public PDC_EntryFormDTO showdata([FromBody] PDC_EntryFormDTO Grouppage)
        {
            Grouppage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            Grouppage.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            Grouppage.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return FGD.showdata(Grouppage);
        }
        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("deletepages/{id:int}")]
        public PDC_EntryFormDTO Delete(int id)
        {
            return FGD.deleterec(id);
        }
        [HttpPost]
        [Route("deactivate")]
        public PDC_EntryFormDTO deactvate([FromBody] PDC_EntryFormDTO id)
        {
            id.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return FGD.deactivateAcademicYear(id);
        }

        //for yearly group 


        [Route("getdpforyear")]
        public PDC_EntryFormDTO getDpData([FromBody] PDC_EntryFormDTO yrs)
        {
            //return sad.getIndependentDropDowns(ctry);
            return FGD.getIndependentDropDowns(yrs);
        }

        [Route("selectcourse")]
        public PDC_EntryFormDTO selectcou([FromBody] PDC_EntryFormDTO Grouppage)
        {
            Grouppage.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            Grouppage.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return FGD.getbran(Grouppage);
        }


        [Route("selectbran")]
        public PDC_EntryFormDTO selectcoubran([FromBody] PDC_EntryFormDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return FGD.getcoubransem(data);
        }



        [Route("selectsem")]
        public PDC_EntryFormDTO selectsem([FromBody] PDC_EntryFormDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));



            return FGD.selectstudent(data);
        }

    }
}
