using PreadmissionDTOs.NAAC.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Reports.Interface
{
    public interface Medical_Criteria1ReportsInterface
    {
        Medical_Criteria1Reports_DTO getdata(Medical_Criteria1Reports_DTO data);
        Task<Medical_Criteria1Reports_DTO> get_report_MC_112Async(Medical_Criteria1Reports_DTO data);
        Medical_Criteria1Reports_DTO report_MC_141(Medical_Criteria1Reports_DTO data);
        Medical_Criteria1Reports_DTO report_MC_142(Medical_Criteria1Reports_DTO data);
        Task<Medical_Criteria1Reports_DTO> M_IDC121_Report(Medical_Criteria1Reports_DTO data);
        Task<Medical_Criteria1Reports_DTO> M_SRC122_Report(Medical_Criteria1Reports_DTO data);
        Task<Medical_Criteria1Reports_DTO> MC_VAC_report_132(Medical_Criteria1Reports_DTO data);
        Task<Medical_Criteria1Reports_DTO> StudentsEnrolledInVAC133_report(Medical_Criteria1Reports_DTO data);
        Task<Medical_Criteria1Reports_DTO> MC_StudentUTFV_134_Report(Medical_Criteria1Reports_DTO data);
    }
}
