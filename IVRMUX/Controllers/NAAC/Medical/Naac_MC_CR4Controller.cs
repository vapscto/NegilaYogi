using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.Medical;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.Medical;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.Medical
{
    [Route("api/[controller]")]
    public class Naac_MC_CR4Controller : Controller
    {

        Naac_MC_CR4_Delegate del = new Naac_MC_CR4_Delegate();

        [Route("loaddata/{id:int}")]
        public Naac_MC_CR4_DTO loaddata(int id)
        {
            Naac_MC_CR4_DTO data = new Naac_MC_CR4_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddata(data);
        }
        [Route("Report")]
        public Naac_MC_CR4_DTO Report([FromBody]Naac_MC_CR4_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.Report(data);
        }
        [Route("InOutPatientReport")]
        public Naac_MC_CR4_DTO InOutPatientReport([FromBody]Naac_MC_CR4_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.InOutPatientReport(data);
        }
        [Route("Membership433Report")]
        public Naac_MC_CR4_DTO Membership433Report([FromBody]Naac_MC_CR4_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.Membership433Report(data);
        }
        [Route("MedExpenditure434Report")]
        public Naac_MC_CR4_DTO MedExpenditure434Report([FromBody]Naac_MC_CR4_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.MedExpenditure434Report(data);
        }
        [Route("Econtent436Report")]
        public Naac_MC_CR4_DTO Econtent436Report([FromBody]Naac_MC_CR4_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.Econtent436Report(data);
        }
        [Route("PhyAcaFacility451Report")]
        public Naac_MC_CR4_DTO PhyAcaFacility451Report([FromBody]Naac_MC_CR4_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.PhyAcaFacility451Report(data);
        }
        [Route("ClassSeminarhall441Report")]
        public Naac_MC_CR4_DTO ClassSeminarhall441Report([FromBody]Naac_MC_CR4_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.ClassSeminarhall441Report(data);
        }
        [Route("BandwidthRangeReport")]
        public Naac_MC_CR4_DTO BandwidthRangeReport([FromBody]Naac_MC_CR4_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.BandwidthRangeReport(data);
        }
        [Route("InfrastructureReport")]
        public Naac_MC_CR4_DTO InfrastructureReport([FromBody]Naac_MC_CR4_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.InfrastructureReport(data);
        }
        [Route("MEDStudentExposed423Report")]
        public Naac_MC_CR4_DTO MEDStudentExposed423Report([FromBody]Naac_MC_CR4_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.MEDStudentExposed423Report(data);
        }
    }
}
