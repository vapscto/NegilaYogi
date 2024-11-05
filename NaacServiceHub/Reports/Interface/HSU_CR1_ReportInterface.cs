using PreadmissionDTOs.NAAC.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Reports.Interface
{
   public interface HSU_CR1_ReportInterface
    {
        HSU_CR1_Report_DTO getdata(HSU_CR1_Report_DTO data);
        Task<HSU_CR1_Report_DTO> HSU_112_Report(HSU_CR1_Report_DTO data);
        Task<HSU_CR1_Report_DTO> HSU_132_133_Report(HSU_CR1_Report_DTO data);
        Task<HSU_CR1_Report_DTO> HSU_141_Report(HSU_CR1_Report_DTO data);
        Task<HSU_CR1_Report_DTO> HSU_142_Report(HSU_CR1_Report_DTO data);
        Task<HSU_CR1_Report_DTO> HSU_121_Report(HSU_CR1_Report_DTO data);
        Task<HSU_CR1_Report_DTO> HSU_122_Report(HSU_CR1_Report_DTO data);
        Task<HSU_CR1_Report_DTO> HSU_123_Report(HSU_CR1_Report_DTO data);
    }
}
