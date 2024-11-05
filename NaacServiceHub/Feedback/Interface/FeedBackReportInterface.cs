using PreadmissionDTOs.FeedBack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.FeedBack.Interface
{
    public interface FeedBackReportInterface
    {
        FeedBackReportDTO getdetails(FeedBackReportDTO data);
        FeedBackReportDTO onchangeradio(FeedBackReportDTO data);
        FeedBackReportDTO getreport(FeedBackReportDTO data);
        FeedBackReportDTO onchangeyear(FeedBackReportDTO data);
        FeedBackReportDTO getreportnew(FeedBackReportDTO data);
        FeedBackReportDTO onchangefeedback(FeedBackReportDTO data);
        FeedBackReportDTO getstudentlist(FeedBackReportDTO data);
        
    }
}
