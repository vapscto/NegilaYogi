using PreadmissionDTOs.com.vaps.Portals.HOD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.HOD.Interfaces
{
    public interface HODExamSectionPerformanceInterface
    {
        HODExamSectionPerformance_DTO Getdetails(HODExamSectionPerformance_DTO data);
        HODExamSectionPerformance_DTO getcategory(HODExamSectionPerformance_DTO data);
        HODExamSectionPerformance_DTO getclassexam(HODExamSectionPerformance_DTO data);
        HODExamSectionPerformance_DTO showreport(HODExamSectionPerformance_DTO data);
        HODExamSectionPerformance_DTO getsubject(HODExamSectionPerformance_DTO data);

    }
}
