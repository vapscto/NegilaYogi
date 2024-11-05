using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Library;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LibraryServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ImportLibrarydataFacade : Controller
    {
        public ImportLibrarydataInterface _objInter;
        public ImportLibrarydataFacade(ImportLibrarydataInterface data)
        {
            _objInter = data;
        }

        [Route("Savedata")]
        public ImportLibrarydataDTO Savedata([FromBody] ImportLibrarydataDTO data)
        {
            return _objInter.Savedata(data);
        }
        [Route("getdetails/{id:int}")]
        public ImportLibrarydataDTO getdetails(int id)
        {
            return _objInter.getdetails(id);
        }

        [Route("deactiveY")]
        public ImportLibrarydataDTO deactiveY([FromBody] ImportLibrarydataDTO data)
        {
            return _objInter.deactiveY(data);
        }
    }
}
