

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Exam;

//PreadmissionDTOs.com.vaps.admission


namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface ProgressCardReportInterface
    {
        ProgressCardReportDTO savedetails(ProgressCardReportDTO data); 
        ProgressCardReportDTO validateordernumber(ProgressCardReportDTO data);
        ProgressCardReportDTO deactivate(ProgressCardReportDTO data);

        ProgressCardReportDTO editdetails(int ID);

        ProgressCardReportDTO Getdetails(ProgressCardReportDTO data);
    }
}
