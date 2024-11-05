using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Sports;
using SportsServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SportsServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class SportsReportTeamPageFacade : Controller
    {
        public SportsReportTeamPageInterface _ReportContext;

        public SportsReportTeamPageFacade(SportsReportTeamPageInterface dt)
        {
            _ReportContext = dt;
        }


        [Route("saveRecord")]
        public SportsReportTeamPageDto saveRecord([FromBody]SportsReportTeamPageDto data)
        {
            return _ReportContext.saveRecord(data);
        }

        [Route("Getdetails")]
        public SportsReportTeamPageDto Getdetails([FromBody]SportsReportTeamPageDto data)//int IVRMM_Id
        {

            return _ReportContext.Getdetails(data);
        }

        [Route("get_modeldata")]
        public SportsReportTeamPageDto get_modeldata([FromBody]SportsReportTeamPageDto data)
        {
            return _ReportContext.get_modeldata(data);
        }

        [Route("showdetails")]
        public SportsReportTeamPageDto showdetails([FromBody] SportsReportTeamPageDto data)
        {
            return _ReportContext.showdetails(data);
        }



        [Route("get_student")]
        public SportsReportTeamPageDto get_student([FromBody]SportsReportTeamPageDto data)
        {
            return _ReportContext.get_student(data);
        }

        [Route("EditRecord")]
        public SportsReportTeamPageDto EditRecord([FromBody]SportsReportTeamPageDto dTO)
        {
            return _ReportContext.EditRecord(dTO);
        }

        [Route("deactivate")]
        public SportsReportTeamPageDto deactivate([FromBody]SportsReportTeamPageDto data)
        {
            return _ReportContext.deactivate(data);
        }

        [Route("SaveRecords")]
        public SportsReportTeamPageDto SaveRecords([FromBody]SportsReportTeamPageDto data)
        {
            return _ReportContext.SaveRecords(data);
        }

        [Route("GetEditData")]
        public SportsReportTeamPageDto GetEditData([FromBody]SportsReportTeamPageDto dTO)
        {
            return _ReportContext.GetEditData(dTO);
        }

        [Route("deactivated")]
        public SportsReportTeamPageDto deactivated([FromBody]SportsReportTeamPageDto data)
        {
            return _ReportContext.deactivated(data);
        }
    }
}
