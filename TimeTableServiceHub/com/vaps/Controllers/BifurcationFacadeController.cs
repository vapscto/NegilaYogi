using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using TimeTableServiceHub.Interfaces;
using PreadmissionDTOs.com.vaps.TT;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TimeTableServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class BifurcationFacadeController : Controller
    {
        public BifurcationInterface _acd;
        public BifurcationFacadeController(BifurcationInterface acdm)
        {
            _acd = acdm;
        }
        [HttpPost]
        [Route("getall")]
        public TT_Bifurcation_DTO getall([FromBody]TT_Bifurcation_DTO dto)
        {
            return _acd.getallDetails(dto);
        }

   
        [HttpPost]
        [Route("savedetail/")]
        public TT_Bifurcation_DTO Post([FromBody]TT_Bifurcation_DTO acdm)
        {
            return _acd.saveProsdet(acdm);
         
        }

        [Route("getdetails/")]
        public TT_Bifurcation_DTO getdetails([FromBody]TT_Bifurcation_DTO acdm)
        {
            // id = 12;
            return _acd.getdetails(acdm);
        }

        [Route("getalldetailsviewrecords/")]
        public TT_Bifurcation_DTO getalldetailsviewrecords([FromBody]TT_Bifurcation_DTO acdm)
        {
        
            return _acd.getalldetailsviewrecords(acdm);
        }

        [Route("getClassdetails/")]
        public TT_Bifurcation_DTO getClassdetails([FromBody]TT_Bifurcation_DTO acdm)
        {
          
            return _acd.getClassdetails(acdm);
       
        }



        [Route("deletedetails/")]
        public TT_Bifurcation_DTO deletedetails([FromBody]TT_Bifurcation_DTO dto)
        {
            return _acd.deleterec(dto);
        }
        
    }
}
