using HostelServiceHub.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Hostel;


namespace HostelServiceHub.Facade
{
    [Route("api/[controller]")]
    public class Hostel_Student_GatePassFacadeController : Controller
    {
            public Hostel_Student_GatePassInterface _ads;

            public Hostel_Student_GatePassFacadeController(Hostel_Student_GatePassInterface adstu)
            {
                _ads = adstu;
            }

            // GET: api/values
            [Route("onloadgetdetails")]
            public Hostel_Student_GatePassDTO getinitialdata([FromBody]Hostel_Student_GatePassDTO dto)
            {
                return _ads.getBasicData(dto);
            }

       
       [HttpPost]
        [Route("savedetails")]
        public Hostel_Student_GatePassDTO savedetails([FromBody]Hostel_Student_GatePassDTO dto)
        {
            return _ads.SaveUpdate(dto);
        }

        [Route("Edit")]
        public Hostel_Student_GatePassDTO Edit([FromBody] Hostel_Student_GatePassDTO dto)
        {
            return _ads.Edit(dto);
        }

        [Route("deactivateRecordById")]
        public Hostel_Student_GatePassDTO deactivateRecordById([FromBody]Hostel_Student_GatePassDTO dto)
        {
            return _ads.deactivate(dto);
        }
    }
}
