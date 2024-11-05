using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Scholorship;
using WebApplication1.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class IVRM_Master_ViddyBharthi : Controller
    {
        public IVRM_Master_ViddyBharthiInterface _acd;
        public IVRM_Master_ViddyBharthi(IVRM_Master_ViddyBharthiInterface acdm)
        {
            _acd = acdm;
        }
        [Route("getall")]
        public ScholorshipMasterDTO Get([FromBody]ScholorshipMasterDTO dto)
        {
            return _acd.getallDetails(dto);
        }
        //savecountry
        [Route("savecountry")]
        public ScholorshipMasterDTO savecountry([FromBody] ScholorshipMasterDTO id)
        {

            return _acd.savecountry(id);
        }
        //savestate
        [Route("savestate")]
        public ScholorshipStateDTO savestate([FromBody] ScholorshipStateDTO id)
        {

            return _acd.savestate(id);
        }
        [Route("onchnagestate")]
        public ScholorshipDitictDTO onchnagestate([FromBody] ScholorshipDitictDTO id)
        {

            return _acd.onchnagestate(id);
        }
        [Route("saveDistrict")]
        public ScholorshipDitictDTO saveDistrict([FromBody] ScholorshipDitictDTO id)
        {

            return _acd.saveDistrict(id);
        }
        [Route("savetaluka")]
        public ScholorshipTalukaDTO savetaluka([FromBody] ScholorshipTalukaDTO id)
        {

            return _acd.savetaluka(id);
        }
        [Route("deactivateCountry")]
        public ScholorshipMasterDTO deactivateCountry([FromBody] ScholorshipMasterDTO id)
        {

            return _acd.deactivateCountry(id);
        }
        [Route("deactivestate")]
        public ScholorshipStateDTO deactivestate([FromBody] ScholorshipStateDTO id)
        {

            return _acd.deactivestate(id);
        }
        [Route("deactivedistict")]
        public ScholorshipDitictDTO deactivedistict([FromBody] ScholorshipDitictDTO id)
        {

            return _acd.deactivedistict(id);
        }
        //deactivetaluka
        [Route("deactivetaluka")]
        public ScholorshipTalukaDTO deactivetaluka([FromBody] ScholorshipTalukaDTO id)
        {

            return _acd.deactivetaluka(id);
        }
    }
}
