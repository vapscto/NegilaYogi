using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeFeeService.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Fees;


namespace CollegeFeeService.com.vaps.Facade
{
    [Route("api/[controller]")]
    public class PDCReportFacade : Controller
    {
        public PDCReportInterface _feegrouppagee;

        public PDCReportFacade(PDCReportInterface maspag)
        {
            _feegrouppagee = maspag;
        }


        [Route("getdetails")]
        public PDC_EntryFormDTO getorgdet([FromBody]PDC_EntryFormDTO dt)
        {
            return _feegrouppagee.getdetails(dt);
        }


        [HttpPost]
        [Route("get_courses")]
        public PDC_EntryFormDTO get_courses([FromBody] PDC_EntryFormDTO data)
        {
            return _feegrouppagee.get_courses(data);
        }
        [HttpPost]
        [Route("get_branches")]
        public PDC_EntryFormDTO get_branches([FromBody] PDC_EntryFormDTO data)
        {
            return _feegrouppagee.get_branches(data);
        }
        [HttpPost]
        [Route("get_semisters")]
        public PDC_EntryFormDTO get_semisters([FromBody] PDC_EntryFormDTO data)
        {
            return _feegrouppagee.get_semisters(data);
        }
        [HttpPost]
        [Route("get_semisters_new")]
        public PDC_EntryFormDTO get_semisters_new([FromBody] PDC_EntryFormDTO data)
        {
            return _feegrouppagee.get_semisters_new(data);
        }

        [HttpPost]

        [Route("getgroupmappedheads")]
        public PDC_EntryFormDTO getgroupmappedheads([FromBody]PDC_EntryFormDTO dto)
        {
            return _feegrouppagee.getgroupmappedheads(dto);
        }

        [Route("getgroupheadsid")]
        public PDC_EntryFormDTO getgroupheadsid([FromBody]PDC_EntryFormDTO dto)
        {
            return _feegrouppagee.getgroupheadsid(dto);
        }


        [Route("Getreportdetails")]
        public Task<PDC_EntryFormDTO> Getreportdetails([FromBody]PDC_EntryFormDTO dto)
        {
            return _feegrouppagee.Getreportdetails(dto);
        }

        [Route("getdata")]
        public PDC_EntryFormDTO getdata([FromBody]PDC_EntryFormDTO dto)
        {
            return _feegrouppagee.getdata(dto);
        }
    }
}
