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
    public class MasterNonBookFacade : Controller
    {
        // GET: api/<controller>

        public MasterNonBookInterface _objInter;
        public MasterNonBookFacade(MasterNonBookInterface data)
        {
            _objInter = data;
        }

        [Route("getdetails")]
        public Task<MatserNonBook_DTO> getdetails([FromBody]MatserNonBook_DTO id)
        {
            return _objInter.getdetails(id);
        }

        [Route("Savedata")]
        public MatserNonBook_DTO Savedata([FromBody] MatserNonBook_DTO data)
        {
            return _objInter.Savedata(data);
        }

        [Route("deactiveY")]
        public MatserNonBook_DTO deactiveY([FromBody] MatserNonBook_DTO data)
        {
            return _objInter.deactiveY(data);
        }

        [Route("Editdata")]
        public Task<MatserNonBook_DTO> Editdata([FromBody] MatserNonBook_DTO data)
        {
            return _objInter.Editdata(data);
        }

        [Route("searching")]
        public MatserNonBook_DTO searching([FromBody] MatserNonBook_DTO data)
        {
            return _objInter.searching(data);
        }

        [Route("changelibrary")]
        public Task<MatserNonBook_DTO> changelibrary([FromBody] MatserNonBook_DTO data)
        {
            return _objInter.changelibrary(data);
        }
        
    }
}
