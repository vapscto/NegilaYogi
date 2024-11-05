using PreadmissionDTOs.NAAC.Admission.Criteria8;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface.Criteria8
{
   public interface NaacPGDegrees813Interface
    {

        NAAC_MC_813_PGDegrees_DTO loaddata(NAAC_MC_813_PGDegrees_DTO data);
        NAAC_MC_813_PGDegrees_DTO save(NAAC_MC_813_PGDegrees_DTO data);
        NAAC_MC_813_PGDegrees_DTO deactive(NAAC_MC_813_PGDegrees_DTO data);
        NAAC_MC_813_PGDegrees_DTO EditData(NAAC_MC_813_PGDegrees_DTO data);
        NAAC_MC_813_PGDegrees_DTO viewuploadflies(NAAC_MC_813_PGDegrees_DTO data);
        NAAC_MC_813_PGDegrees_DTO deleteuploadfile(NAAC_MC_813_PGDegrees_DTO data);
        NAAC_MC_813_PGDegrees_DTO getcomment(NAAC_MC_813_PGDegrees_DTO data);
        NAAC_MC_813_PGDegrees_DTO getfilecomment(NAAC_MC_813_PGDegrees_DTO data);
        NAAC_MC_813_PGDegrees_DTO savecomments(NAAC_MC_813_PGDegrees_DTO data);
        NAAC_MC_813_PGDegrees_DTO savefilewisecomments(NAAC_MC_813_PGDegrees_DTO data);
    }
}
