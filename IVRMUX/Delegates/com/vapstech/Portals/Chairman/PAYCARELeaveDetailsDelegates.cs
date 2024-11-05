
using System;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class PAYCARELeaveDetailsDelegates
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<PAYCARELeaveDetailsDTO, PAYCARELeaveDetailsDTO> COMMM = new CommonDelegate<PAYCARELeaveDetailsDTO, PAYCARELeaveDetailsDTO>();
        public PAYCARELeaveDetailsDTO Getdetails(PAYCARELeaveDetailsDTO data)
        {
            return COMMM.POSTPORTALData(data, "PAYCARELeaveDetailsFacade/Getdetails/");
        }


        public PAYCARELeaveDetailsDTO showreport(PAYCARELeaveDetailsDTO data)
        {
            return COMMM.POSTPORTALData(data, "PAYCARELeaveDetailsFacade/showreport/");
        }
        public PAYCARELeaveDetailsDTO Getdesignation(PAYCARELeaveDetailsDTO data)
        {
            return COMMM.POSTPORTALData(data, "PAYCARELeaveDetailsFacade/Getdesignation/");
        }
    }
    

}
