using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CollegeServiceHub.Interface;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeServiceHub.Facade
{
    [Route("api/[controller]")]
    public class CLGMasterSemisterFacadeController : Controller
    {
        CLGMasterSemisterInterface _semint;
        public CLGMasterSemisterFacadeController(CLGMasterSemisterInterface semint)
        {
            _semint = semint;
        }
        [Route("savesem")]
        public CLGMasterSemisterDTO getalldetails([FromBody] CLGMasterSemisterDTO data)
        {
            return _semint.savesem(data);
        }
        [Route("editsem")]
        public CLGMasterSemisterDTO editsem([FromBody] CLGMasterSemisterDTO data)
        {
            return _semint.editsem(data);
        }
        [Route("getdata")]
        public CLGMasterSemisterDTO getdata([FromBody] CLGMasterSemisterDTO data)
        {
            return _semint.getdata(data);
        }
        [Route("activedeactivesem")]
        public CLGMasterSemisterDTO activedeactivesem([FromBody] CLGMasterSemisterDTO data)
        {
            return _semint.activedeactivesem(data);
        }
        [Route("getOrder")]
        public CLGMasterSemisterDTO getOrder([FromBody] CLGMasterSemisterDTO data)
        {
            return _semint.getOrder(data);
        }
        
    }
}
