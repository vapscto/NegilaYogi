using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdmissionServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class GeneralSiblingEmployeeMappingFacadeController : Controller
    {
        public GeneralSiblingEmployeeMappingInterface _ads;
        public GeneralSiblingEmployeeMappingFacadeController(GeneralSiblingEmployeeMappingInterface adstu)
        {
            _ads = adstu;
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("getalldetails")]
        public GeneralSiblingEmployeeMappingDTO getalldetails([FromBody] GeneralSiblingEmployeeMappingDTO data)
        {
            return _ads.getalldetails(data);
        }

        [Route("selectradio")]
        public GeneralSiblingEmployeeMappingDTO selectradio([FromBody] GeneralSiblingEmployeeMappingDTO data)
        {
            return _ads.selectradio(data);
        }
        [Route("onstudentnamechange")]
        public GeneralSiblingEmployeeMappingDTO onstudentnamechange([FromBody] GeneralSiblingEmployeeMappingDTO data)
        {
            return _ads.onstudentnamechange(data);
        }
        [Route("onselectstaff")]
        public GeneralSiblingEmployeeMappingDTO onselectstaff([FromBody] GeneralSiblingEmployeeMappingDTO data)
        {
            return _ads.onselectstaff(data);
        }
        

        [Route("savedata")]
        public GeneralSiblingEmployeeMappingDTO savedata([FromBody] GeneralSiblingEmployeeMappingDTO data)
        {
            return _ads.savedata(data);
        }

        [Route("Deletedetails")]
        public GeneralSiblingEmployeeMappingDTO Delete([FromBody] GeneralSiblingEmployeeMappingDTO data)
        {
            return _ads.deleterec(data);
        }
        [Route("DeletRecordemployee")]
        public GeneralSiblingEmployeeMappingDTO DeletRecordemployee([FromBody] GeneralSiblingEmployeeMappingDTO data)
        {
            return _ads.DeletRecordemployee(data);
        }

        [Route("viewsiblingdetails")]
        public GeneralSiblingEmployeeMappingDTO viewsiblingdetails([FromBody] GeneralSiblingEmployeeMappingDTO data)
        {
            return _ads.viewsiblingdetails(data);
        }
        [Route("viewsiblingdetailsemployee")]
        public GeneralSiblingEmployeeMappingDTO viewsiblingdetailsemployee([FromBody] GeneralSiblingEmployeeMappingDTO data)
        {
            return _ads.viewsiblingdetailsemployee(data);
        }
    }
}
