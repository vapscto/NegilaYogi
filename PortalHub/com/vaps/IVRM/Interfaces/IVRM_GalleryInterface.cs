using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Portals.IVRM;
using PreadmissionDTOs.com.vaps.Portals.Student;

namespace PortalHub.com.vaps.IVRM.Interfaces
{
    public interface IVRM_GalleryInterface
    {
        IVRM_GalleryDTO getloaddata(IVRM_GalleryDTO data);
        IVRM_GalleryDTO get_section(IVRM_GalleryDTO data);
        IVRM_GalleryDTO savedata(IVRM_GalleryDTO data);
        IVRM_GalleryDTO getcovermodel(IVRM_GalleryDTO data);
        IVRM_GalleryDTO savecover(IVRM_GalleryDTO data);
        IVRM_GalleryDTO deactive(IVRM_GalleryDTO data);

        //edit
        IVRM_GalleryDTO Editdetails(IVRM_GalleryDTO data);

        //kiosk
        IVRM_GalleryDTO kioskvideo(IVRM_GalleryDTO data);



    }

}
