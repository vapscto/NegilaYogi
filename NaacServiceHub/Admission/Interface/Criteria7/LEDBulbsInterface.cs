using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface.Criteria7
{
    public interface LEDBulbsInterface
    {
        Task<NAAC_AC_714_LEDBulbs_DTO> loaddata(NAAC_AC_714_LEDBulbs_DTO data);
        NAAC_AC_714_LEDBulbs_DTO savedatatab1(NAAC_AC_714_LEDBulbs_DTO data);
        NAAC_AC_714_LEDBulbs_DTO editTab1(NAAC_AC_714_LEDBulbs_DTO data);
        NAAC_AC_714_LEDBulbs_DTO savemedicaldatawisecommentsLEDbulb(NAAC_AC_714_LEDBulbs_DTO data);
        NAAC_AC_714_LEDBulbs_DTO savefilewisecommentsLEDbulb(NAAC_AC_714_LEDBulbs_DTO data);
        NAAC_AC_714_LEDBulbs_DTO getcommentLEDbulb(NAAC_AC_714_LEDBulbs_DTO data);
        NAAC_AC_714_LEDBulbs_DTO getfilecommentLEDbulb(NAAC_AC_714_LEDBulbs_DTO data);
        NAAC_AC_714_LEDBulbs_DTO deactivYTab1(NAAC_AC_714_LEDBulbs_DTO data);
        NAAC_AC_714_LEDBulbs_DTO deleteuploadfile(NAAC_AC_714_LEDBulbs_DTO data);
        NAAC_AC_714_LEDBulbs_DTO getData(NAAC_AC_714_LEDBulbs_DTO data);

        //MC
        NAAC_AC_714_LEDBulbs_DTO getDataMCwater(NAAC_AC_714_LEDBulbs_DTO data);
        NAAC_AC_714_LEDBulbs_DTO saveMCwater(NAAC_AC_714_LEDBulbs_DTO data);
        NAAC_AC_714_LEDBulbs_DTO EditDataMCwater(NAAC_AC_714_LEDBulbs_DTO data);
        NAAC_AC_714_LEDBulbs_DTO savemedicaldatawisecomments(NAAC_AC_714_LEDBulbs_DTO data);
        NAAC_AC_714_LEDBulbs_DTO getcomment(NAAC_AC_714_LEDBulbs_DTO data);
        NAAC_AC_714_LEDBulbs_DTO deactivateMCwater(NAAC_AC_714_LEDBulbs_DTO data);

        NAAC_AC_714_LEDBulbs_DTO getDataMCgreen(NAAC_AC_714_LEDBulbs_DTO data);
        NAAC_AC_714_LEDBulbs_DTO saveMCgreen(NAAC_AC_714_LEDBulbs_DTO data);
        NAAC_AC_714_LEDBulbs_DTO EditDataMCgreen(NAAC_AC_714_LEDBulbs_DTO data);
        NAAC_AC_714_LEDBulbs_DTO deactivateMCgreen(NAAC_AC_714_LEDBulbs_DTO data);

        NAAC_AC_714_LEDBulbs_DTO getDataMCdisable(NAAC_AC_714_LEDBulbs_DTO data);
        NAAC_AC_714_LEDBulbs_DTO saveMCdisable(NAAC_AC_714_LEDBulbs_DTO data);
        NAAC_AC_714_LEDBulbs_DTO EditDataMCdisable(NAAC_AC_714_LEDBulbs_DTO data);
        NAAC_AC_714_LEDBulbs_DTO deactivateMCdisable(NAAC_AC_714_LEDBulbs_DTO data);
        //MC
    }
}
