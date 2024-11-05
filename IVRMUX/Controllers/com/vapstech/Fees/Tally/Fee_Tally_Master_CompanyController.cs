using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Fees.Tally;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees.Tally;

namespace IVRMUX.Controllers.com.vapstech.Fees.Tally
{
    [Produces("application/json")]
    [Route("api/Fee_Tally_Master_Company")]
    public class Fee_Tally_Master_CompanyController : Controller
    {
        Fee_Tally_Master_CompanyDelegate obj = new Fee_Tally_Master_CompanyDelegate();

        [Route("getalldetails")]
        public Fee_Tally_Master_CompanyDTO load(Fee_Tally_Master_CompanyDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
           
            return obj.getdata(data);
        }
        [HttpPost]
        [Route("savedata")]
        public Fee_Tally_Master_CompanyDTO savedata([FromBody] Fee_Tally_Master_CompanyDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));                       
            return obj.savedata(data);
        }
        [Route("deletedata")]
        public Fee_Tally_Master_CompanyDTO deletedata([FromBody] Fee_Tally_Master_CompanyDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           
            return obj.deletedata(data);
        }
    }
}