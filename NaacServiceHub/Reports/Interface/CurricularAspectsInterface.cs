using PreadmissionDTOs.NAAC.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Reports.Interface
{
   public interface CurricularAspectsInterface
    {
        CurricularAspects_DTO getdata(CurricularAspects_DTO data);
        Task<CurricularAspects_DTO> get_report(CurricularAspects_DTO data);
        Task<CurricularAspects_DTO> get_nCourse_report(CurricularAspects_DTO data);
        Task<CurricularAspects_DTO> get_report_113(CurricularAspects_DTO data);
        Task<CurricularAspects_DTO> get_report_123(CurricularAspects_DTO data);
        Task<CurricularAspects_DTO> get_report_133(CurricularAspects_DTO data);
        Task<CurricularAspects_DTO> get_report_132(CurricularAspects_DTO data);
        Task<CurricularAspects_DTO> get_122CBCSsystemReport(CurricularAspects_DTO data);
         

    }
}
