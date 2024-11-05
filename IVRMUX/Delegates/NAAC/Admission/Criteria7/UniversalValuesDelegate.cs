using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class UniversalValuesDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<NAAC_AC_7117_UniversalValues_DTO, NAAC_AC_7117_UniversalValues_DTO> COMMM = new CommonDelegate<NAAC_AC_7117_UniversalValues_DTO, NAAC_AC_7117_UniversalValues_DTO>();

        public NAAC_AC_7117_UniversalValues_DTO loaddata(NAAC_AC_7117_UniversalValues_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "UniversalValuesFacade/loaddata/");
        }
        public NAAC_AC_7117_UniversalValues_DTO savedatatab1(NAAC_AC_7117_UniversalValues_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "UniversalValuesFacade/savedatatab1");
        }
        public NAAC_AC_7117_UniversalValues_DTO getfilecomment(NAAC_AC_7117_UniversalValues_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "UniversalValuesFacade/getfilecomment");
        }
        public NAAC_AC_7117_UniversalValues_DTO savefilewisecomments(NAAC_AC_7117_UniversalValues_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "UniversalValuesFacade/savefilewisecomments");
        }
        public NAAC_AC_7117_UniversalValues_DTO savemedicaldatawisecomments(NAAC_AC_7117_UniversalValues_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "UniversalValuesFacade/savemedicaldatawisecomments");
        }
        public NAAC_AC_7117_UniversalValues_DTO getcomment(NAAC_AC_7117_UniversalValues_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "UniversalValuesFacade/getcomment");
        }

        public NAAC_AC_7117_UniversalValues_DTO editTab1(NAAC_AC_7117_UniversalValues_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "UniversalValuesFacade/editTab1");
        }

        public NAAC_AC_7117_UniversalValues_DTO deactivYTab1(NAAC_AC_7117_UniversalValues_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "UniversalValuesFacade/deactivYTab1");
        }

        public NAAC_AC_7117_UniversalValues_DTO deleteuploadfile(NAAC_AC_7117_UniversalValues_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "UniversalValuesFacade/deleteuploadfile");
        }

        public NAAC_AC_7117_UniversalValues_DTO getData(NAAC_AC_7117_UniversalValues_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "UniversalValuesFacade/getData");
        }
    }
}
