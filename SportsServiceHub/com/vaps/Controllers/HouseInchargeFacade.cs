using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Sports;
using SportsServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SportsServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class HouseInchargeFacade : Controller
    {
        // GET: api/<controller>


        HouseInchargeInterface _objinter;
        public HouseInchargeFacade(HouseInchargeInterface obj)
        {
            _objinter = obj;
        }

        // POST api/<controller>
        [HttpPost]
        [Route("Getdetails")]
        public SPCC_Master_House_Staff_DTO Getdetails([FromBody]SPCC_Master_House_Staff_DTO data)
        {
            return _objinter.Getdetails(data);
        }
        [Route("editrecord")]
        public SPCC_Master_House_Staff_DTO editrecord([FromBody] SPCC_Master_House_Staff_DTO id)
        {
            return _objinter.editrecord(id);
        }
        [Route("saverecord")]
        public SPCC_Master_House_Staff_DTO saverecord([FromBody]SPCC_Master_House_Staff_DTO data)
        {
            return _objinter.saverecord(data);
        }
        [Route("deactive")]
        public SPCC_Master_House_Staff_DTO deactive([FromBody]SPCC_Master_House_Staff_DTO data)
        {
            return _objinter.deactive(data);
        }
        [Route("get_House")]
        public SPCC_Master_House_Staff_DTO get_House([FromBody]SPCC_Master_House_Staff_DTO value)
        {
            return _objinter.get_House(value);
        }

        [Route("getdepchange")]
        public SPCC_Master_House_Staff_DTO getdepchange([FromBody]SPCC_Master_House_Staff_DTO data)
        {
            return _objinter.getdepchange(data);
        }
        [Route("get_staff1")]
        public SPCC_Master_House_Staff_DTO get_staff1([FromBody]SPCC_Master_House_Staff_DTO value)
        {
            return _objinter.get_staff1(value);
        }

        
    }
}
