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
    public class GenderWisePaidDetailsFacade : Controller
    {
        public GenderWisePaidDetailsInterface _feetar;

        public GenderWisePaidDetailsFacade(GenderWisePaidDetailsInterface maspag)
        {
            _feetar = maspag;
        }

        // GET: api/values

        [HttpPost]
        [Route("getalldetails123")]
        public GenderWisePaidDetailsDTO Getdet([FromBody] GenderWisePaidDetailsDTO data)
        {
            return _feetar.getdata123(data);
        }


        [Route("getsection")]
        public GenderWisePaidDetailsDTO getsection([FromBody]GenderWisePaidDetailsDTO data)
        {
            return _feetar.getsection(data);
        }
        [Route("getstudent")]
        public GenderWisePaidDetailsDTO getstudent([FromBody]GenderWisePaidDetailsDTO data)
        {
            return _feetar.getstudent(data);
        }

        [Route("getgroupmappedheads")]
        public GenderWisePaidDetailsDTO getstuddetails([FromBody]GenderWisePaidDetailsDTO value)
        {
            return _feetar.getstuddet(value);
        }
        [Route("getreport")]
        public Task<GenderWisePaidDetailsDTO> getreport([FromBody] GenderWisePaidDetailsDTO data)
        {
            return _feetar.getreport(data);
        }
        [Route("get_groups")]
        public GenderWisePaidDetailsDTO get_groups([FromBody]GenderWisePaidDetailsDTO data)
        {
            return _feetar.get_groups(data);
        }
    }
}
