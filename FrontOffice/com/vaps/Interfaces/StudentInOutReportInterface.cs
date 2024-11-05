using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOffice.com.vaps.Interfaces
{
    public interface StudentInOutReportInterface
    {
        StudentInOutReportDTO loaddata(StudentInOutReportDTO data);
        StudentInOutReportDTO getsection(StudentInOutReportDTO data);
        StudentInOutReportDTO getstudent(StudentInOutReportDTO data);
        StudentInOutReportDTO report(StudentInOutReportDTO data);
    }
}
