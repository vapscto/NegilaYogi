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
    public class LostBookReportFacade : Controller
    {
        public LostBookReportInterface _objInter;
        public LostBookReportFacade(LostBookReportInterface para)
        {
            _objInter = para;
        }

        [Route("getdetails")]
        public LostBookReport_DTO getdetails([FromBody] LostBookReport_DTO id)
        {
            return _objInter.getdetails(id);
        }

        [Route("get_report")]
        public Task<LostBookReport_DTO> get_report([FromBody] LostBookReport_DTO data)
        {
            return _objInter.get_report(data);
        }


    }
}
