using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;


namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface SiblingEmployeeStudentReportInterface
    {
        SiblingEmployeeStudentReportDTO getdetails(SiblingEmployeeStudentReportDTO data);
        SiblingEmployeeStudentReportDTO getreport(SiblingEmployeeStudentReportDTO data);
    }
}
