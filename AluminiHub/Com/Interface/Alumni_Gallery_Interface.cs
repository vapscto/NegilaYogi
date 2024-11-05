using PreadmissionDTOs.com.vaps.Alumni;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlumniHub.Com.Interface
{
    public interface Alumni_Gallery_Interface
    {
        Alumni_GalleryDTO getloaddata(Alumni_GalleryDTO data);
      
        Alumni_GalleryDTO savedata(Alumni_GalleryDTO data);
        Alumni_GalleryDTO getcovermodel(Alumni_GalleryDTO data);
        Alumni_GalleryDTO savecover(Alumni_GalleryDTO data);
        Alumni_GalleryDTO deactive(Alumni_GalleryDTO data);
    }
}
