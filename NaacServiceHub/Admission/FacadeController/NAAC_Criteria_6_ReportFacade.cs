using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.Admission.Interface;
using PreadmissionDTOs.NAAC.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.Admission.FacadeController
{
    [Route("api/[controller]")]
    public class NAAC_Criteria_6_ReportFacade : Controller
    {
        public NAAC_Criteria_6_ReportInterface inter;
        public NAAC_Criteria_6_ReportFacade(NAAC_Criteria_6_ReportInterface q)
        {
            inter = q;
        }

      [Route("loaddata")] 
      public NAAC_Criteria_6_DTO loaddata([FromBody] NAAC_Criteria_6_DTO data)
        {
            return inter.loaddata(data);
        }
        [Route("get_report")]
        public NAAC_Criteria_6_DTO get_report([FromBody] NAAC_Criteria_6_DTO data)
        {
            return inter.get_report(data);
        }
        

        
    }
}
