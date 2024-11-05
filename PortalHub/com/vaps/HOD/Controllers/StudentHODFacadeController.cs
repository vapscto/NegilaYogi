using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Student;
using PortalHub.com.vaps.HOD.Interfaces;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.HOD.Controllers
{
    [Route("api/[controller]")]
    public class StudentHODFacadeController : Controller
    {
        public StudentHODInterface _ads;

        public StudentHODFacadeController(StudentHODInterface adstu)
        {
            _ads = adstu;
        }

        [HttpPost]
        [Route("getloaddata")]
        public ExamDTO getloaddata([FromBody]ExamDTO data)
        {
            return _ads.getloaddata(data);
        }

        [HttpPost]
        [Route("getexamdata")]
        public ExamDTO getexamdata([FromBody]ExamDTO sddto)
        {
            return _ads.getexamdata(sddto);
        }
        [HttpPost]
        [Route("getexamdetails")]
        public ExamDTO getexamdetails([FromBody]ExamDTO sddto)
        {
            return _ads.getexamdetails(sddto);
        }
        [Route("getsectiondata")]
        public ExamDTO getsectiondata([FromBody]ExamDTO sddto)
        {
            return _ads.getsectiondata(sddto);
        }
        [Route("get_classes")]
        public ExamDTO get_classes([FromBody]ExamDTO sddto)
        {
            return _ads.get_classes(sddto);
        }
        [Route("getstudentdata")]
        public ExamDTO getstudentdata([FromBody]ExamDTO sddto)
        {
            return _ads.getstudentdata(sddto);
        }

    }
}
