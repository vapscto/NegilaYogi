using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class HumanValuesDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<NAAC_AC_7114_HumanValues_DTO, NAAC_AC_7114_HumanValues_DTO> COMMM = new CommonDelegate<NAAC_AC_7114_HumanValues_DTO, NAAC_AC_7114_HumanValues_DTO>();

        public NAAC_AC_7114_HumanValues_DTO loaddata(NAAC_AC_7114_HumanValues_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "HumanValuesFacade/loaddata/");
        }

        public NAAC_AC_7114_HumanValues_DTO savedatatab1(NAAC_AC_7114_HumanValues_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "HumanValuesFacade/savedatatab1");
        }

        public NAAC_AC_7114_HumanValues_DTO editTab1(NAAC_AC_7114_HumanValues_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "HumanValuesFacade/editTab1");
        }

        public NAAC_AC_7114_HumanValues_DTO deactivYTab1(NAAC_AC_7114_HumanValues_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "HumanValuesFacade/deactivYTab1");
        }

        public NAAC_AC_7114_HumanValues_DTO deleteuploadfile(NAAC_AC_7114_HumanValues_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "HumanValuesFacade/deleteuploadfile");
        }

        public NAAC_AC_7114_HumanValues_DTO getData(NAAC_AC_7114_HumanValues_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "HumanValuesFacade/getData");
        }
        public NAAC_AC_7114_HumanValues_DTO getfilecomment(NAAC_AC_7114_HumanValues_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "HumanValuesFacade/getfilecomment");
        }
        public NAAC_AC_7114_HumanValues_DTO getcomment(NAAC_AC_7114_HumanValues_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "HumanValuesFacade/getcomment");
        }
        public NAAC_AC_7114_HumanValues_DTO savecomments(NAAC_AC_7114_HumanValues_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "HumanValuesFacade/savecomments");
        }
        public NAAC_AC_7114_HumanValues_DTO savefilewisecomments(NAAC_AC_7114_HumanValues_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "HumanValuesFacade/savefilewisecomments");
        }

    }
}
