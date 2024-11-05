using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using VisitorsManagementServiceHub.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VisitorsManagementServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class LateInStudentFacade : Controller
    {

        LateInStudentInterface _objInter;

        public LateInStudentFacade(LateInStudentInterface parameter)
        {
            _objInter = parameter;
        }

        [Route("loaddata")]
        public Task<LateInStudent_DTO> loaddata([FromBody] LateInStudent_DTO data)
        {
            return _objInter.loaddata(data);
        }

        [Route("get_class")]
        public LateInStudent_DTO get_class([FromBody] LateInStudent_DTO data)
        {
            return _objInter.get_class(data);
        }

        [Route("get_section")]
        public LateInStudent_DTO get_section([FromBody] LateInStudent_DTO data)
        {
            return _objInter.get_section(data);
        }

        [Route("get_student")]
        public LateInStudent_DTO get_student([FromBody] LateInStudent_DTO data)
        {
            return _objInter.get_student(data);
        }

        [Route("savedata")]
        public Task<LateInStudent_DTO> savedata([FromBody] LateInStudent_DTO data)
        {
            return _objInter.savedata(data);
        }

        [Route("editdata")]
        public LateInStudent_DTO editdata([FromBody] LateInStudent_DTO data)
        {
            return _objInter.editdata(data);
        }
        [Route("deactive")]
        public LateInStudent_DTO deactive([FromBody] LateInStudent_DTO data)
        {
            return _objInter.deactive(data);
        }

        
    }
}
