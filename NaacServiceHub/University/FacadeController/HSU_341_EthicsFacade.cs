using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.University.Interface;
using PreadmissionDTOs.NAAC.University;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.University.FacadeController
{
    [Route("api/[controller]")]
    public class HSU_341_EthicsFacade : Controller
    {
        public HSU_341_EthicsInterface _inter;
        public HSU_341_EthicsFacade(HSU_341_EthicsInterface i)
        {
            _inter = i;
        }
        [HttpPost]
        [Route("getdata")]
        public HSU_341_EthicsDTO getdata([FromBody] HSU_341_EthicsDTO data)
        {
            return _inter.loaddata(data);
        }
        [Route("savedata")]
        public HSU_341_EthicsDTO savedata([FromBody] HSU_341_EthicsDTO data)
        {
            return _inter.savedata(data);
        }
        [Route("deactive")]
        public HSU_341_EthicsDTO deactive([FromBody] HSU_341_EthicsDTO data)
        {
            return _inter.deactive(data);
        }
        [Route("editdata")]
        public HSU_341_EthicsDTO editdata([FromBody] HSU_341_EthicsDTO data)
        {
            return _inter.editdata(data);
        }
    }
}
