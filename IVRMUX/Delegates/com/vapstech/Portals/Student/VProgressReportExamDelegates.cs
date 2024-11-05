using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;
using CommonLibrary;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Delegates.com.vapstech.Portals
{
    [Route("api/[controller]")]
    public class VProgressReportExamDelegates : Controller
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<VikasaSubjectwiseCumulativeReportDTO, VikasaSubjectwiseCumulativeReportDTO> COMMM = new CommonDelegate<VikasaSubjectwiseCumulativeReportDTO, VikasaSubjectwiseCumulativeReportDTO>();



        public VikasaSubjectwiseCumulativeReportDTO Getdetails(VikasaSubjectwiseCumulativeReportDTO data)
        {
            return COMMM.POSTPORTALData(data, "VProgressReportExamFacade/Getdetails/");

        }

        public VikasaSubjectwiseCumulativeReportDTO showdetails(VikasaSubjectwiseCumulativeReportDTO data)//Int32 IVRMM_Id
        {
            return COMMM.POSTPORTALData(data, "VProgressReportExamFacade/savedetails/");

        }

        public VikasaSubjectwiseCumulativeReportDTO get_exam(VikasaSubjectwiseCumulativeReportDTO data)
        {
            return COMMM.POSTPORTALData(data, "VProgressReportExamFacade/get_exam/");
        }
        public VikasaSubjectwiseCumulativeReportDTO get_Category(VikasaSubjectwiseCumulativeReportDTO data)
        {
            return COMMM.POSTPORTALData(data, "VProgressReportExamFacade/get_Category/");
        }
        public VikasaSubjectwiseCumulativeReportDTO aggregativereport(VikasaSubjectwiseCumulativeReportDTO data)
        {
            return COMMM.POSTPORTALData(data, "VProgressReportExamFacade/aggregativereport/");
        }

    }
}
