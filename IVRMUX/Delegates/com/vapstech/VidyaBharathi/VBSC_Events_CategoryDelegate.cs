using CommonLibrary;
using PreadmissionDTOs.com.vaps.VidyaBharathi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VidyaBharathi
{
    public class VBSC_Events_CategoryDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
      
        CommonDelegate<VBSC_Events_CategoryDTO, VBSC_Events_CategoryDTO> COMMC = new CommonDelegate<VBSC_Events_CategoryDTO, VBSC_Events_CategoryDTO>();
        public VBSC_Events_CategoryDTO loaddata(VBSC_Events_CategoryDTO data)
        {
            return COMMC.POSTDataVidyaBharathi(data, "VBSC_Events_CategoryFacade/loaddata/");
        }
    
        public VBSC_Events_CategoryDTO savedata(VBSC_Events_CategoryDTO data)
        {
            return COMMC.POSTDataVidyaBharathi(data, "VBSC_Events_CategoryFacade/savedata/");
        }
        public VBSC_Events_CategoryDTO deactive(VBSC_Events_CategoryDTO data)
        {
            return COMMC.POSTDataVidyaBharathi(data, "VBSC_Events_CategoryFacade/deactive/");
        }
    }
}
