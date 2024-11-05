using IVRMUX.Delegates.com.vapstech.Inventory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Controllers.com.vapstech.Inventory
{
    [Route("api/[controller]")]
    public class INV_R_GRNController : Controller
    {
        INV_R_GRNDelegate _delegate = new INV_R_GRNDelegate();

      
        [Route("getloaddata")]
        public INV_T_GRNDTO getloaddata([FromBody] INV_T_GRNDTO data)
        {          
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }

        [Route("onreport")]
        public INV_T_GRNDTO onreport([FromBody] INV_T_GRNDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.onreport(data);
        }

        [Route("getdata_ob/{id:int}")]
        public INV_OpeningBalanceDTO getdata_ob(int id)
        {
            INV_OpeningBalanceDTO dto = new INV_OpeningBalanceDTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getdata_ob(dto);
        }
         [Route("GetReport_ob")]
        public INV_OpeningBalanceDTO GetReport_ob([FromBody] INV_OpeningBalanceDTO dto)
        {
           
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.GetReport_ob(dto);
        }
        


    }
}
