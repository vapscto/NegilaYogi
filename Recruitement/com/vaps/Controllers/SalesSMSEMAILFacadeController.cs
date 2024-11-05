using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using Recruitment.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Recruitment.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class SalesSMSEMAILFacadeController : Controller
    {
        public SalesSMSEMAILInterface _ads;

        public SalesSMSEMAILFacadeController(SalesSMSEMAILInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public SalesSMSEMAILDTO getinitialdata([FromBody]SalesSMSEMAILDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        // POST api/values
        [HttpPost]
        [Route("sendsmsemail")]
        public SalesSMSEMAILDTO sendsmsemail([FromBody]SalesSMSEMAILDTO dto)
        {
            return _ads.sendsmsemail(dto);
        }

        [Route("getRecordById/{id:int}")]

        public SalesSMSEMAILDTO getcatgrydet(int id)
        {
            // id = 12;
            return _ads.editData(id);
        }
        [Route("get_state")]
        public SalesSMSEMAILDTO get_state([FromBody]SalesSMSEMAILDTO dto)
        {
            return _ads.get_state(dto);
        }
        [Route("getrpt")]
        public SalesSMSEMAILDTO getrpt([FromBody]SalesSMSEMAILDTO dto)
        {
            return _ads.getrpt(dto);
        }
        [Route("getrpt_lead")]
        public SalesSMSEMAILDTO getrpt_lead([FromBody]SalesSMSEMAILDTO dto)
        {
            return _ads.getrpt_lead(dto);
        }

        [Route("loadtemplate")]
        public SalesSMSEMAILDTO loadtemplate([FromBody]SalesSMSEMAILDTO dto)
        {
            return _ads.loadtemplate(dto);
        }
        [Route("viewtemplatedetails")]
        public SalesSMSEMAILDTO viewtemplatedetails([FromBody]SalesSMSEMAILDTO dto)
        {
            return _ads.viewtemplatedetails(dto);
        }
    }
}
