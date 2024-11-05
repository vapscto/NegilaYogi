using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.Student.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.Student;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.Student.Controllers
{
    [Route("api/[controller]")]
    public class StudentHallticketFacadeController : Controller
    {
        public StudentHallticketInterface _interface;

        public StudentHallticketFacadeController(StudentHallticketInterface _inter)
        {
            _interface = _inter;
        }

        [Route("GetLoadData")]
        public StudentHallticketDTO GetLoadData([FromBody] StudentHallticketDTO data)
        {
            return _interface.GetLoadData(data);
        }

        [Route("GetExamDetails")]
        public StudentHallticketDTO GetExamDetails([FromBody] StudentHallticketDTO data)
        {
            return _interface.GetExamDetails(data);
        }

        [Route("GetReport")]
        public StudentHallticketDTO GetReport([FromBody] StudentHallticketDTO data)
        {
            return _interface.GetReport(data);
        }
    }
}
