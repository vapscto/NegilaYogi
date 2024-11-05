using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.University.Interface
{
   public interface HSU_323_ResearchProjectsRatioInterface
    {
        HSU_323_ResearchProjectsRatioDTO loaddata(HSU_323_ResearchProjectsRatioDTO data);
        HSU_323_ResearchProjectsRatioDTO save(HSU_323_ResearchProjectsRatioDTO data);
        HSU_323_ResearchProjectsRatioDTO deactive(HSU_323_ResearchProjectsRatioDTO data);
        HSU_323_ResearchProjectsRatioDTO EditData(HSU_323_ResearchProjectsRatioDTO data);
        HSU_323_ResearchProjectsRatioDTO viewuploadflies(HSU_323_ResearchProjectsRatioDTO data);
        HSU_323_ResearchProjectsRatioDTO deleteuploadfile(HSU_323_ResearchProjectsRatioDTO data);
    }
}
