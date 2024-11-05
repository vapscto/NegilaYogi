using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface.Criteria7
{
    public interface WasteManagementInterface
    {
        Task<NAAC_AC_718_WasteManagement_DTO> loaddata(NAAC_AC_718_WasteManagement_DTO data);
        NAAC_AC_718_WasteManagement_DTO savedatatab1(NAAC_AC_718_WasteManagement_DTO data);
        NAAC_AC_718_WasteManagement_DTO getfilecomment(NAAC_AC_718_WasteManagement_DTO data);
        NAAC_AC_718_WasteManagement_DTO savefilewisecomments(NAAC_AC_718_WasteManagement_DTO data);
        NAAC_AC_718_WasteManagement_DTO getcomment(NAAC_AC_718_WasteManagement_DTO data);
        NAAC_AC_718_WasteManagement_DTO savemedicaldatawisecomments(NAAC_AC_718_WasteManagement_DTO data);
        NAAC_AC_718_WasteManagement_DTO editTab1(NAAC_AC_718_WasteManagement_DTO data);
        NAAC_AC_718_WasteManagement_DTO deactivYTab1(NAAC_AC_718_WasteManagement_DTO data);
        NAAC_AC_718_WasteManagement_DTO deleteuploadfile(NAAC_AC_718_WasteManagement_DTO data);
        NAAC_AC_718_WasteManagement_DTO getData(NAAC_AC_718_WasteManagement_DTO data);
    }
}
