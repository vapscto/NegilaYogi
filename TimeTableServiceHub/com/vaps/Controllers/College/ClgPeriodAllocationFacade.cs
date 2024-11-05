using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.TT;
using TimeTableServiceHub.Interfaces;
//using TimeTableServiceHub.com.vaps.Interfaces.College;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TimeTableServiceHub.com.vaps.Controllers.College
{
    [Route("api/[controller]")]
    public class ClgPeriodAllocationFacade : Controller
    {


        public ClgPeriodAllocationInterface _ttperiod;

        public ClgPeriodAllocationFacade(ClgPeriodAllocationInterface maspag)
        {
            _ttperiod = maspag;
        }

        [Route("save_period")]
        public ClgPeriodAllocation_DTO save_period([FromBody] ClgPeriodAllocation_DTO org)
        {
            return _ttperiod.save_period(org);
        }
        [Route("getdetails")]
        public ClgPeriodAllocation_DTO getdetails([FromBody] ClgPeriodAllocation_DTO data)
        {
            return _ttperiod.getdetails(data);
        }


        [Route("getcategories")]
        public ClgPeriodAllocation_DTO getcategories([FromBody] ClgPeriodAllocation_DTO data)
        {
            return _ttperiod.getcategories(data);
        }
        [HttpPost]
        [Route("getperiod_class")]
        public ClgPeriodAllocation_DTO getperiod_class([FromBody] ClgPeriodAllocation_DTO data)
        {
            return _ttperiod.getperiod_class(data);
        }


        [Route("savedetail")]
        public ClgPeriodAllocation_DTO savedetail([FromBody] ClgPeriodAllocation_DTO org)
        {
            return _ttperiod.savedetail(org);
        }




        [Route("deactivate")]
        public ClgPeriodAllocation_DTO deactivateAcdmYear([FromBody] ClgPeriodAllocation_DTO id)
        {
            return _ttperiod.deactivate(id);
        }
        [Route("deactivate1")]
        public ClgPeriodAllocation_DTO deactivateAcdmYear1([FromBody] ClgPeriodAllocation_DTO id)
        {
            return _ttperiod.deactivate1(id);
        }






        //[Route("save_period")]
        //public ClgPeriodAllocation_DTO getcategories([FromBody] ClgPeriodAllocation_DTO org)
        //{
        //    return _ttperiod.getcategories(org);
        //}

    }
}
