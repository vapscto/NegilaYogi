using PreadmissionDTOs.NAAC.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Reports.Interface
{
   public interface Medical_Criteria2ReportsInterface
    {
        Medical_Criteria2Reports_DTO getdata(Medical_Criteria2Reports_DTO data);
        Task<Medical_Criteria2Reports_DTO> MC_221_Report(Medical_Criteria2Reports_DTO data);
        Task<Medical_Criteria2Reports_DTO> MC_254_Report(Medical_Criteria2Reports_DTO data);
        Task<Medical_Criteria2Reports_DTO> MC_232_Report(Medical_Criteria2Reports_DTO data);
        Task<Medical_Criteria2Reports_DTO> MC_212_Report(Medical_Criteria2Reports_DTO data);
        Task<Medical_Criteria2Reports_DTO> MC_213_report(Medical_Criteria2Reports_DTO data);
        Task<Medical_Criteria2Reports_DTO> MC_222_Report(Medical_Criteria2Reports_DTO data);
        Task<Medical_Criteria2Reports_DTO> MC_234_Report(Medical_Criteria2Reports_DTO data);
        Task<Medical_Criteria2Reports_DTO> MC_241_Report(Medical_Criteria2Reports_DTO data);
        Task<Medical_Criteria2Reports_DTO> MC_242_Report(Medical_Criteria2Reports_DTO data);
        Task<Medical_Criteria2Reports_DTO> MC_243_Report(Medical_Criteria2Reports_DTO data);
        Task<Medical_Criteria2Reports_DTO> MC_244_Report(Medical_Criteria2Reports_DTO data);
        Task<Medical_Criteria2Reports_DTO> MC_245_Report(Medical_Criteria2Reports_DTO data);
        Task<Medical_Criteria2Reports_DTO> MC_262_Report(Medical_Criteria2Reports_DTO data);
        Task<Medical_Criteria2Reports_DTO> MC_271_Report(Medical_Criteria2Reports_DTO data);
    }
}
