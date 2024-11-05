using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeFeeService.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Fees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeFeeService.com.vaps.Facade
{
    [Route("api/[controller]")]
    public class CLGFeeChequeBounceFacadeController : Controller
    {
       public CLGFeeChequeBounceInterface _inter;
        public CLGFeeChequeBounceFacadeController(CLGFeeChequeBounceInterface inf)
        {
            _inter = inf;
        }
        [HttpPost]
        [Route("getalldetails")]
        public CLGFeeChequeBounceDTO getalldetails([FromBody] CLGFeeChequeBounceDTO data)
        {
            return _inter.getalldetails(data);
        }
        [Route("get_students")]
        public CLGFeeChequeBounceDTO get_students([FromBody] CLGFeeChequeBounceDTO data)
        {           
            return _inter.get_students(data);
        }
        [Route("get_receipts")]
        public CLGFeeChequeBounceDTO get_receipts([FromBody] CLGFeeChequeBounceDTO data)
        {
            return _inter.get_receipts(data);
        }
        [Route("savedata")]
        public CLGFeeChequeBounceDTO savedata([FromBody] CLGFeeChequeBounceDTO data)
        {
            return _inter.savedata(data);
        }
        [Route("DeletRecord")]
        public CLGFeeChequeBounceDTO DeletRecord([FromBody] CLGFeeChequeBounceDTO data)
        {
            return _inter.DeletRecord(data);
        }
    }
}
