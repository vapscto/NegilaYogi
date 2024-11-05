using CommonLibrary;
using PreadmissionDTOs.com.vaps.VidyaBharathi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VidyaBharathi
{
    public class VBSC_Events_Category_StudentsDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
      
        CommonDelegate<VBSC_Events_Category_StudentsDTO, VBSC_Events_Category_StudentsDTO> COMMC = new CommonDelegate<VBSC_Events_Category_StudentsDTO, VBSC_Events_Category_StudentsDTO>();
        public VBSC_Events_Category_StudentsDTO loaddata(VBSC_Events_Category_StudentsDTO data)
        {
            return COMMC.POSTDataVidyaBharathi(data, "VBSC_Events_Category_StudentsFacade/loaddata/");
        }
    
        public VBSC_Events_Category_StudentsDTO savedata(VBSC_Events_Category_StudentsDTO data)
        {
            return COMMC.POSTDataVidyaBharathi(data, "VBSC_Events_Category_StudentsFacade/savedata/");
        }
        public VBSC_Events_Category_StudentsDTO deactive(VBSC_Events_Category_StudentsDTO data)
        {
            return COMMC.POSTDataVidyaBharathi(data, "VBSC_Events_Category_StudentsFacade/deactive/");
        }
    }
}
