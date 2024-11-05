using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AlumniHub.Com.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Alumni;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AlumniHub.Com.Facade
{
    [Route("api/[controller]")]
    public class ALUDASHFacade : Controller
    {
        ALUDASHInterface _aluint;
        public ALUDASHFacade(ALUDASHInterface stu)
        {
            _aluint = stu;
        }

        [HttpPost]
        [Route("getloaddata")]
        public CLGAlumniStudentDTO getdataa([FromBody]CLGAlumniStudentDTO value)
        {
            return _aluint.getloaddata(value);
        }

    }
}
