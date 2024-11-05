using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.University.Interface
{
   public interface NAAC_HSU_InterdisciplinaryProgrammes_123Interface
    {

        NAAC_HSU_InterdisciplinaryProgrammes_123_DTO loaddata(NAAC_HSU_InterdisciplinaryProgrammes_123_DTO data);
        NAAC_HSU_InterdisciplinaryProgrammes_123_DTO save(NAAC_HSU_InterdisciplinaryProgrammes_123_DTO data);
        NAAC_HSU_InterdisciplinaryProgrammes_123_DTO deactive(NAAC_HSU_InterdisciplinaryProgrammes_123_DTO data);
        NAAC_HSU_InterdisciplinaryProgrammes_123_DTO EditData(NAAC_HSU_InterdisciplinaryProgrammes_123_DTO data);
        NAAC_HSU_InterdisciplinaryProgrammes_123_DTO viewuploadflies(NAAC_HSU_InterdisciplinaryProgrammes_123_DTO data);
        NAAC_HSU_InterdisciplinaryProgrammes_123_DTO deleteuploadfile(NAAC_HSU_InterdisciplinaryProgrammes_123_DTO obj);
    }
}
