using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.College.Admission;
using PreadmissionDTOs.com.vaps.College.Admission;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.College.Admission
{
    [Route("api/[controller]")]
    public class ClgSeatDistributionController : Controller
    {

        ClgSeatDistributionDelegate _Seat = new ClgSeatDistributionDelegate();

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

        
        [Route("getalldetails/{id:int}")]
        public ClgSeatDistributionDTO getalldetails (int id)
        {
            ClgSeatDistributionDTO data = new ClgSeatDistributionDTO();
            data.MI_Id= Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Seat.getalldetails(data);
        }


        [Route("getCoursedata")]
        public ClgSeatDistributionDTO getCoursedata([FromBody]ClgSeatDistributionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));           
            return _Seat.getCoursedata(data);
        }
        [Route("getBranchdata")]
        public ClgSeatDistributionDTO getBranchdata([FromBody]ClgSeatDistributionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Seat.getBranchdata(data);
        }
        [Route("getSemesterdata")]
        public ClgSeatDistributionDTO getSemesterdata([FromBody]ClgSeatDistributionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Seat.getSemesterdata(data);
        }

        [Route("get_Category")]
        public ClgSeatDistributionDTO get_Category([FromBody] ClgSeatDistributionDTO data)
        {
            
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Seat.get_Category(data);
        }
        [Route("get_Seattotal/{id:int}")]
        public ClgSeatDistributionDTO get_Seattotal(int id)
        {
            ClgSeatDistributionDTO data = new ClgSeatDistributionDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Seat.get_Seattotal(data);
        }
        [Route("savedata")]
        public ClgSeatDistributionDTO savedata([FromBody]ClgSeatDistributionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Seat.savedata(data);
        }


        //master competitve exam
        [Route("getexamdetails")]
        public Master_Competitive_AdmExamsClgDTO getexamdetails()
        {
            Master_Competitive_AdmExamsClgDTO dto = new Master_Competitive_AdmExamsClgDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ID = UserId;
            return _Seat.getexamdetails(dto);
        }

        [Route("saveExamDetails")]
        public Master_Competitive_AdmExamsClgDTO saveExamDetails([FromBody]Master_Competitive_AdmExamsClgDTO add)
        {
            add.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            add.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            add.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _Seat.saveExamDetails(add);
        }


        [Route("getexamedit/{Id:int}")]
        public Master_Competitive_AdmExamsClgDTO getexamedit(int Id)
        {
            Master_Competitive_AdmExamsClgDTO edit = new Master_Competitive_AdmExamsClgDTO();
            edit.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return _Seat.getexamedit(Id);
        }

        [Route("getsubedit/{Id:int}")]
        public Master_Competitive_AdmExamsClgDTO getsubedit(int Id)
        {
            Master_Competitive_AdmExamsClgDTO edit = new Master_Competitive_AdmExamsClgDTO();
            edit.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return _Seat.getsubedit(Id);
        }

        [HttpDelete]
        [Route("deleterecord/{id:int}")]
        public Master_Competitive_AdmExamsClgDTO Delete(int id)
        {
            Master_Competitive_AdmExamsClgDTO del = new Master_Competitive_AdmExamsClgDTO();
            del.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Seat.deleterecord(id);
        }

        [HttpDelete]
        [Route("deleterecordsub/{id:int}")]
        public Master_Competitive_AdmExamsClgDTO deleterecordsub(int id)
        {
            Master_Competitive_AdmExamsClgDTO del = new Master_Competitive_AdmExamsClgDTO();
            del.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Seat.deleterecordsub(id);
        }

        [Route("saveExamMapDetails")]
        public Master_Competitive_AdmExamsClgDTO saveExamMapDetails([FromBody]Master_Competitive_AdmExamsClgDTO add)
        {
            add.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            add.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            add.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _Seat.saveExamMapDetails(add);
        }
    }
}
