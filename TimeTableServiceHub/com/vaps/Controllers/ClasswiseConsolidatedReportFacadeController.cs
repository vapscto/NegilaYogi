using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.TT;
using TimeTableServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TimeTableServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ClasswiseConsolidatedReportFacadeController : Controller
    {
        public ClasswiseConsolidatedReportInterface inter;


        public ClasswiseConsolidatedReportFacadeController(ClasswiseConsolidatedReportInterface t)
        {
            inter = t;
        }

        //  [Route("loaddata")]
        //public TT_ClasswiseConsolidatedReportDTO loaddata([FromBody] TT_ClasswiseConsolidatedReportDTO data)
        // {
        //     return inter.loaddata(data);
        // }



        [Route("getalldetails/{id:int}")]
        public TT_ClasswiseConsolidatedReportDTO getalldetails(int id)
        {
            return inter.getalldetails(id);
        }
        [Route("Report")]
        public TT_ClasswiseConsolidatedReportDTO Report([FromBody] TT_ClasswiseConsolidatedReportDTO data)
        {

            return inter.Report(data);
        }
        [Route("getabreport")]
        public TT_ClasswiseConsolidatedReportDTO getabreport([FromBody] TT_ClasswiseConsolidatedReportDTO data)
        {

            return inter.getabreport(data);
        }

    }
}
