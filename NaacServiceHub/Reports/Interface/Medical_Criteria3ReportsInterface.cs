using PreadmissionDTOs.NAAC.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Reports.Interface
{
   public interface Medical_Criteria3ReportsInterface
    {
        Medical_Criteria3Reports_DTO getdata(Medical_Criteria3Reports_DTO data);
        Task<Medical_Criteria3Reports_DTO> MC_311_Report(Medical_Criteria3Reports_DTO data);
        Task<Medical_Criteria3Reports_DTO> MC_312_Report(Medical_Criteria3Reports_DTO data);
        Task<Medical_Criteria3Reports_DTO> MC_313_Report(Medical_Criteria3Reports_DTO data);
        Task<Medical_Criteria3Reports_DTO> MC_322_Report(Medical_Criteria3Reports_DTO data);
        Task<Medical_Criteria3Reports_DTO> MC_331_report(Medical_Criteria3Reports_DTO data);
        Task<Medical_Criteria3Reports_DTO> MC_332_Report(Medical_Criteria3Reports_DTO data);
        Task<Medical_Criteria3Reports_DTO> MC_333_Report(Medical_Criteria3Reports_DTO data);
        Task<Medical_Criteria3Reports_DTO> MC_334_Report(Medical_Criteria3Reports_DTO data);
        Task<Medical_Criteria3Reports_DTO> MC_341_Report(Medical_Criteria3Reports_DTO data);
        Task<Medical_Criteria3Reports_DTO> MC_342_Report(Medical_Criteria3Reports_DTO data);
        Task<Medical_Criteria3Reports_DTO> MC_351_Report(Medical_Criteria3Reports_DTO data);
        Task<Medical_Criteria3Reports_DTO> MC_352_Report(Medical_Criteria3Reports_DTO data);

    }
}
