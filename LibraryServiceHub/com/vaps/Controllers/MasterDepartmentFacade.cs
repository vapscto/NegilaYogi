using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibraryServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.Library;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace LibraryServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class MasterDepartmentFacade : Controller
    {
        public MasterDepartmentInterface _LibInter;
        public MasterDepartmentFacade(MasterDepartmentInterface para)
        {
            _LibInter = para;
        }
        [Route("Savedata")]
        public MasterDepartmentDTO Savedata([FromBody]MasterDepartmentDTO data)
        {
            return _LibInter.Savedata(data); 
        }

        //[HttpGet]
        [Route("getdetails/{id:int}")]
        public MasterDepartmentDTO getdetails(int id)
        {
            return _LibInter.getdetails(id);
        }
        [Route("deactiveY")]
        public MasterDepartmentDTO deactiveY([FromBody]MasterDepartmentDTO data)
        {
            return _LibInter.deactiveY(data);
        }
    }
}
