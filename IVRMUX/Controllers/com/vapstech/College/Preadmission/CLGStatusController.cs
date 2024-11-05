using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;
using corewebapi18072016.Delegates.com.vapstech.College.Admission;
using Microsoft.AspNetCore.Http;
using IVRMUX.Delegates.com.vapstech.College.Preadmission;
using PreadmissionDTOs.com.vaps.College.Preadmission;
using PreadmissionDTOs;
using corewebapi18072016.Delegates;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Preadmission
{
    [Route("api/[controller]")]
    public class CLGStatusController : Controller
    {
        CLGStatusDelegate delegates = new CLGStatusDelegate();
        StudentApplicationDelegate sad = new StudentApplicationDelegate();
        [Route("Getdetails")]
        public CollegePreadmissionstudnetDto Getdetails()
        {
            CollegePreadmissionstudnetDto dto = new CollegePreadmissionstudnetDto();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.ASMAY_Id= Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ID = UserId;

            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return delegates.Getdetails(dto);
        }
        [Route("getCourse/{YearId:int}")]
        public CollegePreadmissionstudnetDto getCourse(int YearId)
        {
            CollegePreadmissionstudnetDto obj = new CollegePreadmissionstudnetDto();
            obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id_Preadmission"));
            obj.ASMAY_Id = YearId;
            return delegates.getCourse(obj);
        }
        [Route("getBranch")]
        public CollegePreadmissionstudnetDto getBranch([FromBody]CollegePreadmissionstudnetDto dt)
        {
            dt.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id_Preadmission"));
            return delegates.getBranch(dt);
        }
        
        [Route("SearchData")]
        public CollegePreadmissionstudnetDto getStudentOnSearchFilter([FromBody] CollegePreadmissionstudnetDto cdto)
        {
            cdto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.SearchData(cdto);
        }

        //master competitve exam
        [Route("getexamdetails")]
        public Master_Competitive_ExamsClgDTO getexamdetails()
        {
            Master_Competitive_ExamsClgDTO dto = new Master_Competitive_ExamsClgDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ID = UserId;
            return delegates.getexamdetails(dto);
        }

        [Route("saveExamDetails")]
        public Master_Competitive_ExamsClgDTO saveExamDetails([FromBody]Master_Competitive_ExamsClgDTO add)
        {
            add.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            add.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            add.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delegates.saveExamDetails(add);
        }


        [Route("getexamedit/{Id:int}")]
        public Master_Competitive_ExamsClgDTO getexamedit(int Id)
        {
            Master_Competitive_ExamsClgDTO edit = new Master_Competitive_ExamsClgDTO();
            edit.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return delegates.getexamedit(Id);
        }

        [Route("getsubedit/{Id:int}")]
        public Master_Competitive_ExamsClgDTO getsubedit(int Id)
        {
            Master_Competitive_ExamsClgDTO edit = new Master_Competitive_ExamsClgDTO();
            edit.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return delegates.getsubedit(Id);
        }

        [HttpDelete]
        [Route("deleterecord/{id:int}")]
        public Master_Competitive_ExamsClgDTO Delete(int id)
        {
            Master_Competitive_ExamsClgDTO del = new Master_Competitive_ExamsClgDTO();
            del.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.deleterecord(id);
        }

        [HttpDelete]
        [Route("deleterecordsub/{id:int}")]
        public Master_Competitive_ExamsClgDTO deleterecordsub(int id)
        {
            Master_Competitive_ExamsClgDTO del = new Master_Competitive_ExamsClgDTO();
            del.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.deleterecordsub(id);
        }

        [Route("saveExamMapDetails")]
        public Master_Competitive_ExamsClgDTO saveExamMapDetails([FromBody]Master_Competitive_ExamsClgDTO add)
        {
            add.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            add.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            add.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delegates.saveExamMapDetails(add);
        }

    }
}
