using PreadmissionDTOs.NAAC.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Medical.Interface
{
   public interface NAAC_MC_423_StuLearningResourceInterface
    {
        NAAC_MC_423_StuLearningResource_DTO loaddata(NAAC_MC_423_StuLearningResource_DTO data);
        NAAC_MC_423_StuLearningResource_DTO save(NAAC_MC_423_StuLearningResource_DTO data);
      
        NAAC_MC_423_StuLearningResource_DTO EditData(NAAC_MC_423_StuLearningResource_DTO data);
        NAAC_MC_423_StuLearningResource_DTO viewuploadflies(NAAC_MC_423_StuLearningResource_DTO data);
        NAAC_MC_423_StuLearningResource_DTO deleteuploadfile(NAAC_MC_423_StuLearningResource_DTO obj);
        NAAC_MC_423_StuLearningResource_DTO loaddatainfra(NAAC_MC_423_StuLearningResource_DTO data);
        NAAC_MC_423_StuLearningResource_DTO saveinfra(NAAC_MC_423_StuLearningResource_DTO data);

        NAAC_MC_423_StuLearningResource_DTO EditDatainfra(NAAC_MC_423_StuLearningResource_DTO data);
    }
}
