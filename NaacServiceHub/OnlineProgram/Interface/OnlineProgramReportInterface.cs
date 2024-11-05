using PreadmissionDTOs.NAAC.OnlineProgram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.OnlineProgram.Interface
{
   public interface OnlineProgramReportInterface
    {
        OnlineProgramReport_DTO getyearlyprogram(OnlineProgramReport_DTO data);
        Task<OnlineProgramReport_DTO> getYearlyProgramReport(OnlineProgramReport_DTO data);
        Task<OnlineProgramReport_DTO> ConferenceDetailsReport(OnlineProgramReport_DTO data);
    }
}
