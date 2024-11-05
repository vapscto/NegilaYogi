
using System;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class ChairmanloginCountDelegates
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ChairmanloginCountDTO, ChairmanloginCountDTO> COMMM = new CommonDelegate<ChairmanloginCountDTO, ChairmanloginCountDTO>();
        public ChairmanloginCountDTO Getdetails(ChairmanloginCountDTO data)
        {
            return COMMM.POSTPORTALData(data, "ChairmanloginCountFacade/Getdetails/");
        }

      
       
        public ChairmanloginCountDTO getstdappcount(ChairmanloginCountDTO data)
        {
            return COMMM.POSTPORTALData(data, "ChairmanloginCountFacade/getstdappcount/");
        }
        public ChairmanloginCountDTO getstaffappcount(ChairmanloginCountDTO data)
        {
            return COMMM.POSTPORTALData(data, "ChairmanloginCountFacade/getstaffappcount/");
        }

        public ChairmanloginCountDTO GetpopupDetails(ChairmanloginCountDTO data)
        {
            return COMMM.POSTPORTALData(data, "ChairmanloginCountFacade/GetpopupDetails/");
        }
    }
    
}
