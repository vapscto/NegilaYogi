using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Transport;
using TransportServiceHub.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TransportServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class MasterHirer_Group_Rate_Facade : Controller
    {
        public MasterHire_Group_RateInterface _interface;
        public MasterHirer_Group_Rate_Facade(MasterHire_Group_RateInterface inter)
        {
            _interface = inter;
        }
        //Master Hirer Group.
       [Route("getdata")]
       public MasterHirer_Group_RateDTO getdata([FromBody] MasterHirer_Group_RateDTO obj)
        {
            return _interface.getdata(obj);
        }
        [Route("save")]
        public MasterHirer_Group_RateDTO save([FromBody] MasterHirer_Group_RateDTO dt)
        {
            return _interface.save(dt);
        }
        [Route("edit/{id:int}")]
        public MasterHirer_Group_RateDTO edit(int id)
        {
            return _interface.edit(id);
        }
        [Route("deactivate")]
        public MasterHirer_Group_RateDTO deactivate([FromBody]MasterHirer_Group_RateDTO dto)
        {
            return _interface.deactivate(dto);
        }
        //Master Hirer Rate.
        [Route("loadRateData")]
        public MasterHirer_Group_RateDTO loadRateData([FromBody] MasterHirer_Group_RateDTO obj)
        {
            return _interface.getRatedata(obj);
        }
        [Route("saveRate")]
        public MasterHirer_Group_RateDTO saveRate([FromBody] MasterHirer_Group_RateDTO dt)
        {
            return _interface.saveRate(dt);
        }
        [Route("EditRate/{id:int}")]
        public MasterHirer_Group_RateDTO EditRate(int id)
        {
            return _interface.editRate(id);
        }
        //Master Hirer.
        [Route("loadHirerData")]
        public MasterHirer_Group_RateDTO loadHirerData([FromBody] MasterHirer_Group_RateDTO obj)
        {
            return _interface.loadHirerData(obj);
        }
        [Route("saveHirer")]
        public MasterHirer_Group_RateDTO saveHirer([FromBody] MasterHirer_Group_RateDTO dt)
        {
            return _interface.saveHirer(dt);
        }
        [Route("EditHirer/{id:int}")]
        public MasterHirer_Group_RateDTO EditHirer(int id)
        {
            return _interface.EditHirer(id);
        }
        [Route("deactivateHirer")]
        public MasterHirer_Group_RateDTO deactivateHirer([FromBody]MasterHirer_Group_RateDTO dto)
        {
            return _interface.deactivateHirer(dto);
        }
    }
}
