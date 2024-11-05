using PreadmissionDTOs.NAAC.Admission.Criteria8;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface.Criteria8
{
  public  interface NaacImmunisation8110Interface
    {

        NAAC_MC_8110_Immunisation_DTO loaddata(NAAC_MC_8110_Immunisation_DTO data);
        NAAC_MC_8110_Immunisation_DTO save(NAAC_MC_8110_Immunisation_DTO data);
        NAAC_MC_8110_Immunisation_DTO deactive(NAAC_MC_8110_Immunisation_DTO data);
        NAAC_MC_8110_Immunisation_DTO EditData(NAAC_MC_8110_Immunisation_DTO data);
        NAAC_MC_8110_Immunisation_DTO viewuploadflies(NAAC_MC_8110_Immunisation_DTO data);
        NAAC_MC_8110_Immunisation_DTO deleteuploadfile(NAAC_MC_8110_Immunisation_DTO data);
        NAAC_MC_8110_Immunisation_DTO getcomment(NAAC_MC_8110_Immunisation_DTO data);
        NAAC_MC_8110_Immunisation_DTO getfilecomment(NAAC_MC_8110_Immunisation_DTO data);
        NAAC_MC_8110_Immunisation_DTO savecomments(NAAC_MC_8110_Immunisation_DTO data);
        NAAC_MC_8110_Immunisation_DTO savefilewisecomments(NAAC_MC_8110_Immunisation_DTO data);
    }
}
