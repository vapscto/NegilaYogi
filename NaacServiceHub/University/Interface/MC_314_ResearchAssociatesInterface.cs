using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.University.Interface
{
   public interface MC_314_ResearchAssociatesInterface
    {
        MC_314_ResearchAssociatesDTO loaddata(MC_314_ResearchAssociatesDTO data);
        MC_314_ResearchAssociatesDTO save(MC_314_ResearchAssociatesDTO data);
        MC_314_ResearchAssociatesDTO deactive(MC_314_ResearchAssociatesDTO data);
        MC_314_ResearchAssociatesDTO EditData(MC_314_ResearchAssociatesDTO data);
        MC_314_ResearchAssociatesDTO viewuploadflies(MC_314_ResearchAssociatesDTO data);
        MC_314_ResearchAssociatesDTO deleteuploadfile(MC_314_ResearchAssociatesDTO data);
    }
}
