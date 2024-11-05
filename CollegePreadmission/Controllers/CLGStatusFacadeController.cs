using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using PreadmissionDTOs.com.vaps.College.Admission;
using CollegePreadmission.Interfaces;
using PreadmissionDTOs.com.vaps.College.Preadmission;
using PreadmissionDTOs;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegePreadmission.Controllers
{
    [Route("api/[controller]")]
    public class CLGStatusFacade : Controller
    {
        CLGStatusInterface _interface;
        public CLGStatusFacade(CLGStatusInterface interf)
        {
            _interface = interf;
        }
        [Route("Getdetails")]
        public CollegePreadmissionstudnetDto Getdetails([FromBody]CollegePreadmissionstudnetDto obj)
        {
            return _interface.Getdetails(obj);
        }
        [Route("getCourse")]
        public CollegePreadmissionstudnetDto getCourse([FromBody]CollegePreadmissionstudnetDto data)
        {
            return _interface.getCourse(data);
        }
        [Route("getBranch")]
        public CollegePreadmissionstudnetDto getBranch([FromBody] CollegePreadmissionstudnetDto dt)
        {
            return _interface.getBranch(dt);
        }
        [Route("SearchData")]
        public CollegePreadmissionstudnetDto SearchData([FromBody] CollegePreadmissionstudnetDto dt)
        {
            return _interface.SearchData(dt); 
        }


        //master competitive exam
        [Route("getexamdetails")]
        public Master_Competitive_ExamsClgDTO getexamdetails([FromBody]Master_Competitive_ExamsClgDTO obj)
        {
            return _interface.getexamdetails(obj);
        }
        [Route("saveExamDetails")]
        public Master_Competitive_ExamsClgDTO saveExamDetails([FromBody] Master_Competitive_ExamsClgDTO obj)
        {
            return _interface.saveExamDetails(obj);

        }
        [Route("saveExamMapDetails")]
        public Master_Competitive_ExamsClgDTO saveExamMapDetails([FromBody] Master_Competitive_ExamsClgDTO obj)
        {
            return _interface.saveExamMapDetails(obj);

        }
        [Route("getexamedit/{id:int}")]
        public Master_Competitive_ExamsClgDTO getexamedit(int id)
        {
            return _interface.getexamedit(id);
        }

        [Route("getsubedit/{id:int}")]
        public Master_Competitive_ExamsClgDTO getsubedit(int id)
        {
            return _interface.getsubedit(id);
        }


        [Route("deleterecordsub/{id:int}")]
        public Master_Competitive_ExamsClgDTO deleterecordsub(int id)
        {
            return _interface.deleterecordsub(id);
        }

        [Route("deleterecord/{id:int}")]
        public Master_Competitive_ExamsClgDTO deleterecord(int id)
        {
            return _interface.deleterecord(id);
        }




    }
}
