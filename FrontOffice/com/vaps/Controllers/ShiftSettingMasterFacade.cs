using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FrontOfficeHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.FrontOffice;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FrontOfficeHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ShiftSettingMasterFacade : Controller
    {
        public ShiftSettingMasterInterface _org;
        public ShiftSettingMasterFacade(ShiftSettingMasterInterface maspag)
        {
            _org = maspag;
        }

        [HttpPost]
        [Route("getalldetails")]
        public MasterShiftsTimingsDTO Getdet([FromBody] MasterShiftsTimingsDTO data)
        {
            return _org.getdata(data);
        }
        [Route("savedata")]
        public MasterShiftsTimingsDTO savedataa([FromBody] MasterShiftsTimingsDTO data)
        {

            return _org.savedatadelegate(data);
        }
        [Route("getpagedetails/{id:int}")]
        public MasterShiftsTimingsDTO getpagedetails(int id)
        {
            // id = 12;
            return _org.getpageedit(id);
        }

        [HttpPost]
        [Route("deactivate")]
        public MasterShiftsTimingsDTO Deactivate([FromBody] MasterShiftsTimingsDTO id)
        {
            // id = 12;
            return _org.deactivate(id);
        }
        [Route("getalldetailsviewrecords1/{id:int}")]
        //[Route("getenquirycontroller")]
        public MasterShiftsTimingsDTO getalldetailsviewrecords1(int id)
        {
            // id = 12;
            return _org.getalldetailsviewrecords1(id);
        }
        //[HttpDelete]
        //[Route("deletedetails/{id:int}")]
        //public MasterShiftsTimingsDTO Deleterec(int id)
        //{
        //    return _org.deleterec(id);
        //}
    }
}
