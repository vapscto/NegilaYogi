using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.Medical;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.Medical;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.Medical
{
    [Route("api/[controller]")]
    public class Naac_MC_CR6Controller : Controller
    {
        Naac_MC_CR6_Delegate del = new Naac_MC_CR6_Delegate();

        [Route("loaddata/{id:int}")]
        public Naac_MC_CR6_DTO loaddata(int id)
        {
            Naac_MC_CR6_DTO data = new Naac_MC_CR6_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddata(data);
        }
        [Route("MedFinancialSupport632Report")]
        public Naac_MC_CR6_DTO MedFinancialSupport632Report([FromBody]Naac_MC_CR6_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.MedFinancialSupport632Report(data);
        }
        [Route("MedDevPrograms634634Report")]
        public Naac_MC_CR6_DTO MedDevPrograms634634Report([FromBody]Naac_MC_CR6_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.MedDevPrograms634634Report(data);
        }
        [Route("MedFunds643Report")]
        public Naac_MC_CR6_DTO MedFunds643Report([FromBody]Naac_MC_CR6_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.MedFunds643Report(data);
        }
        [Route("MedIQAC652Report")]
        public Naac_MC_CR6_DTO MedIQAC652Report([FromBody]Naac_MC_CR6_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.MedIQAC652Report(data);
        }
        [Route("MEDInternalQuality653")]
        public Naac_MC_CR6_DTO MEDInternalQuality653([FromBody]Naac_MC_CR6_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.MEDInternalQuality653(data);
        }
        [Route("MedEGovernance622Report")]
        public Naac_MC_CR6_DTO MedEGovernance622Report([FromBody]Naac_MC_CR6_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.MedEGovernance622Report(data);
        }
        [Route("MedDevPrograms633Report")]
        public Naac_MC_CR6_DTO MedDevPrograms633Report([FromBody]Naac_MC_CR6_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.MedDevPrograms633Report(data);
        }
    }
}
