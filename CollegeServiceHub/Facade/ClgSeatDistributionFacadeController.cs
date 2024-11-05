using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CollegeServiceHub.Interface;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeServiceHub.Facade
{
    [Route("api/[controller]")]
    public class ClgSeatDistributionFacadeController : Controller
    {
        public ClgSeatDistributionInterface _Seatint;

        public ClgSeatDistributionFacadeController(ClgSeatDistributionInterface Seatintf)
        {
            _Seatint = Seatintf;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }


        [Route("getalldetails")]
        public ClgSeatDistributionDTO getalldetails([FromBody] ClgSeatDistributionDTO data)
        {
            return _Seatint.getalldetails(data);
        }

        [Route("getCoursedata")]
        public ClgSeatDistributionDTO getCoursedata([FromBody] ClgSeatDistributionDTO data)
        {
            return _Seatint.getCoursedata(data);
        }
        [Route("getBranchdata")]
        public ClgSeatDistributionDTO getBranchdata([FromBody] ClgSeatDistributionDTO data)
        {
            return _Seatint.getBranchdata(data);

        }
        [Route("getSemesterdata")]
        public ClgSeatDistributionDTO getSemesterdata([FromBody] ClgSeatDistributionDTO data)
        {
            return _Seatint.getSemesterdata(data);
        }
        [Route("get_Category")]
        public ClgSeatDistributionDTO get_Category([FromBody] ClgSeatDistributionDTO data)
        {
            return _Seatint.get_Category(data);
        }
        [Route("savedata")]
        public ClgSeatDistributionDTO savedata([FromBody] ClgSeatDistributionDTO data)
        {
            return _Seatint.savedata(data);
        }
        [Route("get_Seattotal")]
        public ClgSeatDistributionDTO get_Seattotal([FromBody] ClgSeatDistributionDTO data)
        {
            return _Seatint.get_Seattotal(data);
        }

        //master competitive exam
        [Route("getexamdetails")]
        public Master_Competitive_AdmExamsClgDTO getexamdetails([FromBody]Master_Competitive_AdmExamsClgDTO obj)
        {
            return _Seatint.getexamdetails(obj);
        }
        [Route("saveExamDetails")]
        public Master_Competitive_AdmExamsClgDTO saveExamDetails([FromBody] Master_Competitive_AdmExamsClgDTO obj)
        {
            return _Seatint.saveExamDetails(obj);

        }
        [Route("saveExamMapDetails")]
        public Master_Competitive_AdmExamsClgDTO saveExamMapDetails([FromBody] Master_Competitive_AdmExamsClgDTO obj)
        {
            return _Seatint.saveExamMapDetails(obj);

        }
        [Route("getexamedit/{id:int}")]
        public Master_Competitive_AdmExamsClgDTO getexamedit(int id)
        {
            return _Seatint.getexamedit(id);
        }

        [Route("getsubedit/{id:int}")]
        public Master_Competitive_AdmExamsClgDTO getsubedit(int id)
        {
            return _Seatint.getsubedit(id);
        }


        [Route("deleterecordsub/{id:int}")]
        public Master_Competitive_AdmExamsClgDTO deleterecordsub(int id)
        {
            return _Seatint.deleterecordsub(id);
        }

        [Route("deleterecord/{id:int}")]
        public Master_Competitive_AdmExamsClgDTO deleterecord(int id)
        {
            return _Seatint.deleterecord(id);
        }

    }
}
