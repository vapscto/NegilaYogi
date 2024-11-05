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
    public class ExcelImportNotDoneReportFacade : Controller
    {
        public ExcelImportNotDoneReportInterface _feetar;

        public ExcelImportNotDoneReportFacade(ExcelImportNotDoneReportInterface maspag)
        {
            _feetar = maspag;
        }

        // GET: api/values

        [HttpPost]
        [Route("getalldetails123")]
        public ExcelImportNotDoneReportDTO Getdet([FromBody] ExcelImportNotDoneReportDTO data)
        {
            return _feetar.getdata123(data);
        }


        [Route("getsection")]
        public ExcelImportNotDoneReportDTO getsection([FromBody]ExcelImportNotDoneReportDTO data)
        {
            return _feetar.getsection(data);
        }
        [Route("getstudent")]
        public ExcelImportNotDoneReportDTO getstudent([FromBody]ExcelImportNotDoneReportDTO data)
        {
            return _feetar.getstudent(data);
        }

        [Route("getgroupmappedheads")]
        public ExcelImportNotDoneReportDTO getstuddetails([FromBody]ExcelImportNotDoneReportDTO value)
        {
            return _feetar.getstuddet(value);
        }
        [Route("getreport")]
        public Task<ExcelImportNotDoneReportDTO> getreport([FromBody] ExcelImportNotDoneReportDTO data)
        {
            return _feetar.getreport(data);
        }
        [Route("get_groups")]
        public ExcelImportNotDoneReportDTO get_groups([FromBody]ExcelImportNotDoneReportDTO data)
        {
            return _feetar.get_groups(data);
        }
        [HttpDelete]
        [Route("deletemodpages/{id:int}")]
        public ExcelImportNotDoneReportDTO Delete(int id)
        {
            return _feetar.deleterec(id);
        }
    }
}
