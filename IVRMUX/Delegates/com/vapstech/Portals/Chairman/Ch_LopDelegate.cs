using System;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class Ch_LopDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<Ch_LopDTO, Ch_LopDTO> COMMM = new CommonDelegate<Ch_LopDTO, Ch_LopDTO>();

        public Ch_LopDTO getalldetails(Ch_LopDTO data)
        {
            return COMMM.POSTPORTALData(data, "Ch_LopFacade/getdata/");
        }
       

       
        public Ch_LopDTO onmonth(Ch_LopDTO data)
        {
            return COMMM.POSTPORTALData(data, "Ch_LopFacade/onmonth/");
        }

        
    }
}
