using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.College.Portals.IVRM;
using PreadmissionDTOs.com.vaps.Portals.IVRM;
using PreadmissionDTOs.com.vaps.Portals.Student;

namespace CollegePortals.com.vaps.IVRM.Interfaces
{
    public interface Clg_IVRM_GalleryInterface
    {
        ClgIVRMGalleryDTO getloaddata(ClgIVRMGalleryDTO data);
        ClgIVRMGalleryDTO get_branch(ClgIVRMGalleryDTO data);
        ClgIVRMGalleryDTO get_semester(ClgIVRMGalleryDTO data);
        ClgIVRMGalleryDTO get_Section(ClgIVRMGalleryDTO data);
        ClgIVRMGalleryDTO savedata(ClgIVRMGalleryDTO data);
        ClgIVRMGalleryDTO getcovermodel(ClgIVRMGalleryDTO data);
        ClgIVRMGalleryDTO savecover(ClgIVRMGalleryDTO data);
        ClgIVRMGalleryDTO deactive(ClgIVRMGalleryDTO data);

    }

}
