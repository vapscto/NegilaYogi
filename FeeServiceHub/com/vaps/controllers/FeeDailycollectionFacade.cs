using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeeServiceHub.com.vaps.interfaces;
using PreadmissionDTOs.com.vaps.Fees;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class FeeDailycollectionFacade : Controller
    {

        public FeeDailyCollectionInterface _feegrouppagee;

        public FeeDailycollectionFacade(FeeDailyCollectionInterface maspag)
        {
            _feegrouppagee = maspag;
        }



        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5

        [Route("getdetails")]
        public DailyCollectionReportDTO getorgdet([FromBody]DailyCollectionReportDTO dt)
        {
            return _feegrouppagee.getdetails(dt);
        }

        [Route("getgroupmappedheads")]
        public DailyCollectionReportDTO getgroupmappedheads([FromBody]DailyCollectionReportDTO dto)
        {
            return _feegrouppagee.getgroupmappedheads(dto);
        }

        [Route("getgroupheadsid")]
        public DailyCollectionReportDTO getgroupheadsid([FromBody]DailyCollectionReportDTO dto)
        {
            return _feegrouppagee.getgroupheadsid(dto);
        }


        [Route("Getreportdetails")]
        public Task<DailyCollectionReportDTO> Getreportdetails([FromBody]DailyCollectionReportDTO dto)
        {
            return _feegrouppagee.Getreportdetails(dto);
        }

        [Route("getdata")]
        public DailyCollectionReportDTO getdata([FromBody]DailyCollectionReportDTO dto)
        {
            return _feegrouppagee.getdata(dto);
        }

        [Route("FeeAccountDetailsReport")]
        public Task<DailyCollectionReportDTO> FeeAccountDetailsReport([FromBody]DailyCollectionReportDTO dto)
        {
            return _feegrouppagee.FeeAccountDetailsReport(dto);
        }
        [Route("ChairmanSMS")]
        public void ChairmanSMS([FromBody]DailyCollectionReportDTO response)
        {
             _feegrouppagee.ChairmanSMS(response);
        }

        //UserWisereportdetails
        [Route("UserWisereportdetails")]
        public Task<DailyCollectionReportDTO> UserWisereportdetails([FromBody]DailyCollectionReportDTO dto)
        {
            return _feegrouppagee.UserWisereportdetails(dto);
        }

        //Report VVVKS
        [Route("getreport")]
        public Task<DailyCollectionReportDTO> getreport([FromBody]DailyCollectionReportDTO dto)
        {
            return _feegrouppagee.getreport(dto);
        }

    }
}
