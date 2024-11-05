using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRMSServicesHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.HRMS;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMSServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class PFTransactionFacadeController : Controller
    {
        // GET: api/values
        public PFTransactionInterface _ads;

        public PFTransactionFacadeController(PFTransactionInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public PFReportsDTO getinitialdata([FromBody]PFReportsDTO dto)
        {
            return _ads.getBasicData(dto);
        }



        [Route("SavePFData")]
        public PFReportsDTO SavePFData([FromBody]PFReportsDTO dto)
        {
            return _ads.SavePFData(dto);
        }

        [Route("getReport")]
        public PFReportsDTO getReport([FromBody]PFReportsDTO dto)
        {
            return _ads.getReport(dto);
        }

        [Route("getEmployeedetailsBySelectionStJames")]
        public PFReportsDTO getEmployeedetailsBySelectionStJames([FromBody]PFReportsDTO dto)
        {
            return _ads.getEmployeedetailsBySelectionStJames(dto);
        }


        //delete
        [Route("FilterEmployeeData")]
        public PFReportsDTO DeleteRecord([FromBody]PFReportsDTO dto)
        {
            return _ads.DeleteRecord(dto);
        }
        [Route("editdata")]
        public PFReportsDTO editdata([FromBody]PFReportsDTO dto)
        {
            return _ads.editdata(dto);
        }


        [Route("savedetails")]
        public PFReportsDTO savedetails([FromBody]PFReportsDTO dto)
        {
            return _ads.savedetails(dto);
        }

        [Route("getloaddata")]
        public PFReportsDTO getloaddata([FromBody]PFReportsDTO dto)
        {
            return _ads.getloaddata(dto);
        }
        [Route("deactive")]
        public PFReportsDTO deactive([FromBody] PFReportsDTO data)
        {
            return _ads.deactive(data);
        }

        [Route("PFBlurcalculation")]
        public PFReportsDTO PFBlurcalculation([FromBody] PFReportsDTO data)
        {
            return _ads.PFBlurcalculation(data);
        }
        [Route("EditSave")]
        public PFReportsDTO EditSave([FromBody] PFReportsDTO data)
        {
            return _ads.EditSave(data);
        }
        [Route("finalverify")]
        public PFReportsDTO finalverify([FromBody] PFReportsDTO data)
        {
            return _ads.finalverify(data);
        }
    }
}
