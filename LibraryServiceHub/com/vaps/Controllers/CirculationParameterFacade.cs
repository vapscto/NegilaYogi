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
    public class CirculationParameterFacade : Controller
    {
        public CirculationParameterInterface _objInter;
        public CirculationParameterFacade(CirculationParameterInterface para)
        {
            _objInter = para;
        }
       [Route("getdetails")]
     
       public Task<CirculationParameterDTO> getdetails([FromBody] CirculationParameterDTO data)
        {
            return _objInter.getdetails(data);
        }
        [Route("Savedata")]
        public Task<CirculationParameterDTO> Savedata([FromBody] CirculationParameterDTO data)
        {
            return _objInter.Savedata(data);
        }
        [Route("getdata")]
        public Task<CirculationParameterDTO> getdata([FromBody] CirculationParameterDTO data)
        {
            return _objInter.getdata(data);
        }
        [Route("gettype")]
        public CirculationParameterDTO gettype([FromBody] CirculationParameterDTO data)
        {
            return _objInter.gettype(data);
        }
        [Route("deactiveY")]
        public CirculationParameterDTO deactiveY([FromBody] CirculationParameterDTO data)
        {
            return _objInter.deactiveY(data);
        }
        [Route("editdata")]
        public CirculationParameterDTO editdata([FromBody] CirculationParameterDTO data)
        {
            return _objInter.editdata(data);
        }
    }
}
