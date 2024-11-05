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
    public class SportsStudentHouseMappingFacade : Controller
    {
        // GET: api/<controller>
        SportsStudentHouseMappingInterface _interface;
        public SportsStudentHouseMappingFacade(SportsStudentHouseMappingInterface interfaces)
        {
            _interface = interfaces;
        }

        [Route("getdetails")]
        public SPCC_Student_House_DTO getdetails([FromBody]SPCC_Student_House_DTO data)
        {
            return _interface.getdetails(data);
        }

        [Route("get_class")]
        public SPCC_Student_House_DTO get_class([FromBody]SPCC_Student_House_DTO data)
        {
            return _interface.get_class(data);
        }

        [Route("get_section")]
        public SPCC_Student_House_DTO get_section([FromBody]SPCC_Student_House_DTO data)
        {
            return _interface.get_section(data);
        }

        [Route("get_student")]
        public SPCC_Student_House_DTO get_student([FromBody]SPCC_Student_House_DTO data)
        {
            return _interface.get_student(data);
        }

        [Route("get_student_info")]
        public SPCC_Student_House_DTO get_student_info([FromBody]SPCC_Student_House_DTO data)
        {
            return _interface.get_student_info(data);
        }

        [Route("saveRecord")]
        public SPCC_Student_House_DTO saveRecord([FromBody]SPCC_Student_House_DTO data)
        {
            return _interface.saveRecord(data);
        }

        [Route("EditRecord")]
        public SPCC_Student_House_DTO EditRecord([FromBody]SPCC_Student_House_DTO dTO)
        {
            return _interface.EditRecord(dTO);
        }

        [Route("deactivate")]
        public SPCC_Student_House_DTO deactivate([FromBody]SPCC_Student_House_DTO data)
        {
            return _interface.deactivate(data);
        }

        
    }
}
