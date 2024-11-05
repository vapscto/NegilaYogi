using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;
using AdmissionServiceHub.com.vaps.Interfaces;
using DataAccessMsSqlServerProvider;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class HHSStudyCertificateFacadeController : Controller
    {
        public HHSStudyCertificateInterface _report;
        private readonly DomainModelMsSqlServerContext _db;
        public HHSStudyCertificateFacadeController(HHSStudyCertificateInterface IAtt, DomainModelMsSqlServerContext db)
        {
            _report = IAtt;
            _db = db;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [Route("getdetails")]
        public HHSStudyCertificateDTO getdetails([FromBody] HHSStudyCertificateDTO data)
        {
            return _report.getdetails(data);
        }

        [Route("getS")]
        public HHSStudyCertificateDTO getstudentlist([FromBody]HHSStudyCertificateDTO student)
        {
            return _report.getstudlist(student);
        }

        [HttpPost]

        [Route("getStudData")]
        public Task<HHSStudyCertificateDTO> Post([FromBody] HHSStudyCertificateDTO studData)
        {
            return _report.getStudDetails(studData);
        }

        [Route("MigrationCertificateStuddetails")]
        public Task<HHSStudyCertificateDTO> MigrationCertificateStuddetails([FromBody] HHSStudyCertificateDTO studData)
        {
            return _report.MigrationCertificateStuddetails(studData);
        }

        [Route("onacademicyearchange")]
        public HHSStudyCertificateDTO onacademicyearchange([FromBody] HHSStudyCertificateDTO data)
        {
            return _report.onacademicyearchange(data);
        }

        [Route("searchfilter")]
        public HHSStudyCertificateDTO searchfilter([FromBody] HHSStudyCertificateDTO data)
        {
            return _report.searchfilter(data);
        }

        [Route("getstudentname")]
        public HHSStudyCertificateDTO getstudentname([FromBody] HHSStudyCertificateDTO data)
        {
            return _report.getstudentname(data);
        }

        //Certificate Generated Report
        [Route("CertificateGeneratedReportLoad")]
        public HHSStudyCertificateDTO CertificateGeneratedReportLoad([FromBody]HHSStudyCertificateDTO data)
        {
            return _report.CertificateGeneratedReportLoad(data);
        }
        [Route("GetCertificateGeneratedReport")]
        public HHSStudyCertificateDTO GetCertificateGeneratedReport([FromBody]HHSStudyCertificateDTO data)
        {
            return _report.GetCertificateGeneratedReport(data);
        }

    }
}
