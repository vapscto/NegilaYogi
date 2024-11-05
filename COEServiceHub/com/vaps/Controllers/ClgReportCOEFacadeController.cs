using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.COE;
using COEServiceHub.com.vaps.Interfaces;

namespace COEServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ClgReportCOEFacadeController : Controller
    {
        public ClgReportCOEInterface _ttcategory;

        public ClgReportCOEFacadeController(ClgReportCOEInterface maspag)
        {
            _ttcategory = maspag;
        }


        //[Route("getdetails")]
        //public ClgMasterCOEDTO getdetails([FromBody] ClgMasterCOEDTO org)
        //{
        //    return _ttcategory.getdetails(org);
        //}
        [Route("getdata/{id:int}")]
        public ClgMasterCOEDTO getdata(int id)
        {
            return _ttcategory.getinitialData(id);
        }

        // POST api/values
        [HttpPost]
        [Route("getReport")]
        public Task<ClgMasterCOEDTO> getReport([FromBody]ClgMasterCOEDTO data)
        {
            return _ttcategory.getReport(data);
        }

        // PUT api/values/5
        [Route("mothreport")]
        public ClgMasterCOEDTO mothreport([FromBody]ClgMasterCOEDTO data)
        {
            return _ttcategory.mothreport(data);
        }
    }

}