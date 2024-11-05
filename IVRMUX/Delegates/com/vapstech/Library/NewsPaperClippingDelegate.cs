using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Library
{
    public class NewsPaperClippingDelegate
    {

        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ImageClipping_DTO, ImageClipping_DTO> COMMM = new CommonDelegate<ImageClipping_DTO, ImageClipping_DTO>();
        public ImageClipping_DTO savedetail(ImageClipping_DTO data)
        {
            return COMMM.PostLibrary(data, "NewsPaperClippingFacade/savedetail/");
        }
        public ImageClipping_DTO Getdetails(ImageClipping_DTO data)
        {
            return COMMM.PostLibrary(data, "NewsPaperClippingFacade/Getdetails/");
        }
        public ImageClipping_DTO deactivate(ImageClipping_DTO data)
        {
            return COMMM.PostLibrary(data, "NewsPaperClippingFacade/deactivate/");
        }
        public ImageClipping_DTO editdetails(ImageClipping_DTO data)
        {
            return COMMM.PostLibrary(data, "NewsPaperClippingFacade/editdetails/");
        }

    }
}
