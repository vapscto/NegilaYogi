using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.HOD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Portals.HOD
{
    public class HODExamSectionPerformanceDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HODExamSectionPerformance_DTO, HODExamSectionPerformance_DTO> COMMM = new CommonDelegate<HODExamSectionPerformance_DTO, HODExamSectionPerformance_DTO>();

        public HODExamSectionPerformance_DTO Getdetails(HODExamSectionPerformance_DTO data)
        {
            return COMMM.POSTPORTALData(data, "HODExamSectionPerformanceFacade/Getdetails/");
        }

        public HODExamSectionPerformance_DTO getcategory(HODExamSectionPerformance_DTO data)
        {
            return COMMM.POSTPORTALData(data, "HODExamSectionPerformanceFacade/getcategory/");
        }
        public HODExamSectionPerformance_DTO getclassexam(HODExamSectionPerformance_DTO data)
        {
            return COMMM.POSTPORTALData(data, "HODExamSectionPerformanceFacade/getclassexam/");
        }
        public HODExamSectionPerformance_DTO showreport(HODExamSectionPerformance_DTO data)
        {
            return COMMM.POSTPORTALData(data, "HODExamSectionPerformanceFacade/showreport/");
        }

        public HODExamSectionPerformance_DTO getsubject(HODExamSectionPerformance_DTO data)
        {
            return COMMM.POSTPORTALData(data, "HODExamSectionPerformanceFacade/getsubject/");
        }


    }
}
