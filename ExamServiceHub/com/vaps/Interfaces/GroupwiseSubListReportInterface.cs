using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface GroupwiseSubListReportInterface
    {
        GroupwiseSubListReportDTO getdetails(GroupwiseSubListReportDTO data);
        GroupwiseSubListReportDTO onchangeyear(GroupwiseSubListReportDTO data);
        GroupwiseSubListReportDTO onreport(GroupwiseSubListReportDTO data);
    }
}
