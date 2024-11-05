using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Library;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LibraryServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class Staff_BookTranasctionFacade : Controller
    {

        public Staff_BookTranasctionInterface _objInter;
        public Staff_BookTranasctionFacade(Staff_BookTranasctionInterface data)
        {
            _objInter = data;
        }

        [Route("getdetails")]
        public Staff_BookTranasctionDTO getdetails([FromBody]Staff_BookTranasctionDTO data)
        {
            return _objInter.getdetails(data);
        }

        [Route("get_Staffdetails")]
        public Staff_BookTranasctionDTO get_Staffdetails([FromBody] Staff_BookTranasctionDTO data)
        {
            return _objInter.get_Staffdetails(data);
        }

        [Route("get_bookdetails")]
        public Staff_BookTranasctionDTO get_bookdetails([FromBody] Staff_BookTranasctionDTO data)
        {
            return _objInter.get_bookdetails(data);
        }

        [Route("Savedata")]
        public Staff_BookTranasctionDTO Savedata([FromBody] Staff_BookTranasctionDTO data)
        {
            return _objInter.Savedata(data);
        }
        [Route("renewaldata")]
        public Staff_BookTranasctionDTO renewaldata([FromBody] Staff_BookTranasctionDTO data)
        {
            return _objInter.renewaldata(data);
        }

        [Route("Editdata")]
        public Staff_BookTranasctionDTO Editdata([FromBody] Staff_BookTranasctionDTO data)
        {
            return _objInter.Editdata(data);
        }

        [Route("returndata")]
        public Staff_BookTranasctionDTO returndata([FromBody] Staff_BookTranasctionDTO data)
        {
            return _objInter.returndata(data);
        }

    }
}
