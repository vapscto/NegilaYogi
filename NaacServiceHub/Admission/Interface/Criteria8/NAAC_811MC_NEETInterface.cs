using PreadmissionDTOs.NAAC.Admission.Criteria8;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface.Criteria8
{
   public interface NAAC_811MC_NEETInterface
    {
        Task<NAAC_811MC_NEET_DTO> loaddata(NAAC_811MC_NEET_DTO data);
        NAAC_811MC_NEET_DTO savedata(NAAC_811MC_NEET_DTO data);
        NAAC_811MC_NEET_DTO editdata(NAAC_811MC_NEET_DTO data);
        NAAC_811MC_NEET_DTO deactivY(NAAC_811MC_NEET_DTO data);
        NAAC_811MC_NEET_DTO viewuploadflies(NAAC_811MC_NEET_DTO data);
        NAAC_811MC_NEET_DTO deleteuploadfile(NAAC_811MC_NEET_DTO data);
        NAAC_811MC_NEET_DTO getcomment(NAAC_811MC_NEET_DTO data);
        NAAC_811MC_NEET_DTO getfilecomment(NAAC_811MC_NEET_DTO data);
        NAAC_811MC_NEET_DTO savecomments(NAAC_811MC_NEET_DTO data);
        NAAC_811MC_NEET_DTO savefilewisecomments(NAAC_811MC_NEET_DTO data);
    }
}
