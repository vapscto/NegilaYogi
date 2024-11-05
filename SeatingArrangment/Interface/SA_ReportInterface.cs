using PreadmissionDTOs.SeatingArrangment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeatingArrangment.Interface
{
    public interface SA_ReportInterface
    {
        SA_ReportDTO GetExamDateloaddata(SA_ReportDTO data);
        SA_ReportDTO OnChangeyear(SA_ReportDTO data);
        SA_ReportDTO GetAbsentStudentReport(SA_ReportDTO data);
        SA_ReportDTO GetMalpracticeStudentReport(SA_ReportDTO data);
    }
}
