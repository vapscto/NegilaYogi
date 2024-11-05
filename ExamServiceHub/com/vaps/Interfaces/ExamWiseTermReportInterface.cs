using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
   public interface ExamWiseTermReportInterface
    {

        Task<ExamWiseTermReport_DTO> Getdetails(ExamWiseTermReport_DTO data);
        ExamWiseTermReport_DTO onchangeyear(ExamWiseTermReport_DTO data);
        ExamWiseTermReport_DTO onchangeclass(ExamWiseTermReport_DTO data);
        ExamWiseTermReport_DTO onchangesection(ExamWiseTermReport_DTO data);

    }
}
