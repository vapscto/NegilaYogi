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

namespace corewebapi18072016.Delegates.com.vapstech.Portals.IVRM
{
    public class IVRM_GalleryDelegate
    {

        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<IVRM_GalleryDTO, IVRM_GalleryDTO> COMMM = new CommonDelegate<IVRM_GalleryDTO, IVRM_GalleryDTO>();

        public IVRM_GalleryDTO getloaddata(IVRM_GalleryDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_GalleryFacade/getloaddata/");
        }
        public IVRM_GalleryDTO get_section(IVRM_GalleryDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_GalleryFacade/get_section/");
        }
        public IVRM_GalleryDTO savedata(IVRM_GalleryDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_GalleryFacade/savedata/");
        }
        public IVRM_GalleryDTO getcovermodel(IVRM_GalleryDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_GalleryFacade/getcovermodel/");
        }
        public IVRM_GalleryDTO savecover(IVRM_GalleryDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_GalleryFacade/savecover/");
        }
         public IVRM_GalleryDTO deactive(IVRM_GalleryDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_GalleryFacade/deactive/");
        }


        //edit
        public IVRM_GalleryDTO Editdetails(IVRM_GalleryDTO data)
        {
            return COMMM.POSTPORTALData(data, "IVRM_GalleryFacade/Editdetails/");
        }
    }
}
