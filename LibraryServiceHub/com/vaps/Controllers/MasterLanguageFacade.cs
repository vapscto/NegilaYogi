using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Library;
using LibraryServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace LibraryServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class MasterLanguageFacade : Controller
    {
        public MasterLanguageInterface _objInter;
        public MasterLanguageFacade(MasterLanguageInterface para)
        {
            _objInter = para;
        }

        [Route("Savedata")]
        public MasterLanguageDTO Savedata([FromBody]MasterLanguageDTO data)
        {
            return _objInter.Savedata(data);
        }
        [Route("getdetails/{id:int}")]
        public MasterLanguageDTO getdetails(int id)
        {
            return _objInter.getdetails(id);
        }
       [Route("deactiveY")]
        public MasterLanguageDTO deactiveY([FromBody]MasterLanguageDTO data)
        {
            return _objInter.deactiveY(data);
        }
    }
}
