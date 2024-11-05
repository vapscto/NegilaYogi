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
    public class MakerAndCheckerReportFacade : Controller
    {

        public MakerAndCheckerReportInterface _feegrouppagee;

        public MakerAndCheckerReportFacade(MakerAndCheckerReportInterface maspag)
        {
            _feegrouppagee = maspag;
        }


        [Route("getdetails")]
        public CollegePaymentApprovalDTO getorgdet([FromBody]CollegePaymentApprovalDTO dt)
        {
            return _feegrouppagee.getdetails(dt);
        }


        [HttpPost]
        [Route("get_courses")]
        public CollegePaymentApprovalDTO get_courses([FromBody] CollegePaymentApprovalDTO data)
        {
            return _feegrouppagee.get_courses(data);
        }
        [HttpPost]
        [Route("get_branches")]
        public CollegePaymentApprovalDTO get_branches([FromBody] CollegePaymentApprovalDTO data)
        {
            return _feegrouppagee.get_branches(data);
        }
        [HttpPost]
        [Route("get_semisters")]
        public CollegePaymentApprovalDTO get_semisters([FromBody] CollegePaymentApprovalDTO data)
        {
            return _feegrouppagee.get_semisters(data);
        }
        [HttpPost]
        [Route("get_semisters_new")]
        public CollegePaymentApprovalDTO get_semisters_new([FromBody] CollegePaymentApprovalDTO data)
        {
            return _feegrouppagee.get_semisters_new(data);
        }

        [HttpPost]

        [Route("getgroupmappedheads")]
        public CollegePaymentApprovalDTO getgroupmappedheads([FromBody]CollegePaymentApprovalDTO dto)
        {
            return _feegrouppagee.getgroupmappedheads(dto);
        }

        [Route("getgroupheadsid")]
        public CollegePaymentApprovalDTO getgroupheadsid([FromBody]CollegePaymentApprovalDTO dto)
        {
            return _feegrouppagee.getgroupheadsid(dto);
        }


        [Route("Getreportdetails")]
        public Task<CollegePaymentApprovalDTO> Getreportdetails([FromBody]CollegePaymentApprovalDTO dto)
        {
            return _feegrouppagee.Getreportdetails(dto);
        }

        [Route("getdata")]
        public CollegePaymentApprovalDTO getdata([FromBody]CollegePaymentApprovalDTO dto)
        {
            return _feegrouppagee.getdata(dto);
        }
    }
}
