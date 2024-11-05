using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.FrontOffice;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FrontOffice;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.FrontOffice
{
    [Route("api/[controller]")]
    public class EmployeeLogImportController : Controller
    {
        EmployeeLogImportDelegatecs _delobj = new EmployeeLogImportDelegatecs();
            [Route("Savedata")]
            public EmployeeLogImportDTO Savedata([FromBody] EmployeeLogImportDTO data)
            {
                data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
                data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
                return _delobj.Savedata(data);
            }
    }
}
