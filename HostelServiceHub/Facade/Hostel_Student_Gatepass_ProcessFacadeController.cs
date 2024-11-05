
using HostelServiceHub.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Hostel;

namespace HostelServiceHub.Facade
{
    [Route("api/[controller]")]
    public class Hostel_Student_Gatepass_ProcessFacadeController : Controller
    {
        public Hostel_Student_Gatepass_ProcessInterface _ads;

            public Hostel_Student_Gatepass_ProcessFacadeController(Hostel_Student_Gatepass_ProcessInterface adstu)
            {
                _ads = adstu;
            }

            // GET: api/values
            [Route("onloadgetdetails")]
            public Hostel_Student_GatePassDTO getinitialdata([FromBody]Hostel_Student_GatePassDTO dto)
            {
                return _ads.getBasicData(dto);
            }
        // POST api/values

        [Route("empdetails")]
        public Hostel_Student_GatePassDTO empdetails([FromBody]Hostel_Student_GatePassDTO dto)
        {
            return _ads.empdetails(dto);
        }


        [Route("approvedrecord")]
        public Hostel_Student_GatePassDTO approvedrecord([FromBody] Hostel_Student_GatePassDTO dto)
        {
            return _ads.approvedrecord(dto);
        }


        //------------------   Approval Report------------------------------


        [Route("Onload")]
        public Hostel_Student_GatePassDTO Onload([FromBody]Hostel_Student_GatePassDTO dto)
        {
            return _ads.Onload(dto);
        }
        [Route("getapprovalreport")]
        public Hostel_Student_GatePassDTO getapprovalreport([FromBody]Hostel_Student_GatePassDTO dto)
        {
            return _ads.getapprovalreport(dto);
        }

        //GatePass Admin Apply
        [Route("getGatePassAdminApplyOnload")]
        public Hostel_Student_GatePassDTO getGatePassAdminApplyOnload([FromBody] Hostel_Student_GatePassDTO data)
        {
            return _ads.getGatePassAdminApplyOnload(data);
        }
        [Route("SaveUpdate")]
        public Hostel_Student_GatePassDTO SaveUpdate([FromBody] Hostel_Student_GatePassDTO data)
        {
            return _ads.SaveUpdate(data);
        }
        [Route("UpdateStatus")]
        public Hostel_Student_GatePassDTO UpdateStatus([FromBody] Hostel_Student_GatePassDTO data)
        {
            return _ads.UpdateStatus(data);
        }
        [Route("deactivate")]
        public Hostel_Student_GatePassDTO deactivate([FromBody] Hostel_Student_GatePassDTO data)
        {
            return _ads.deactivate(data);
        }
        [Route("Edit")]
        public Hostel_Student_GatePassDTO Edit([FromBody] Hostel_Student_GatePassDTO data)
        {
            return _ads.Edit(data);
        }



    }
    }
