using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeFeeService.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Fees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeFeeService.com.vaps.Facade
{
    [Route("api/[controller]")]
    public class CollegeYearlyCollectionReportFacade : Controller
    {
        public CollegeYearlyCollectionReportInterface _feegrouppagee;

        public CollegeYearlyCollectionReportFacade(CollegeYearlyCollectionReportInterface maspag)
        {
            _feegrouppagee = maspag;
        }


        [Route("getdetails")]
        public CollegeConcessionDTO getorgdet([FromBody]CollegeConcessionDTO dt)
        {
            return _feegrouppagee.getdetails(dt);
        }


        [HttpPost]
        [Route("get_courses")]
        public CollegeConcessionDTO get_courses([FromBody] CollegeConcessionDTO data)
        {
            return _feegrouppagee.get_courses(data);
        }
        [HttpPost]
        [Route("get_branches")]
        public CollegeConcessionDTO get_branches([FromBody] CollegeConcessionDTO data)
        {
            return _feegrouppagee.get_branches(data);
        }
        [HttpPost]
        [Route("get_semisters")]
        public CollegeConcessionDTO get_semisters([FromBody] CollegeConcessionDTO data)
        {
            return _feegrouppagee.get_semisters(data);
        }
        [HttpPost]
        [Route("get_semisters_new")]
        public CollegeConcessionDTO get_semisters_new([FromBody] CollegeConcessionDTO data)
        {
            return _feegrouppagee.get_semisters_new(data);
        }

        [HttpPost]

        [Route("getgroupmappedheads")]
        public CollegeConcessionDTO getgroupmappedheads([FromBody]CollegeConcessionDTO dto)
        {
            return _feegrouppagee.getgroupmappedheads(dto);
        }

        [Route("getgroupheadsid")]
        public CollegeConcessionDTO getgroupheadsid([FromBody]CollegeConcessionDTO dto)
        {
            return _feegrouppagee.getgroupheadsid(dto);
        }


        [Route("Getreportdetails")]
        public Task<CollegeConcessionDTO> Getreportdetails([FromBody]CollegeConcessionDTO dto)
        {
            return _feegrouppagee.Getreportdetails(dto);
        }

        [Route("getdata")]
        public CollegeConcessionDTO getdata([FromBody]CollegeConcessionDTO dto)
        {
            return _feegrouppagee.getdata(dto);
        }

        //headwisedetails
        [Route("headwisedetails")]
        public Task<CollegeConcessionDTO> headwisedetails([FromBody]CollegeConcessionDTO dto)
        {
            return _feegrouppagee.headwisedetails(dto);
        }
    }
}
