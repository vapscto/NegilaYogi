
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.COE;
using PreadmissionDTOs.com.vaps.COE;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.Exam;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Exam
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class CategorySubjectMappingController : Controller
    {
        CategorySubjectMappingDelegates objdelegate = new CategorySubjectMappingDelegates();


        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5    
        [HttpGet]
        [Route("getalldetails")]
        public CategorySubjectMappingDTO Get([FromQuery] int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            CategorySubjectMappingDTO obj = objdelegate.getdetails(id);
            obj.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return obj;
           // return objdelegate.getdetails(id);
        }

        [HttpPost]
        [Route("savedetail")]
        public CategorySubjectMappingDTO savedetail([FromBody] CategorySubjectMappingDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            categorypage.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return objdelegate.savedetail(categorypage);
        }


        [HttpPost]
        [Route("deactivate")]
        public CategorySubjectMappingDTO deactivate([FromBody] CategorySubjectMappingDTO categorypage)
        {
            //  categorypage.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.deactivate(categorypage);
        }

        [Route("getalldetailsviewrecords/{id:int}")]
        public CategorySubjectMappingDTO getalldetailsviewrecords(int id)
        {
            return objdelegate.getalldetailsviewrecords(id);
        }

        [Route("getdetails/{id:int}")]
        public CategorySubjectMappingDTO getdetail(int id)
        {
            return objdelegate.getpagedetails(id);
        }

        [Route("get_category")]
        public CategorySubjectMappingDTO get_category([FromBody] CategorySubjectMappingDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.get_category(categorypage);
        }

        [Route("get_subjects")]
        public CategorySubjectMappingDTO get_subjects([FromBody] CategorySubjectMappingDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.get_subjects(categorypage);
        }

        [HttpDelete]
        [Route("deletepages/{id:int}")]
        public CategorySubjectMappingDTO deletepages(int id)
        {
            return objdelegate.deleterec(id);
        }

        /* Category Date Mapping */
        [Route("OnLoadCategoryDates/{id:int}")]
        public CategorySubjectMappingDTO OnLoadCategoryDates(int id)
        {
            CategorySubjectMappingDTO data = new CategorySubjectMappingDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.OnLoadCategoryDates(data);
        }

        [Route("get_categoryDates")]
        public CategorySubjectMappingDTO get_categoryDates([FromBody] CategorySubjectMappingDTO categorypage)
        { 
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.get_categoryDates(categorypage);
        } 

        [Route("savedatadates")]
        public CategorySubjectMappingDTO savedatadates([FromBody] CategorySubjectMappingDTO categorypage)
        { 
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            categorypage.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return objdelegate.savedatadates(categorypage);
        } 
    }
}
