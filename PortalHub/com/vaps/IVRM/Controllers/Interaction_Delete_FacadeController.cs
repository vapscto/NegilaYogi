using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.IVRS.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.IVRM;
using PreadmissionDTOs.com.vaps.Portals.IVRS;

namespace PortalHub.com.vaps.IVRS.Controllers
{
    [Route("api/[controller]")]
    public class Interaction_Delete_FacadeController : Controller
    {
        Interaction_Delete_ReportInterface intr;
        public Interaction_Delete_FacadeController(Interaction_Delete_ReportInterface intr1)
        {
            intr = intr1;
        }
        [Route("getreport")]
        public Interaction_Delete_Report_DTO getreport([FromBody]Interaction_Delete_Report_DTO dto)
        {
            return intr.getreport(dto);
        }
        [Route("loadreportdata")]
        public Interaction_Delete_Report_DTO loadreportdata([FromBody]Interaction_Delete_Report_DTO dto)
        {
            return intr.loadreportdata(dto);
        }
        [Route("getintreport")]
        public Interaction_Delete_Report_DTO getintreport([FromBody]Interaction_Delete_Report_DTO dto)
        {
            return intr.getintreport(dto);
        }
        [Route("mobload")]
        public Interaction_Delete_Report_DTO mobload([FromBody]Interaction_Delete_Report_DTO dto)
        {
            return intr.mobload(dto);
        }
        [Route("mobreport")]
        public Interaction_Delete_Report_DTO mobreport([FromBody]Interaction_Delete_Report_DTO dto)
        {
            return intr.mobreport(dto);
        }

    }
}