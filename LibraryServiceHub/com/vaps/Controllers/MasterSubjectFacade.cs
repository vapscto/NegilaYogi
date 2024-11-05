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
    public class MasterSubjectFacade : Controller
    {
        public MasterSubjectInterface _objinter;
        public MasterSubjectFacade(MasterSubjectInterface para)
        {
            _objinter = para;
        }

        [Route("Savedata")]
        public MasterSubject_DTO Savedata([FromBody]MasterSubject_DTO data)
        {
            return _objinter.Savedata(data);
        }

        [Route("getdetails/{id:int}")]
        public MasterSubject_DTO getdetails(int id)
        {
            return _objinter.getdetails(id);
        }

        [Route("deactiveY")]
        public MasterSubject_DTO deactiveY([FromBody] MasterSubject_DTO data)
        {
            return _objinter.deactiveY(data);
        }
    }
}
