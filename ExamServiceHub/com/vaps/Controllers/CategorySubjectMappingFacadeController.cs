
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using PreadmissionDTOs.com.vaps.COE;
using PreadmissionDTOs.com.vaps.Exam;
using ExamServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class CategorySubjectMappingFacadeController : Controller
    {
        public CategorySubjectMappingInterface _ttcategory;

        public CategorySubjectMappingFacadeController(CategorySubjectMappingInterface maspag)
        {
            _ttcategory = maspag;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        
        [HttpGet]
        [Route("getdetails/{id:int}")]
        public CategorySubjectMappingDTO getorgdet(int id)
        {
            return _ttcategory.getdetails(id);
        }
     
        [HttpPost]
        [Route("savedetail")]
        public CategorySubjectMappingDTO Post([FromBody] CategorySubjectMappingDTO org)
        {
            return _ttcategory.savedetail(org);
        }


        [HttpPost]
        [Route("deactivate")]
        public CategorySubjectMappingDTO deactivate([FromBody] CategorySubjectMappingDTO org)
        {
            return _ttcategory.deactivate(org);
        }

        [Route("getalldetailsviewrecords/{id:int}")]
        public CategorySubjectMappingDTO getalldetailsviewrecords(int id)
        {
            return _ttcategory.getalldetailsviewrecords(id);
        }

        [Route("getpagedetails/{id:int}")]
        public CategorySubjectMappingDTO getpagedetails(int id)
        {
            return _ttcategory.getpageedit(id);
        }

        [Route("get_category")]
        public CategorySubjectMappingDTO get_category([FromBody] CategorySubjectMappingDTO org)
        {
            return _ttcategory.get_category(org);
        }

        [Route("get_subjects")]
        public CategorySubjectMappingDTO get_subjects([FromBody] CategorySubjectMappingDTO org)
        {
            return _ttcategory.get_subjects(org);
        }

        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public CategorySubjectMappingDTO Deleterec(int id)
        {
            return _ttcategory.deleterec(id);
        }

        /* Category Dates Mapping */
        [Route("OnLoadCategoryDates")]
        public CategorySubjectMappingDTO OnLoadCategoryDates([FromBody] CategorySubjectMappingDTO org)
        {
            return _ttcategory.OnLoadCategoryDates(org);
        }

        [Route("get_categoryDates")]
        public CategorySubjectMappingDTO get_categoryDates([FromBody] CategorySubjectMappingDTO org)
        {
            return _ttcategory.get_categoryDates(org);
        }

        [Route("savedatadates")]
        public CategorySubjectMappingDTO savedatadates([FromBody] CategorySubjectMappingDTO org)
        {
            return _ttcategory.savedatadates(org);
        }
    }
}