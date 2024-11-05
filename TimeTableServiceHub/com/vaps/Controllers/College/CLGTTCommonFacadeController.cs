using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using TimeTableServiceHub.Interfaces;
using PreadmissionDTOs.com.vaps.TT;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TimeTableServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class CLGTTCommonFacadeController : Controller
    {
        public CLGTTCommonInterface _acd;
        public CLGTTCommonFacadeController(CLGTTCommonInterface acdm)
        {
            _acd = acdm;
        }
      
        [HttpPost]
        [Route("getBranch")]
        public CLGTTCommonDTO getBranch([FromBody]CLGTTCommonDTO data)
        {
            return _acd.getBranch(data);
          
        }
        [HttpPost]
        [Route("getcourse_catg")]
        public CLGTTCommonDTO getcourse_catg([FromBody]CLGTTCommonDTO data)
        {
            return _acd.getcourse_catg(data);
          
        }
        [HttpPost]
        [Route("getbranch_catg")]
        public CLGTTCommonDTO getbranch_catg([FromBody]CLGTTCommonDTO data)
        {
            return _acd.getbranch_catg(data);
          
        }
        [HttpPost]
        [Route("multplegetbranch_catg")]
        public CLGTTCommonDTO multplegetbranch_catg([FromBody]CLGTTCommonDTO data)
        {
            return _acd.multplegetbranch_catg(data);
          
        }
        [HttpPost]
        [Route("get_semister")]
        public CLGTTCommonDTO get_semister([FromBody]CLGTTCommonDTO data)
        {
            return _acd.get_semister(data);
          
        }

        [HttpPost]
        [Route("multget_semister")]
        public CLGTTCommonDTO multget_semister([FromBody]CLGTTCommonDTO data)
        {
            return _acd.multget_semister(data);
          
        }
        [HttpPost]
        [Route("get_section")]
        public CLGTTCommonDTO get_section([FromBody]CLGTTCommonDTO data)
        {
            return _acd.get_section(data);
          
        }
        [HttpPost]
        [Route("get_staff")]
        public CLGTTCommonDTO get_staff([FromBody]CLGTTCommonDTO data)
        {
            return _acd.get_staff(data);
          
        }
        [HttpPost]
        [Route("get_subject")]
        public CLGTTCommonDTO get_subject([FromBody]CLGTTCommonDTO data)
        {
            return _acd.get_subject(data);
          
        }
        [HttpPost]
        [Route("get_subject_onsec")]
        public CLGTTCommonDTO get_subject_onsec([FromBody]CLGTTCommonDTO data)
        {
            return _acd.get_subject_onsec(data);
          
        }
        [HttpPost]
        [Route("get_semday")]
        public CLGTTCommonDTO get_semday([FromBody]CLGTTCommonDTO data)
        {
            return _acd.get_semday(data);
          
        }
        [HttpPost]
        [Route("get_staffaca")]
        public CLGTTCommonDTO get_staffaca([FromBody]CLGTTCommonDTO data)
        {
            return _acd.get_staffaca(data);
          
        }

        [HttpPost]
        [Route("get_course_onstaff")]
        public CLGTTCommonDTO get_course_onstaff([FromBody]CLGTTCommonDTO data)
        {
            return _acd.get_course_onstaff(data);
          
        }

        [HttpPost]
        [Route("get_branch_onstaff")]
        public CLGTTCommonDTO get_branch_onstaff([FromBody]CLGTTCommonDTO data)
        {
            return _acd.get_branch_onstaff(data);
          
        }
        [HttpPost]
        [Route("get_sem_onstaff")]
        public CLGTTCommonDTO get_sem_onstaff([FromBody]CLGTTCommonDTO data)
        {
            return _acd.get_sem_onstaff(data);
          
        }
        [HttpPost]
        [Route("get_sec_onstaff")]
        public CLGTTCommonDTO get_sec_onstaff([FromBody]CLGTTCommonDTO data)
        {
            return _acd.get_sec_onstaff(data);
          
        }
        [HttpPost]
        [Route("get_subject_onstaff")]
        public CLGTTCommonDTO get_subject_onstaff([FromBody]CLGTTCommonDTO data)
        {
            return _acd.get_subject_onstaff(data);
          
        }

        [HttpPost]
        [Route("get_subjecttab3")]
        public CLGTTCommonDTO get_subjecttab3([FromBody]CLGTTCommonDTO data)
        {
            return _acd.get_subjecttab3(data);
          
        }

        [HttpPost]
        [Route("get_course_onsubject")]
        public CLGTTCommonDTO get_course_onsubject([FromBody]CLGTTCommonDTO data)
        {
            return _acd.get_course_onsubject(data);
          
        }

        [HttpPost]
        [Route("get_branch_onsubject")]
        public CLGTTCommonDTO get_branch_onsubject([FromBody]CLGTTCommonDTO data)
        {
            return _acd.get_branch_onsubject(data);
          
        }
        [HttpPost]
        [Route("get_sem_onsubject")]
        public CLGTTCommonDTO get_sem_onsubject([FromBody]CLGTTCommonDTO data)
        {
            return _acd.get_sem_onsubject(data);
          
        }
        [HttpPost]
        [Route("get_sec_onsubject")]
        public CLGTTCommonDTO get_sec_onsubject([FromBody]CLGTTCommonDTO data)
        {
            return _acd.get_sec_onsubject(data);
          
        }
        [HttpPost]
        [Route("get_staff_onsubject")]
        public CLGTTCommonDTO get_staff_onsubject([FromBody]CLGTTCommonDTO data)
        {
            return _acd.get_staff_onsubject(data);
          
        } 
        
    }
}
