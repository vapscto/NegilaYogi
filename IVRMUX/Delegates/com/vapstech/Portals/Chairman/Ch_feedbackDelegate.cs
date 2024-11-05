using System;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class Ch_feedbackDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<Ch_feedbackDTO, Ch_feedbackDTO> COMMM = new CommonDelegate<Ch_feedbackDTO, Ch_feedbackDTO>();

        public Ch_feedbackDTO getalldetails(Ch_feedbackDTO data)
        {
            return COMMM.POSTPORTALData(data, "Ch_feedbackFacade/getdata/");
        }
       

       
        public Ch_feedbackDTO onmonth(Ch_feedbackDTO data)
        {
            return COMMM.POSTPORTALData(data, "Ch_feedbackFacade/onmonth/");
        }

        
    }
}
