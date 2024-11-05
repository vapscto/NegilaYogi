using CommonLibrary;
using PreadmissionDTOs.com.vaps.Alumni;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Alumni
{
    public class Alumni_Gallery_Delegate
    {
        CommonDelegate<Alumni_GalleryDTO, Alumni_GalleryDTO> COMMM = new CommonDelegate<Alumni_GalleryDTO, Alumni_GalleryDTO>();

        public Alumni_GalleryDTO getloaddata(Alumni_GalleryDTO data)
        {
            return COMMM.POSTDataAlumni(data, "Alumni_GalleryFacade/getloaddata/");
        }
       
        public Alumni_GalleryDTO savedata(Alumni_GalleryDTO data)
        {
            return COMMM.POSTDataAlumni(data, "Alumni_GalleryFacade/savedata/");
        }
        public Alumni_GalleryDTO getcovermodel(Alumni_GalleryDTO data)
        {
            return COMMM.POSTDataAlumni(data, "Alumni_GalleryFacade/getcovermodel/");
        }
        public Alumni_GalleryDTO savecover(Alumni_GalleryDTO data)
        {
            return COMMM.POSTDataAlumni(data, "Alumni_GalleryFacade/savecover/");
        }
        public Alumni_GalleryDTO deactive(Alumni_GalleryDTO data)
        {
            return COMMM.POSTDataAlumni(data, "Alumni_GalleryFacade/deactive/");
        }

    }
}
