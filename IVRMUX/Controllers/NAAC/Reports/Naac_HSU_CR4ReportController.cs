using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.Reports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.University;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.Reports.Medical
{
    [Route("api/[controller]")]
    public class Naac_HSU_CR4ReportController : Controller
    {

        Naac_HSU_CR4ReportDelegate del = new Naac_HSU_CR4ReportDelegate();

        [Route("loaddata/{id:int}")]
        public Naac_HSU_CR4Report_DTO loaddata(int id)
        {
            Naac_HSU_CR4Report_DTO data = new Naac_HSU_CR4Report_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddata(data);
        }
        [Route("HSUclinicalinfra423Report")]
        public Naac_HSU_CR4Report_DTO HSUclinicalinfra423Report([FromBody]Naac_HSU_CR4Report_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.HSUclinicalinfra423Report(data);
        }
        [Route("ClinicalLabReport")]
        public Naac_HSU_CR4Report_DTO ClinicalLabReport([FromBody]Naac_HSU_CR4Report_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.ClinicalLabReport(data);
        }
        [Route("HSUMembership433Report")]
        public Naac_HSU_CR4Report_DTO HSUMembership433Report([FromBody]Naac_HSU_CR4Report_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.HSUMembership433Report(data);
        }
        [Route("HSU_ExpenditureBook434Report")]
        public Naac_HSU_CR4Report_DTO HSU_ExpenditureBook434Report([FromBody]Naac_HSU_CR4Report_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.HSU_ExpenditureBook434Report(data);
        }
        [Route("HSUEcontent435Report")]
        public Naac_HSU_CR4Report_DTO HSUEcontent435Report([FromBody]Naac_HSU_CR4Report_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.HSUEcontent435Report(data);
        }
        [Route("HSUClassSeminarhall441Report")]
        public Naac_HSU_CR4Report_DTO HSUClassSeminarhall441Report([FromBody]Naac_HSU_CR4Report_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.HSUClassSeminarhall441Report(data);
        }
        [Route("HSUBandwidthRangeReport")]
        public Naac_HSU_CR4Report_DTO HSUBandwidthRangeReport([FromBody]Naac_HSU_CR4Report_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.HSUBandwidthRangeReport(data);
        }
        [Route("HSU_PhyAcaFacility451Report")]
        public Naac_HSU_CR4Report_DTO HSU_PhyAcaFacility451Report([FromBody]Naac_HSU_CR4Report_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.HSU_PhyAcaFacility451Report(data);
        }
        [Route("HSU_Infrastructureexpenditure414Report")]
        public Naac_HSU_CR4Report_DTO HSU_Infrastructureexpenditure414Report([FromBody]Naac_HSU_CR4Report_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.HSU_Infrastructureexpenditure414Report(data);
        }
    }
}
