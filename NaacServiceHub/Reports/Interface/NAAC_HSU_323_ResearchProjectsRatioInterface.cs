using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Reports.Interface
{
   public interface NAAC_HSU_323_ResearchProjectsRatioInterface
    {
        HSU_323_ResearchProjectsRatioDTO getdata(HSU_323_ResearchProjectsRatioDTO data);
        Task<HSU_323_ResearchProjectsRatioDTO> get_323U_report(HSU_323_ResearchProjectsRatioDTO data); Task<HSU_323_ResearchProjectsRatioDTO> get_334U_report(HSU_323_ResearchProjectsRatioDTO data);       Task<HSU_323_ResearchProjectsRatioDTO> get_344U_report(HSU_323_ResearchProjectsRatioDTO data);
        Task<HSU_323_ResearchProjectsRatioDTO> get_333U_report(HSU_323_ResearchProjectsRatioDTO data);
        Task<HSU_323_ResearchProjectsRatioDTO> get_349U_report(HSU_323_ResearchProjectsRatioDTO data);
        Task<HSU_323_ResearchProjectsRatioDTO> get_348U_report(HSU_323_ResearchProjectsRatioDTO data);
    }
}
