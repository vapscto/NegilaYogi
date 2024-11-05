using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Interfaces
{
   public interface NewsPaperClippingInterface
    {
        ImageClipping_DTO savedetail(ImageClipping_DTO data);
        ImageClipping_DTO Getdetails(ImageClipping_DTO data);
        ImageClipping_DTO deactivate(ImageClipping_DTO data);
        ImageClipping_DTO editdetails(ImageClipping_DTO data);
    }
}
