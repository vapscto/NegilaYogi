using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRMSServicesHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.HRMS;
namespace HRMSServicesHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class HRMasterChapterVIFacadeController : Controller
    {
        public MasterChapterVIInterface _ads;

        public HRMasterChapterVIFacadeController(MasterChapterVIInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public MasterChapterVIDTO getinitialdata([FromBody]MasterChapterVIDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        // POST api/values
        [HttpPost]
        public MasterChapterVIDTO Post([FromBody]MasterChapterVIDTO dto)
        {
            return _ads.SaveUpdate(dto);
        }

        [Route("getRecordById/{id:int}")]

        public MasterChapterVIDTO getcatgrydet(int id)
        {
            // id = 12;
            return _ads.editData(id);
        }
        [Route("deactivateRecordById")]
        public MasterChapterVIDTO deactivateRecordById([FromBody]MasterChapterVIDTO dto)
        {
            return _ads.deactivate(dto);
        }

        [HttpPost]
        [Route("validateordernumber")]
        public MasterChapterVIDTO validateordernumber([FromBody]MasterChapterVIDTO dto)
        {
            return _ads.validateordernumber(dto);
        }
    }
}
