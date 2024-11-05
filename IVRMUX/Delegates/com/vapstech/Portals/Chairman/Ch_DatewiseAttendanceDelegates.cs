
using System;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class Ch_DatewiseAttendanceDelegates
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<Ch_DatewiseAttendanceDTO, Ch_DatewiseAttendanceDTO> COMMM = new CommonDelegate<Ch_DatewiseAttendanceDTO, Ch_DatewiseAttendanceDTO>();
        public Ch_DatewiseAttendanceDTO Getdetails(Ch_DatewiseAttendanceDTO data)
        {
            return COMMM.POSTPORTALData(data, "Ch_DatewiseAttendanceFacade/Getdetails/");
        }

        public Ch_DatewiseAttendanceDTO getclass(Ch_DatewiseAttendanceDTO data)
        {
            return COMMM.POSTPORTALData(data, "Ch_DatewiseAttendanceFacade/getclass/");
        }
        public Ch_DatewiseAttendanceDTO Getsection(Ch_DatewiseAttendanceDTO data)
        {
            return COMMM.POSTPORTALData(data, "Ch_DatewiseAttendanceFacade/Getsection/");
        }
       
        public Ch_DatewiseAttendanceDTO Getreport(Ch_DatewiseAttendanceDTO data)
        {
            return COMMM.POSTPORTALData(data, "Ch_DatewiseAttendanceFacade/Getreport/");
        }

       

        

    }
}
