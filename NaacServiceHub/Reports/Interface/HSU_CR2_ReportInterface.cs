using PreadmissionDTOs.NAAC.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Reports.Interface
{
   public interface HSU_CR2_ReportInterface
    {
        HSU_CR2_Report_DTO getdata(HSU_CR2_Report_DTO data);
        Task<HSU_CR2_Report_DTO> HSU_211_Report(HSU_CR2_Report_DTO data);
        Task<HSU_CR2_Report_DTO> HSU_212_Report(HSU_CR2_Report_DTO data);
        Task<HSU_CR2_Report_DTO> HSU_213_Report(HSU_CR2_Report_DTO data);
        Task<HSU_CR2_Report_DTO> HSU_221_Report(HSU_CR2_Report_DTO data);
        Task<HSU_CR2_Report_DTO> HSU_222_Report(HSU_CR2_Report_DTO data);
        Task<HSU_CR2_Report_DTO> HSU_232_Report(HSU_CR2_Report_DTO data);
        Task<HSU_CR2_Report_DTO> HSU_234_Report(HSU_CR2_Report_DTO data);
        Task<HSU_CR2_Report_DTO> HSU_241_Report(HSU_CR2_Report_DTO data);
        Task<HSU_CR2_Report_DTO> HSU_242_Report(HSU_CR2_Report_DTO data);
        Task<HSU_CR2_Report_DTO> HSU_243_Report(HSU_CR2_Report_DTO data);
        Task<HSU_CR2_Report_DTO> HSU_244_Report(HSU_CR2_Report_DTO data);
        Task<HSU_CR2_Report_DTO> HSU_245_Report(HSU_CR2_Report_DTO data);
        Task<HSU_CR2_Report_DTO> HSU_251_Report(HSU_CR2_Report_DTO data);
        Task<HSU_CR2_Report_DTO> HSU_252_Report(HSU_CR2_Report_DTO data);
        Task<HSU_CR2_Report_DTO> HSU_253_Report(HSU_CR2_Report_DTO data);
        Task<HSU_CR2_Report_DTO> HSU_255_Report(HSU_CR2_Report_DTO data);
        Task<HSU_CR2_Report_DTO> HSU_262_Report(HSU_CR2_Report_DTO data);
        Task<HSU_CR2_Report_DTO> HSU_271_Report(HSU_CR2_Report_DTO data);
                                 
    }                            
}                                
                                 