
using System;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class PAYCAREEmployeeDetailsDelegates
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<PAYCAREEmployeeDetailsDTO, PAYCAREEmployeeDetailsDTO> COMMM = new CommonDelegate<PAYCAREEmployeeDetailsDTO, PAYCAREEmployeeDetailsDTO>();
        public PAYCAREEmployeeDetailsDTO Getdetails(PAYCAREEmployeeDetailsDTO data)
        {
            return COMMM.POSTPORTALData(data, "PAYCAREEmployeeDetailsFacade/Getdetails/");
        }

      
       
        public PAYCAREEmployeeDetailsDTO Getemppop(PAYCAREEmployeeDetailsDTO data)
        {
            return COMMM.POSTPORTALData(data, "PAYCAREEmployeeDetailsFacade/Getemppop/");
        }
    }
    
}
