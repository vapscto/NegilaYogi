using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Library;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Library;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Library
{
    [Route("api/[controller]")]
    public class MasterDonorController : Controller
    {

        MasterDonorDelegate _delobj = new MasterDonorDelegate();


        [Route("Savedata")]
        public MasterDonorDTO Savedata([FromBody]MasterDonorDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return _delobj.Savedata(data);
        }
        [Route("getdetails/{id:int}")]
        public MasterDonorDTO getdetails(int id)
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            id = mi_id;
            return _delobj.getdetails(id);
        }
        [Route("deactiveY")]
        public MasterDonorDTO deactiveY([FromBody]MasterDonorDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return _delobj.deactiveY(data);
        }

    }
}
