using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRMSServicesHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.HRMS;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMSServicesHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class MasterEarningsDeductionsFacadeController : Controller
    {
        public MasterEarningsDeductionsInterface _ads;

        public MasterEarningsDeductionsFacadeController(MasterEarningsDeductionsInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public HR_Master_EarningsDeductionsDTO getinitialdata([FromBody]HR_Master_EarningsDeductionsDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        // POST api/values
        [HttpPost]
        public HR_Master_EarningsDeductionsDTO Post([FromBody]HR_Master_EarningsDeductionsDTO dto)
        {
            return _ads.SaveUpdate(dto);
        }

        [Route("getRecordById/{id:int}")]

        public HR_Master_EarningsDeductionsDTO getcatgrydet(int id)
        {
            // id = 12;
            return _ads.editData(id);
        }
        [Route("deactivateRecordById")]
        public HR_Master_EarningsDeductionsDTO deactivateRecordById([FromBody]HR_Master_EarningsDeductionsDTO dto)
        {
            return _ads.deactivate(dto);
        }

        [Route("orderchangedata")]
        public HR_Master_EarningsDeductionsDTO orderchangedata([FromBody]HR_Master_EarningsDeductionsDTO dto)
        {
            return _ads.changeorderData(dto);
        }



        //type

        // GET: api/values
        [Route("onloadgetdetailstype")]
        public HR_Master_EarningsDeductions_TypeDTO getinitialdata([FromBody]HR_Master_EarningsDeductions_TypeDTO dto)
        {
            return _ads.getBasicDatatype(dto);
        }

        // POST api/values
        [HttpPost]
        [Route("savedetails")]
        public HR_Master_EarningsDeductions_TypeDTO Post([FromBody]HR_Master_EarningsDeductions_TypeDTO dto)
        {
            return _ads.SaveUpdatetype(dto);
        }

        [Route("getRecordByIdType/{id:int}")]

        public HR_Master_EarningsDeductions_TypeDTO getcatgrydettype(int id)
        {
            // id = 12;
            return _ads.editDatatype(id);
        }
        [Route("deactivateRecordByIdType")]
        public HR_Master_EarningsDeductions_TypeDTO deactivateRecordByIdType([FromBody]HR_Master_EarningsDeductions_TypeDTO dto)
        {
            return _ads.deactivatetype(dto);
        }


    }
}
