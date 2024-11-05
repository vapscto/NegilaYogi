using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdmissionServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class AttendanceRunFacade : Controller
    {
        public AttendanceRunInterface _inter;
        public AttendanceRunFacade(AttendanceRunInterface inter)
        {
            _inter = inter;
        }

        [Route("loaddata")]
        public AttendanceRunDTO loaddata([FromBody] AttendanceRunDTO data)
        {
            return _inter.loaddata(data);
        }

        [Route("savedetails")]
        public AttendanceRunDTO savedetails([FromBody] AttendanceRunDTO data)
        {
            return _inter.savedetails(data);
        }

        [Route("griddetails")]
        public AttendanceRunDTO griddetails([FromBody] AttendanceRunDTO data)
        {
            return _inter.griddetails(data);
        }

    }
}
