using PreadmissionDTOs.NAAC.FeedBack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Feedback.Interface
{
   public interface FeedBackSchoolReportInterface
    {
        FeedBackSchoolReportDTO getdetails(FeedBackSchoolReportDTO data);
        FeedBackSchoolReportDTO getreport(FeedBackSchoolReportDTO data);
        //count
        FeedBackSchoolReportDTO count(FeedBackSchoolReportDTO data);
        //onclass
        FeedBackSchoolReportDTO onclass(FeedBackSchoolReportDTO data);
    }
}
