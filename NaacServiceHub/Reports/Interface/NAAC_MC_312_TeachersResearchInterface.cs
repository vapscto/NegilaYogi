using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Reports.Interface
{
   public interface NAAC_MC_312_TeachersResearchInterface
    {
        UC_312_TeachersResearchDTO getdata(UC_312_TeachersResearchDTO data);
        Task<UC_312_TeachersResearchDTO> get_312U_report(UC_312_TeachersResearchDTO data);
        Task<UC_312_TeachersResearchDTO> get_313U_report(UC_312_TeachersResearchDTO data);
        Task<UC_312_TeachersResearchDTO> get_314U_report(UC_312_TeachersResearchDTO data);
        Task<UC_312_TeachersResearchDTO> get_315U_report(UC_312_TeachersResearchDTO data);
        Task<UC_312_TeachersResearchDTO> get_316U_report(UC_312_TeachersResearchDTO data);
        Task<UC_312_TeachersResearchDTO> get_342U_report(UC_312_TeachersResearchDTO data);
        Task<UC_312_TeachersResearchDTO> get_343U_report(UC_312_TeachersResearchDTO data);
        Task<UC_312_TeachersResearchDTO> get_372U_report(UC_312_TeachersResearchDTO data);
        Task<UC_312_TeachersResearchDTO> get_362U_report(UC_312_TeachersResearchDTO data);
        Task<UC_312_TeachersResearchDTO> get_352U_report(UC_312_TeachersResearchDTO data);
        Task<UC_312_TeachersResearchDTO> get_371U_report(UC_312_TeachersResearchDTO data);
        Task<UC_312_TeachersResearchDTO> get_341U_report(UC_312_TeachersResearchDTO data);
        Task<UC_312_TeachersResearchDTO> NAAC_MC_345_TeachersResearchReport(UC_312_TeachersResearchDTO data);
        Task<UC_312_TeachersResearchDTO> NAAC_MC_346_TeachersResearchReport(UC_312_TeachersResearchDTO data);
    }
}
