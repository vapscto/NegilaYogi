using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.HRMS;
using IVRMUX.Delegates.com.vapstech.Library;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.Library;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.HRMS
{
    [Route("api/[controller]")]
    public class EmployeeDataImportController : Controller
    {
        EmployeeDataImportDelegate _delobj = new EmployeeDataImportDelegate();

        //[Route("Savedata")]
        //public ImportLibrarydataDTO Savedata([FromBody] ImportLibrarydataDTO data)
        //{
        //    data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    return _delobj.Savedata(data);
        //}



        public EmployeeDataImportDTO Savedata([FromBody] EmployeeDataImportDTO data)
        {
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.Savedata(data);
        }
        [Route("getdetails/{id:int}")]
        public EmployeeDataImportDTO getdetails(int id)
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            id = mi_id;
            return _delobj.getdetails(id);
        }

        [Route("deactiveY")]
        public EmployeeDataImportDTO deactiveY([FromBody] EmployeeDataImportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.deactiveY(data);
        }
    }
}
