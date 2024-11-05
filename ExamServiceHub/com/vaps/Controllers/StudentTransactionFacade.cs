using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExamServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class StudentTransactionFacade : Controller
    {
        StudentTransactionInterface _stud;
        public StudentTransactionFacade(StudentTransactionInterface stud)
        {
            _stud = stud;

        }
        [Route("getDetails")]
        public StudentTransactionDTO getDetails([FromBody] StudentTransactionDTO data)
        {
            return _stud.getDetails(data);
        }
        [Route("onchangeyear")]
        public StudentTransactionDTO onchangeyear([FromBody] StudentTransactionDTO dto)
        {           
            return _stud.onchangeyear(dto);
        }
        [Route("onchangeclass")]
        public StudentTransactionDTO onchangeclass([FromBody] StudentTransactionDTO data)
        {
            return _stud.onchangeclass(data);
        }
        [Route("onchangesection")]
        public StudentTransactionDTO onchangesection([FromBody] StudentTransactionDTO data)
        {
            return _stud.onchangesection(data);
        }
        [Route("onchangeskills")]
        public StudentTransactionDTO onchangeskills([FromBody] StudentTransactionDTO data)
        {
            return _stud.onchangeskills(data);
        }
        [Route("onchangeactivites")]
        public StudentTransactionDTO onchangeactivites([FromBody] StudentTransactionDTO data)
        {
            return _stud.onchangeactivites(data);
        }

        [Route("getStudentList")]
        public StudentTransactionDTO getStudentList([FromBody] StudentTransactionDTO data)
        {
            return _stud.getStudentList(data);
        }
        //getmobiletList
        [Route("getmobiletList")]
        public StudentTransactionDTO getmobiletList([FromBody] StudentTransactionDTO data)
        {
            return _stud.getmobiletList(data);
        }
        [Route("save")]
        public StudentTransactionDTO save([FromBody] StudentTransactionDTO obj)
        {
            return _stud.save(obj);
        }

        [Route("onchangeactivitesskillflag")]
        public StudentTransactionDTO onchangeactivitesskillflag([FromBody] StudentTransactionDTO obj)
        {
            return _stud.onchangeactivitesskillflag(obj);
        }
        
    }
}
