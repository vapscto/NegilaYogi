using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.admission;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.admission
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
   // [EnableCors("AllowSpecificOrigin")]
    public class TotalStrengthController : Controller
    {
       
        TotalStrengthDelegate totStr = new TotalStrengthDelegate();
      
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // get initial dropdown data
        [Route("getdetails")]
        public Adm_M_StudentDTO getInitialData()
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //return sad.getInitailData(mi_id);
            var aa = totStr.GetDataById(mi_id);
            Adm_M_StudentDTO cdto = aa;
            return cdto;
        }

        [HttpPost]
        [Route("Studdetails")]

        public Adm_M_StudentDTO getStudData([FromBody] Adm_M_StudentDTO stuDTO)
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
           //Adm_M_StudentDTO stuDTO = new Adm_M_StudentDTO();
           // stuDTO.ASMAY_Id = id;
            stuDTO.MI_Id = mi_id;

            //return sad.getInitailData(mi_id);
           
            return totStr.GetStudDataById(stuDTO);
        }

        // POST api/values
        [Route("getsection")]
        public Adm_M_StudentDTO getsection([FromBody] Adm_M_StudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return totStr.getsection(data);
        }

        [Route("getclass")]
        public Adm_M_StudentDTO getclass([FromBody] Adm_M_StudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return totStr.getclass(data);
        }

        [Route("getelective")]
        public Adm_M_StudentDTO getelective([FromBody] Adm_M_StudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return totStr.getelective(data);
        }

    }
}
