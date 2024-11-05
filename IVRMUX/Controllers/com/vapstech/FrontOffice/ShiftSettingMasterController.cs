using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.FrontOffice;
using corewebapi18072016.Delegates.com.vapstech.FrontOffice;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class ShiftSettingMasterController : Controller
    {
        ShiftSettingMasterDelegate od = new ShiftSettingMasterDelegate();
        
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public MasterShiftsTimingsDTO getinitialdropdowns(int id)
        {
            MasterShiftsTimingsDTO data = new MasterShiftsTimingsDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.MI_Id = mid;
            return od.getdata(data);
        }

        [HttpPost]
        [Route("savedata")]
        public MasterShiftsTimingsDTO savedataa([FromBody] MasterShiftsTimingsDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return od.savedatadelegate(data);
        }
        [HttpPost]
        [Route("deactivate")]
        public MasterShiftsTimingsDTO deactvate([FromBody] MasterShiftsTimingsDTO id)
        {
            return od.deactivate(id);
        }
        [Route("Editdetails/{id:int}")]//10
        public MasterShiftsTimingsDTO EditDetails(int id)
        {
            HttpContext.Session.SetString("sectionid", id.ToString());
            return od.EditDetails(id);
        }

        [Route("getalldetailsviewrecords1/{id:int}")]
        public MasterShiftsTimingsDTO getalldetailsviewrecords1(int id)
        {

            return od.getalldetailsviewrecords1(id);
        }
        //for edit
        [Route("getdetails/{id:int}")]//11
        public MasterShiftsTimingsDTO getdetail(int id)
        {
            HttpContext.Session.SetString("pageid", id.ToString()); //Set
                                                                    // id = 12;
            return od.EditDetails(id);
        }
        

    }
}
