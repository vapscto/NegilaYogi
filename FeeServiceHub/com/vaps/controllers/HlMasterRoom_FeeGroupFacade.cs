using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeeServiceHub.com.vaps.interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class HlMasterRoom_FeeGroupFacade : Controller
    {
        public HlMasterRoom_FeeGroupInterface _org;

        public HlMasterRoom_FeeGroupFacade(HlMasterRoom_FeeGroupInterface orga)
        {
            _org = orga;
        }


        [Route("loaddata")]
        public HlMasterRoom_FeeGroupDTO loaddata([FromBody] HlMasterRoom_FeeGroupDTO data)
        {
            return _org.loaddata(data);
        }
        [Route("save")]
        public HlMasterRoom_FeeGroupDTO save([FromBody] HlMasterRoom_FeeGroupDTO data)
        {
            return _org.save(data);
        }
        [Route("edittab1")]
        public HlMasterRoom_FeeGroupDTO edittab1([FromBody]  HlMasterRoom_FeeGroupDTO data)
        {
            return _org.edittab1(data);
        }
        [Route("deactive")]
        public HlMasterRoom_FeeGroupDTO deactive([FromBody] HlMasterRoom_FeeGroupDTO data)
        {
            return _org.deactive(data);
        }

    }
}
