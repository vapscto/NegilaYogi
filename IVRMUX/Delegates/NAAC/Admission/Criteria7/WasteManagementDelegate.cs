using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class WasteManagementDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<NAAC_AC_718_WasteManagement_DTO, NAAC_AC_718_WasteManagement_DTO> COMMM = new CommonDelegate<NAAC_AC_718_WasteManagement_DTO, NAAC_AC_718_WasteManagement_DTO>();

        public NAAC_AC_718_WasteManagement_DTO loaddata(NAAC_AC_718_WasteManagement_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "WasteManagementFacade/loaddata/");
        }

        public NAAC_AC_718_WasteManagement_DTO savedatatab1(NAAC_AC_718_WasteManagement_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "WasteManagementFacade/savedatatab1");
        }
        public NAAC_AC_718_WasteManagement_DTO editTab1(NAAC_AC_718_WasteManagement_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "WasteManagementFacade/editTab1");
        }
        public NAAC_AC_718_WasteManagement_DTO getcomment(NAAC_AC_718_WasteManagement_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "WasteManagementFacade/getcomment");
        }
        public NAAC_AC_718_WasteManagement_DTO savemedicaldatawisecomments(NAAC_AC_718_WasteManagement_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "WasteManagementFacade/savemedicaldatawisecomments");
        }
        public NAAC_AC_718_WasteManagement_DTO savefilewisecomments(NAAC_AC_718_WasteManagement_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "WasteManagementFacade/savefilewisecomments");
        }
        public NAAC_AC_718_WasteManagement_DTO getfilecomment(NAAC_AC_718_WasteManagement_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "WasteManagementFacade/getfilecomment");
        }

        public NAAC_AC_718_WasteManagement_DTO deactivYTab1(NAAC_AC_718_WasteManagement_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "WasteManagementFacade/deactivYTab1");
        }

        public NAAC_AC_718_WasteManagement_DTO deleteuploadfile(NAAC_AC_718_WasteManagement_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "WasteManagementFacade/deleteuploadfile");
        }

        public NAAC_AC_718_WasteManagement_DTO getData(NAAC_AC_718_WasteManagement_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "WasteManagementFacade/getData");
        }
    }
}
