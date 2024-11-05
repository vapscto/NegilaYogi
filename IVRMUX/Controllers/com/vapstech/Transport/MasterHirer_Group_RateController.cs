using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.Transport;
using PreadmissionDTOs.com.vaps.Transport;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Transport
{
    [Route("api/[controller]")]
    public class MasterHirer_Group_RateController : Controller
    {
        MasterHirer_Group_Rate_Delegate _delegate = new MasterHirer_Group_Rate_Delegate();
        //Master Hirer Group.
        [Route("loadData/{id:int}")]
        public MasterHirer_Group_RateDTO loadData(int id)
        {
            MasterHirer_Group_RateDTO obj = new MasterHirer_Group_RateDTO();
            obj.MI_Id= Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.load(obj);
        }
        [Route("save")]
        public MasterHirer_Group_RateDTO save([FromBody]MasterHirer_Group_RateDTO data )
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.save(data);
        }
        [Route("Edit/{id:int}")]
        public MasterHirer_Group_RateDTO Edit(int id)
        {
            return _delegate.edit(id);
        }
        [Route("deactivate")]
        public MasterHirer_Group_RateDTO deactivate([FromBody]MasterHirer_Group_RateDTO data )
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.TRHG_Id = data.TRHG_Id;
            return _delegate.deactivate(data);
        }

        //Master Hirer Rate.

        [Route("loadRateData/{id:int}")]
        public MasterHirer_Group_RateDTO loadRateData(int id)
        {
            MasterHirer_Group_RateDTO obj = new MasterHirer_Group_RateDTO();
            obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.loadRateData(obj);
        }
        [Route("saveRate")]
        public MasterHirer_Group_RateDTO saveRate([FromBody]MasterHirer_Group_RateDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.saveRate(data);
        }
        [Route("EditRate/{id:int}")]
        public MasterHirer_Group_RateDTO EditRate(int id)
        {
            return _delegate.EditRate(id);
        }
        //Master Hirer.
        [Route("loadHirerData/{id:int}")]
        public MasterHirer_Group_RateDTO loadHirerData(int id)
        {
            MasterHirer_Group_RateDTO obj = new MasterHirer_Group_RateDTO();
            obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.loadHirerData(obj);
        }
        [Route("saveHirer")]
        public MasterHirer_Group_RateDTO saveHirer([FromBody]MasterHirer_Group_RateDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.saveHirer(data);
        }
        [Route("EditHirer/{id:int}")]
        public MasterHirer_Group_RateDTO EditHirer(int id)
        {
            return _delegate.EditHirer(id);
        }
        [Route("deactivateHirer")]
        public MasterHirer_Group_RateDTO deactivateHirer([FromBody]MasterHirer_Group_RateDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.TRMH_Id = data.TRMH_Id;
            return _delegate.deactivateHirer(data);
        }
    }
}
