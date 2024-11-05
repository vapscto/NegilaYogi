using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees;
using CollegeFeeService.com.vaps.Interfaces;

namespace CollegeFeeService.com.vaps.Facade
{
    [Route("api/[controller]")]
    public class CollegeMothEndReportFacedeController : Controller
    {
        public CollegeMothEndReportInterface _feetar;

        public CollegeMothEndReportFacedeController(CollegeMothEndReportInterface maspag)
        {
            _feetar = maspag;
        }



        [HttpPost]
        [Route("getalldetails123")]
        public FeeMonthEndReportDTO Getdet([FromBody] FeeMonthEndReportDTO data)
        {
            return _feetar.getdata123(data);
        }

        [Route("getreport")]
        public Task<FeeMonthEndReportDTO> getreport([FromBody] FeeMonthEndReportDTO data)
        {
            return _feetar.getreport(data);
        }
        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
