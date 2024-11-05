using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Portals.Student;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.IVRM;
using PreadmissionDTOs.com.vaps.College.Portals.IVRM;

namespace corewebapi18072016.Delegates.com.vapstech.Portals.IVRM
{
    public class Clg_IVRM_GalleryDelegate
    {

        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ClgIVRMGalleryDTO, ClgIVRMGalleryDTO> COMMM = new CommonDelegate<ClgIVRMGalleryDTO, ClgIVRMGalleryDTO>();

        public ClgIVRMGalleryDTO getloaddata(ClgIVRMGalleryDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "Clg_IVRM_GalleryFacade/getloaddata/");
        }
        public ClgIVRMGalleryDTO get_branch(ClgIVRMGalleryDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "Clg_IVRM_GalleryFacade/get_branch/");
        }
        public ClgIVRMGalleryDTO get_semester(ClgIVRMGalleryDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "Clg_IVRM_GalleryFacade/get_semester/");
        }
        public ClgIVRMGalleryDTO get_Section(ClgIVRMGalleryDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "Clg_IVRM_GalleryFacade/get_Section/");
        }
        public ClgIVRMGalleryDTO savedata(ClgIVRMGalleryDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "Clg_IVRM_GalleryFacade/savedata/");
        }
        public ClgIVRMGalleryDTO getcovermodel(ClgIVRMGalleryDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "Clg_IVRM_GalleryFacade/getcovermodel/");
        }
        public ClgIVRMGalleryDTO savecover(ClgIVRMGalleryDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "Clg_IVRM_GalleryFacade/savecover/");
        }
         public ClgIVRMGalleryDTO deactive(ClgIVRMGalleryDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "Clg_IVRM_GalleryFacade/deactive/");
        }
    }
}
