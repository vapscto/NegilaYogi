using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CollegeServiceHub.Interface;
using PreadmissionDTOs.com.vaps.admission;
using PreadmissionDTOs.com.vaps.College.Admission;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeServiceHub.Facade
{
    [Route("api/[controller]")]
    public class ClgSectionAllotmentFacadeController : Controller
    {
        public ClgSectionAllotmentInterface _enq;
        public ClgSectionAllotmentFacadeController(ClgSectionAllotmentInterface Instit)
        {
            _enq = Instit;
        }

        [Route("getAllDetails")]
        public ClgYearWiseStudentDTO Getdata([FromBody]ClgYearWiseStudentDTO sct)
        {
            return _enq.GetDropDownList(sct);
        }

        // get list by year
        [HttpGet]
        [Route("getStudentdetailsByYear/{id:int}")]
        public ClgYearWiseStudentDTO getStudentdetailsByYear(long id)
        {
            return _enq.GetStudentListByYear(id);
        }


        // POST api/values
        [HttpPost]
        public ClgYearWiseStudentDTO saveSctionAllotment([FromBody] ClgYearWiseStudentDTO sct)
        {
            return _enq.saveSctionAllotment(sct);
        }

        [Route("GetstudentdetailsbyYearandclass")]
        public ClgYearWiseStudentDTO GetstudentdetailsbyYearandclass([FromBody] ClgYearWiseStudentDTO sct)
        {
            return _enq.GetstudentdetailsbyYearandclass(sct);
        }


        [Route("Getbranch")]
        public ClgYearWiseStudentDTO Getbranch([FromBody] ClgYearWiseStudentDTO sct)
        {
            return _enq.Getbranch(sct);
        }
        [Route("Get_semister")]
        public ClgYearWiseStudentDTO Get_semister([FromBody] ClgYearWiseStudentDTO sct)
        {
            return _enq.Get_semister(sct);
        }

        
        [Route("Get_academiccourse")]
        public ClgYearWiseStudentDTO Get_academiccourse([FromBody] ClgYearWiseStudentDTO sct)
        {
            return _enq.Get_academiccourse(sct);
        }




        [Route("GetPromocourse")]
        public ClgYearWiseStudentDTO GetPromocourse([FromBody] ClgYearWiseStudentDTO sct)
        {
            return _enq.GetPromocourse(sct);
        }
        [Route("GetPromobranch")]
        public ClgYearWiseStudentDTO GetPromobranch([FromBody] ClgYearWiseStudentDTO sct)
        {
            return _enq.GetPromobranch(sct);
        }

        [Route("GetPromosem")]
        public ClgYearWiseStudentDTO GetPromosem([FromBody] ClgYearWiseStudentDTO sct)
        {
            return _enq.GetPromosem(sct);
        }
        [Route("promsemonchange")]
        public ClgYearWiseStudentDTO promsemonchange([FromBody] ClgYearWiseStudentDTO sct)
        {
            return _enq.promsemonchange(sct);
        }


        

        [Route("GetStudentListByURN")]
        public ClgYearWiseStudentDTO GetStudentListByURN([FromBody] ClgYearWiseStudentDTO data)
        {
            return _enq.GetStudentListByURN(data);
        }
        [Route("GetStudentListByURNsave")]
        public ClgYearWiseStudentDTO GetStudentListByURNsave([FromBody]ClgYearWiseStudentDTO data)
        {
            return _enq.GetStudentListByURNsave(data);
        }

        //Year loss section

        [Route("OnChangeyearlossAcademic")]
        public ClgYearWiseStudentDTO OnChangeyearlossAcademic([FromBody] ClgYearWiseStudentDTO data)
        {           
            return _enq.OnChangeyearlossAcademic(data);
        }
        [Route("Getyearlossbranch")]
        public ClgYearWiseStudentDTO Getyearlossbranch([FromBody] ClgYearWiseStudentDTO data)
        {            
            return _enq.Getyearlossbranch(data);
        }
        [Route("Getyearlosssem")]
        public ClgYearWiseStudentDTO Getyearlosssem([FromBody] ClgYearWiseStudentDTO data)
        {          
            return _enq.Getyearlosssem(data);
        }
        [Route("GetStudentListByYear_yearloss1")]
        public ClgYearWiseStudentDTO GetStudentListByYear_yearloss1([FromBody] ClgYearWiseStudentDTO data)
        {
            return _enq.GetStudentListByYear_yearloss1(data);
        }
        
    }
}
