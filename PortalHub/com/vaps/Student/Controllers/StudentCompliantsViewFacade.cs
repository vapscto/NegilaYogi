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
    public class StudentCompliantsViewFacade : Controller
    {
        public StudentCompliantsViewInterface _inter;
        public StudentCompliantsViewFacade(StudentCompliantsViewInterface inter)
        {
            _inter = inter;
        }

        [Route("loaddata")]
        public Task<StudentCompliantsView_DTO> loaddata([FromBody] StudentCompliantsView_DTO data)
        {
            return _inter.loaddata(data);
        }

        [Route("report1")]
        public Task<StudentCompliantsView_DTO> report1([FromBody] StudentCompliantsView_DTO data)
        {
            return _inter.report1(data);
        }
    }
}
