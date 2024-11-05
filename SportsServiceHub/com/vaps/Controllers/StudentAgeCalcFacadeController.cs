using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Sports;
using SportsServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SportsServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class StudentAgeCalcFacadeController : Controller
    {
        StudentAgeCalcInterface _interface;
        public StudentAgeCalcFacadeController(StudentAgeCalcInterface interfaces)
        {
            _interface = interfaces;
        }
        [Route("Getdetails")]
        public StudentAgeCalcDTO getDetails([FromBody]StudentAgeCalcDTO data)
        {
            return _interface.Getdetails(data);
        }
        [Route("getStudents")]
        public StudentAgeCalcDTO getStudents([FromBody]StudentAgeCalcDTO data)
        {
            return _interface.getStudents(data);
        }
        [Route("save")]
        public StudentAgeCalcDTO save([FromBody]StudentAgeCalcDTO data)
        {
            return _interface.saveRecord(data);
        }
        [Route("report")]
        public StudentAgeCalcDTO report([FromBody]StudentAgeCalcDTO data)
        {
            return _interface.report(data);
        }
        
        [Route("Get_Class_House")]
        public StudentAgeCalcDTO Get_Class_House(StudentAgeCalcDTO data)
        {
            return _interface.Get_Class_House(data);
        }

    }
}
