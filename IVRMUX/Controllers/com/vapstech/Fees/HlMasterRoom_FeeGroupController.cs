using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Fees;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Fees
{
    [Route("api/[controller]")]
    public class HlMasterRoom_FeeGroupController : Controller
    {

        HlMasterRoom_FeeGroupDelegate del = new HlMasterRoom_FeeGroupDelegate();

        [Route("loaddata")]
        public HlMasterRoom_FeeGroupDTO loaddata([FromBody] HlMasterRoom_FeeGroupDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.loaddata(data);

        }
        [Route("save")]
        public HlMasterRoom_FeeGroupDTO save([FromBody]HlMasterRoom_FeeGroupDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.save(data);
        }
        [Route("edittab1")]
        public HlMasterRoom_FeeGroupDTO edittab1([FromBody] HlMasterRoom_FeeGroupDTO data)




        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.edittab1(data);
        }
        [Route("deactive")]

        public HlMasterRoom_FeeGroupDTO deactive([FromBody]HlMasterRoom_FeeGroupDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.deactive(data);
        }
    }
}
